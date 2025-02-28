using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Test.Player;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;
using Random = UnityEngine.Random;

namespace Test.Enemy
{
    public class EnemySpawner : IInitializable
    {
        readonly ScreenBoundary _screenBoundary;
        readonly EnemyPresenter.Factory _enemyFactory;
        readonly EnemyRegistry _enemyRegistry;
        readonly PlayerPresenter _player;
        private CompositeDisposable _disposer;
        private IEnemySpawnerModel _enemySpawnerModel;

        public EnemySpawner(ScreenBoundary screenBoundary,
                            EnemyPresenter.Factory enemyFactory,
                            EnemyRegistry enemyRegistry,
                            PlayerPresenter player,
                            CompositeDisposable disposer,
                            IEnemySpawnerModel enemySpawnerModel)
        {
            _enemyFactory = enemyFactory;
            _enemyRegistry = enemyRegistry;
            _player = player;
            _disposer = disposer;
            _enemySpawnerModel = enemySpawnerModel;
            _screenBoundary = screenBoundary;
        }

        void IInitializable.Initialize()
        {
            _enemySpawnerModel.IsSpawning
                .Select(isSpawning =>
                {
                    if (isSpawning)
                    {
                        return Observable.Interval(TimeSpan.FromSeconds(_enemySpawnerModel.RespawnCheckInterval)).Select(_ => Unit.Default);
                    }
                    else
                    {
                        return Observable.Empty<Unit>();
                    }
                })
                .Switch()
                .SelectMany(_ => _enemyRegistry.Enemies)
                .Where((enemy) =>
                {
                    Vector2 distanceToPlayer = (enemy.Position - _player.Position);
                    return Mathf.Abs(distanceToPlayer.x) > _screenBoundary.ExtentWidth + _enemySpawnerModel.RespawnMargin
                        || Mathf.Abs(distanceToPlayer.y) > _screenBoundary.ExtentHeight + _enemySpawnerModel.RespawnMargin;
                })
                .Subscribe((enemy) => RespawnEnemy(enemy))
                .AddTo(_disposer);

            //We spawn a new enemy every X seconds, when spawning is active
            _enemySpawnerModel.IsSpawning
                .Select(isSpawning =>
                {
                    if (isSpawning && _enemyRegistry.Enemies.Count() < _enemySpawnerModel.MaxEnemyCount)
                    {
                        return Observable.Interval(TimeSpan.FromSeconds(1f / _enemySpawnerModel.EnemySpawnRate.Value)).Select(_ => Unit.Default);
                    }
                    else
                    {
                        return Observable.Empty<Unit>();
                    }
                })
                .Switch()
                .Subscribe(_ => SpawnEnemy())
                .AddTo(_disposer);
        }

        public void StartSpawning()
        {
            _enemySpawnerModel.SetSpawning(true);
        }
        public void StopSpawning()
        {
            _enemySpawnerModel.SetSpawning(false);
        }
      
        public void Reset()
        {
            _enemySpawnerModel.Reset();
            while (_enemyRegistry.Enemies.Count() > 0)
                _enemyRegistry.Enemies.First().Dispose();            
        }

        public async UniTask SpawnInitialWave()
        {
            //We spawn a bunch of enemies at start, with a little delay between each.
            for (int i = 0; i < _enemySpawnerModel.StartEnemyCount; i++)
            {
                await UniTask.WaitForSeconds(5f/ _enemySpawnerModel.StartEnemyCount);
                SpawnEnemy();
            }
        }

        void SpawnEnemy()
        {
            _enemyFactory.Create(GetRandomSpawnPosition());
        }

        async void RespawnEnemy(EnemyPresenter enemy)
        {
            Assert.IsTrue(_enemyRegistry.Enemies.Contains(enemy));

            //We could just teleport the enemy to a new start position, but then we would need to manually redo the whole spawn process (model reset, etc).
            //It is simply easier to just dispose and recreate, as this will trigger OnSpawn.
            //The MemoryPool mitigate the spike.

            //We can perform the dispose/create just yet as this would modify the registry while being iterated.
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);

            enemy.Dispose();
            SpawnEnemy();
        }

        Vector2 GetRandomSpawnPosition()
        {
            var screenRandomSide = Random.Range(0, 4);
            var posOnSide = Random.Range(0, 1.0f);

            Vector2 deltaPosition = Vector2.zero;

            switch (screenRandomSide)
            {
                case 0:
                    // top
                    {
                        deltaPosition = new Vector2(
                            _screenBoundary.Left + posOnSide * _screenBoundary.Width,
                            _screenBoundary.Top + _enemySpawnerModel.EnemySpawnMargin);
                        break;
                    }
                case 1:
                    // right
                    {
                        deltaPosition = new Vector2(
                            _screenBoundary.Right + _enemySpawnerModel.EnemySpawnMargin,
                            _screenBoundary.Top - posOnSide * _screenBoundary.Height);
                        break;
                    }
                case 2:
                    // bottom
                    {
                        deltaPosition = new Vector2(
                            _screenBoundary.Left + posOnSide * _screenBoundary.Width,
                            _screenBoundary.Bottom - _enemySpawnerModel.EnemySpawnMargin);
                        break;
                    }
                case 3:
                    // left
                    {
                        deltaPosition = new Vector2(
                            _screenBoundary.Left - _enemySpawnerModel.EnemySpawnMargin,
                            _screenBoundary.Top - posOnSide * _screenBoundary.Height);
                        break;
                    }
            }
            return _player.Position + deltaPosition;
        }
    }
}