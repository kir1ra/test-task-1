using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Test.Enemy;
using Test.Signals;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Test.Player.Weapon.Projectile
{
    public class BoltPresenter : IPoolable<Vector2, IMemoryPool>, IInitializable, ITickable, IDisposable
    {
        private readonly DebugSettings _debugSettings;
        private readonly BoltView _boltView;
        private readonly IBoltModel _boltModel;
        private readonly PlayerPresenter _player;
        private readonly ScreenBoundary _screenBoundary;
        private readonly EnemyRegistry _enemyRegistry;
        private readonly CompositeDisposable _disposer;
        private readonly SignalBus _signalBus;

        private IMemoryPool _memoryPool;
        private CancellationTokenSource _autoDisposeCTS;
        private bool _autoDisposePending;

        public BoltPresenter(DebugSettings debugSettings,
                             BoltView boltView,
                             IBoltModel boltModel,
                             PlayerPresenter player,
                             ScreenBoundary screenBoundary,
                             EnemyRegistry enemyRegistry,
                             CompositeDisposable disposer,
                             SignalBus signalBus)
        {
            _debugSettings = debugSettings;
            _boltView = boltView;
            _boltModel = boltModel;
            _player = player;
            _screenBoundary = screenBoundary;
            _enemyRegistry = enemyRegistry;
            _disposer = disposer;
            _signalBus = signalBus;
        }

        void IInitializable.Initialize()
        {
            _boltView.OnEnemyHit
                     .Select(enemyId => _enemyRegistry[enemyId])
                     .Subscribe(enemy => enemy?.TakeDamage(_boltModel.Damage))
                     .AddTo(_disposer);

            _signalBus.Subscribe<PlayerDiedSignal>(Dispose);
        }

        void ITickable.Tick()
        {
            _boltView.Move(_boltModel.Direction);

            ReflectOffScreenBoundaries();
        }

        async UniTaskVoid ScheduleDelayDispose(float delayInSeconds)
        {
            if (_autoDisposePending)
                return;
            
            try
            {
                _autoDisposePending = true;
                await UniTask.WaitForSeconds(delayInSeconds, cancellationToken: (_autoDisposeCTS = new CancellationTokenSource()).Token);
                _autoDisposePending = false;
                Dispose();
            }
            catch (OperationCanceledException) 
            {

            }
            finally
            {
                _autoDisposePending = false;
            }
            
        }

        void ReflectOffScreenBoundaries()
        {
            Vector2 pos = _boltView.Position;

            Vector2 normal = Vector2.zero;

            if(pos.x < _player.Position.x + _screenBoundary.Left)
            {
                pos.x = _player.Position.x + _screenBoundary.Left;
                normal += Vector2.right;
            }
            else if (pos.x > _player.Position.x + _screenBoundary.Right)
            {
                pos.x = _player.Position.x + _screenBoundary.Right;
                normal += Vector2.left;
            }

            if (pos.y < _player.Position.y + _screenBoundary.Bottom)
            {
                pos.y = _player.Position.y + _screenBoundary.Bottom;
                normal += Vector2.up;
            }
            else if (pos.y > _player.Position.y + _screenBoundary.Top)
            {
                pos.y = _player.Position.y + _screenBoundary.Top;
                normal += Vector2.down;
            }

            _boltView.Teleport(pos);

            if(normal != Vector2.zero)
            {
                var reflectedDirection = Vector2.Reflect(_boltModel.Direction, normal);


                if (_debugSettings.EditorDrawLine)
                    Debug.DrawLine(_boltView.Position, _boltView.Position + reflectedDirection * 30, Color.white, 1f);

                if (_enemyRegistry.Enemies.Count() > 0)
                {
                    //Find the direction to the most aligned enemy to the reflected direction.
                    //So that the reflection is as realist as possible
                    var closestDirectionToEnemy = _enemyRegistry.Enemies                        
                        .Select((enemy) => (enemy.Position - pos).normalized)
                    //Filter to enemies ahead, with a selective angle from the reflected direction, avoid weird angles
                        .Where(dirToEnemy => Vector2.Dot(reflectedDirection, dirToEnemy) >= 1 - _boltModel.BounceAimbotFactor)
                    //This is for debug purpose, display all considered enemies
                        .Select(dirToEnemy =>
                        {
                            if (_debugSettings.EditorDrawLine)
                                Debug.DrawLine(_boltView.Position, _boltView.Position + dirToEnemy * 15, Color.yellow, 1f);
                            
                            return dirToEnemy;
                        })
                        .OrderBy(dirToEnemy => Vector2.Angle(reflectedDirection, dirToEnemy))
                        .FirstOrDefault();

                    if (closestDirectionToEnemy != Vector2.zero)
                        reflectedDirection = closestDirectionToEnemy;
                }

                if (_debugSettings.EditorDrawLine)
                    Debug.DrawLine(_boltView.Position, _boltView.Position + reflectedDirection * 5, Color.blue, 1f);

                _boltModel.Direction = reflectedDirection;
            }
        }

        void IPoolable<Vector2, IMemoryPool>.OnSpawned(Vector2 spawnPosition, IMemoryPool memoryPool)
        {
            _memoryPool = memoryPool;
            _boltModel.Direction = Random.insideUnitCircle.normalized;
            _boltView.Teleport(spawnPosition);
            _boltView.SetSpeed(_boltModel.Speed);

            ScheduleDelayDispose(_boltModel.Lifetime).Forget();
        }

        void IPoolable<Vector2, IMemoryPool>.OnDespawned()
        {
            _memoryPool = null;
        }
        public void Dispose()
        {
            if (_autoDisposePending)
                _autoDisposeCTS?.Cancel();

            _autoDisposeCTS?.Dispose();
            _autoDisposeCTS = null;

            _memoryPool?.Despawn(this);
        }

        ~BoltPresenter() 
        {
            _signalBus.Unsubscribe<PlayerDiedSignal>(Dispose);
        }

        public class Factory : PlaceholderFactory<Vector2, BoltPresenter>
        {
        }


        /// <summary>
        ///This custom pool replicate the gameobject active state management from the MonoPool
        ///without requiring the Facade (here Presenter) to be a MonoBehaviour
        /// </summary>
        public class MemoryPool : PoolableMemoryPool<Vector2, IMemoryPool, BoltPresenter>
        {
            protected override void OnCreated(BoltPresenter item)
            {
                base.OnCreated(item);
                item._boltView.gameObject.SetActive(false);
            }

            protected override void OnDestroyed(BoltPresenter item)
            {
                base.OnDestroyed(item);
                GameObject.Destroy(item._boltView.gameObject);
            }

            protected override void OnDespawned(BoltPresenter item)
            {
                base.OnDespawned(item);
                item._boltView.gameObject.SetActive(false);
            }

            protected override void Reinitialize(Vector2 startPosition, IMemoryPool memoryPool, BoltPresenter item)
            {
                item._boltView.gameObject.SetActive(true);
                base.Reinitialize(startPosition, memoryPool, item);
            }
        }
    }
}