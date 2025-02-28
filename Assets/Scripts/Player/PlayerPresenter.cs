using UniRx;
using Zenject;
using UnityEngine;
using Test.Signals;
using Cysharp.Threading.Tasks;

namespace Test.Player
{
    public class PlayerPresenter : IInitializable
    {
        private readonly JoystickView _joystickView;
        private readonly PlayerView _playerView;
        private readonly IPlayerModel _playerModel;
        private readonly CompositeDisposable _disposer;
        private readonly SignalBus _signalBus;

        public PlayerPresenter(JoystickView joystickView,
                               PlayerView playerView,
                               IPlayerModel playerModel,
                               CompositeDisposable disposer,
                               SignalBus signalBus)
        {
            _joystickView = joystickView;
            _playerView = playerView;
            _playerModel = playerModel;
            _disposer = disposer;
            _signalBus = signalBus;
        }

        public Vector2 Position => _playerView.Position;
        public IReadOnlyReactiveProperty<float> AttackSpeed => _playerModel.CurrentAttackSpeed;

        void IInitializable.Initialize()
        {
            _joystickView.OnInput.Where(_ => !_playerModel.IsDead.Value)
                                 .Subscribe(_playerView.Move)
                                 .AddTo(_disposer);
            
            _playerModel.CurrentHealth.CombineLatest(_playerModel.MaxHealth, (h, hMax) => h / (hMax > 0 ? hMax : 1f))
                                      .Subscribe(_playerView.UpdateHealthPercent)
                                      .AddTo(_disposer);

            _playerModel.CurrentHealth.Pairwise()
                                     .Where(pair => pair.Current < pair.Previous)
                                     .Subscribe(_ => _playerView.AnimateHit().Forget())
                                     .AddTo(_disposer);

            _playerModel.CurrentSpeed.Subscribe(_playerView.SetSpeed).AddTo(_disposer);

            _playerModel.IsDead.Where(isDead => isDead).Subscribe(_ => OnPlayerDied()).AddTo(_disposer);
        }

        void OnPlayerDied()
        {
            _signalBus.Fire<PlayerDiedSignal>();

            //TODO: Destroy Player Renderer : _playerView.AnimeDestroy...
        }

        public void TakeDamage(int damage)
        {
            _playerModel.TakeDamage(damage);
        }
        public async UniTask Revive()
        {
            await _playerView.AnimateRevive();

            _playerModel.RestoreHealth();
            _signalBus.Fire<PlayerReviveSignal>();
        }

        public void Reset()
        {
            _playerModel.Reset();

            //TODO: Restore Player Renderer : _playerView.Restore...
        }
    }
}