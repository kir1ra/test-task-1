using System;
using Cysharp.Threading.Tasks;
using Test.Signals;
using UniRx;
using UnityEngine;
using Zenject;

namespace Test.Enemy
{
    public class EnemyPresenter : IPoolable<Vector2, IMemoryPool>, IInitializable, IDisposable
    {
        [Inject(Id = "EnemyId")] public int EnemyId;

        private SignalBus _signalBus;
        private EnemyView _enemyView;
        private IEnemyModel _enemyModel;
        private EnemyRegistry _registry;
        private EnemyStateManager _stateManager;
        private CompositeDisposable _disposer;

        public Vector2 Position => _enemyView.Position;
        private IMemoryPool _memoryPool;

        public EnemyPresenter(SignalBus signalBus,
                              EnemyView enemyView,
                              IEnemyModel enemyModel,
                              EnemyRegistry registry,
                              EnemyStateManager stateManager,
                              CompositeDisposable disposer)
        {
            _signalBus = signalBus;
            _enemyView = enemyView;
            _enemyModel = enemyModel;
            _registry = registry;
            _stateManager = stateManager;
            _disposer = disposer;
        }

        void IInitializable.Initialize()
        {
            _enemyModel.CurrentHealth.CombineLatest(_enemyModel.MaxHealth, (c, m) => c / (m > 0 ? m : 1f))
                                     .Subscribe(_enemyView.UpdateHealthPercent)
                                     .AddTo(_disposer);

            _enemyModel.CurrentHealth.Pairwise()
                                     .Where(pair => pair.Current < pair.Previous)
                                     .Subscribe(_ => _enemyView.AnimateHit().Forget())
                                     .AddTo(_disposer);

            _enemyModel.CurrentSpeed.Subscribe(_enemyView.SetSpeed).AddTo(_disposer);

            _enemyModel.IsDead.Where(isDead => isDead).Subscribe(_ => OnEnemyDied().Forget()).AddTo(_disposer);
        }

        async UniTaskVoid OnEnemyDied()
        {
            _signalBus.Fire<EnemyKilledSignal>();
            _stateManager.ChangeState(EnemyStates.Idle);
            await _enemyView.AnimateDeath();
            Dispose();
        }

        public void TakeDamage(int damage)
        {
            _enemyModel.TakeDamage(damage);
        }
        public void Heal()
        {
            _enemyModel.RestoreHealth();
        }

        public void Reset()
        {
            _enemyModel.Reset();

            //TODO: Restore Player Renderer : _playerView.Restore...
        }

        void IPoolable<Vector2, IMemoryPool>.OnSpawned(Vector2 startPosition, IMemoryPool memoryPool)
        {
            _memoryPool = memoryPool;

            _registry.AddEnemy(EnemyId, this);

            _enemyView.Teleport(startPosition);
            _enemyModel.Reset();
            _enemyView.Reset();

            _stateManager.ChangeState(EnemyStates.Chase);
        }
        void IPoolable<Vector2, IMemoryPool>.OnDespawned()
        {
            _registry.RemoveEnemy(EnemyId);
            _memoryPool = null;
        }
        public void Dispose()
        {
            _memoryPool?.Despawn(this);
        }

        public class Factory : PlaceholderFactory<Vector2, EnemyPresenter>
        {
        }

        /// <summary>
        ///This custom pool replicate the gameobject active state management from the MonoPool
        ///without requiring the Facade (here Presenter) to be a MonoBehaviour
        /// </summary>
        public class MemoryPool : PoolableMemoryPool<Vector2, IMemoryPool, EnemyPresenter>
        {
            protected override void OnCreated(EnemyPresenter item)
            {
                base.OnCreated(item);
                item._enemyView.gameObject.SetActive(false);
            }

            protected override void OnDestroyed(EnemyPresenter item)
            {
                base.OnDestroyed(item);
                GameObject.Destroy(item._enemyView.gameObject);
            }

            protected override void OnDespawned(EnemyPresenter item)
            {
                base.OnDespawned(item);
                item._enemyView.gameObject.SetActive(false);
            }

            protected override void Reinitialize(Vector2 startPosition, IMemoryPool memoryPool, EnemyPresenter item)
            {
                item._enemyView.gameObject.SetActive(true);
                base.Reinitialize(startPosition, memoryPool, item);
            }
        }
    }
}