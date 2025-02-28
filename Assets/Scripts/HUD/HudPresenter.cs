using System;
using Test.Enemy;
using UniRx;
using UnityEngine;
using Zenject;

namespace Test.Hud
{
    public class HudPresenter: IInitializable
    {
        private readonly IGameModel _gameModel;
        private readonly HudView _hudView;
        private readonly CompositeDisposable _disposer;
        private readonly EnemyRegistry _enemyRegistry;

        public HudPresenter(IGameModel gameModel,
                            HudView hudView,
                            CompositeDisposable disposer,
                            EnemyRegistry enemyRegistry)
        {
            _hudView = hudView;
            _disposer = disposer;
            _enemyRegistry = enemyRegistry;
            _gameModel = gameModel;
        }

        void IInitializable.Initialize()
        {
            _gameModel.SurvivalTime.Select(t => Mathf.FloorToInt(t))
                                   .DistinctUntilChanged()
                                   .Select(t => TimeSpan.FromSeconds(t))
                                   .Subscribe(_hudView.UpdateTimeWatch)
                                   .AddTo(_disposer);

            _gameModel.EnemyKillCount.Subscribe(_hudView.UpdateKillCounter)
                                     .AddTo(_disposer);


            _enemyRegistry.EnemyCount.Subscribe(_hudView.UpdateRemainingEnemiesCounter)
                                     .AddTo(_disposer);
        }
    }
}