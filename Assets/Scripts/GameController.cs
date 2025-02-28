using System;
using Cysharp.Threading.Tasks;
using Test.Enemy;
using Test.Player;
using Test.Player.Weapon;
using Test.Signals;
using UniRx;
using UnityEngine;
using Zenject;

namespace Test
{
    public class GameController : IInitializable, IDisposable
    {
        readonly SignalBus _signalBus;
        readonly CompositeDisposable _disposer;
        readonly EnemySpawner _enemySpawner;
        readonly PlayerPresenter _player;
        readonly IGameModel _gameModel;
        readonly WeaponBeltController _weaponBeltController;
        readonly BackgroundView _backgroundView;


        private IDisposable _timerSubscription;
        public IReadOnlyReactiveProperty<GameStates> State;

        public GameController(SignalBus signalBus,
                              CompositeDisposable disposer,
                              EnemySpawner enemySpawner,
                              PlayerPresenter player,
                              IGameModel gameModel,
                              WeaponBeltController weaponBeltController,
                              BackgroundView backgroundView)
        {
            _signalBus = signalBus;
            _disposer = disposer;
            _enemySpawner = enemySpawner;
            _player = player;
            _gameModel = gameModel;
            _weaponBeltController = weaponBeltController;
            _backgroundView = backgroundView;

            State = _gameModel.State.ToReactiveProperty();
        }

        void IInitializable.Initialize()
        {
            _signalBus.Subscribe<PlayerDiedSignal>(StopGame);
            _signalBus.Subscribe<EnemyKilledSignal>(_gameModel.IncrementEnemyKillCount);

            _player.ObserveEveryValueChanged(x => x.Position).Subscribe(_backgroundView.MoveTo).AddTo(_disposer);
        }

        public void StartNewGame()
        {
            ResetGame();

            //Give a crossbow to the player on game start
            _weaponBeltController.AddWeapon(WeaponType.Crossbow);
            _weaponBeltController.SetShooting(true);
            _enemySpawner.SpawnInitialWave().ContinueWith(_enemySpawner.StartSpawning).Forget();

            StartTimer();

            _gameModel.SetState(GameStates.Playing);
        }

        private void StopGame()
        {
            _gameModel.SetState(GameStates.GameOver);
            _enemySpawner.StopSpawning();
            _weaponBeltController.SetShooting(false);

            _timerSubscription?.Dispose();
            _timerSubscription = null;
        }

        private void ResetGame()
        {
            _gameModel.Reset();
            _enemySpawner.Reset();
            _player.Reset();
            _weaponBeltController.Reset();
        }

        public async UniTaskVoid ReviveAndResume()
        {
            await _player.Revive();

            _weaponBeltController.SetShooting(true);
            _enemySpawner.StartSpawning();

            StartTimer();

            _gameModel.SetState(GameStates.Playing);
        }

        void StartTimer()
        {
            _timerSubscription = Observable.EveryUpdate()
                                           .Select(_ => Time.deltaTime)
                                           .Subscribe(_gameModel.IncreaseSurvivalTime)
                                           .AddTo(_disposer);
        }

        void IDisposable.Dispose()
        {
            _signalBus.Unsubscribe<PlayerDiedSignal>(StopGame);
            _signalBus.Unsubscribe<EnemyKilledSignal>(_gameModel.IncrementEnemyKillCount);
        }
    }
}