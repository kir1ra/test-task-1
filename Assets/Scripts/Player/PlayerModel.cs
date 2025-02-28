using System;
using UniRx;
using UnityEngine;

namespace Test.Player
{
    public class PlayerModel : IPlayerModel
    {
        private readonly Settings _settings;
        private readonly CompositeDisposable _disposer;

        private readonly ReactiveProperty<int> _currentHealth;
        private readonly ReactiveProperty<int> _maxHealth;

        private readonly ReactiveProperty<float> _currentSpeed;
        private readonly ReactiveProperty<float> _maxSpeed;

        private readonly ReactiveProperty<float> _currentAttackSpeed;


        public IReadOnlyReactiveProperty<int> CurrentHealth => _currentHealth;
        public IReadOnlyReactiveProperty<int> MaxHealth => _maxHealth;

        public IReadOnlyReactiveProperty<float> CurrentSpeed => _currentSpeed;
        public IReadOnlyReactiveProperty<float> MaxSpeed => _maxSpeed;
        
        public IReadOnlyReactiveProperty<float> CurrentAttackSpeed => _currentAttackSpeed;

        public IReadOnlyReactiveProperty<bool> IsDead { get; private set; }
        
        public PlayerModel(Settings settings,
                           CompositeDisposable disposer)
        {
            _settings = settings;
            _disposer = disposer;

            _maxHealth = new(_settings.StartHealth);
            _currentHealth = new(_settings.StartHealth);

            _maxSpeed = new(_settings.StartSpeed);
            _currentSpeed = new(_settings.StartSpeed);

            _currentAttackSpeed = new(_settings.StartAttackSpeed);

            IsDead = _currentHealth.Select(h => h <= 0).ToReactiveProperty();
        }

        void IPlayerModel.TakeDamage(int attackDamage)
        {
            int currentHealth = _currentHealth.Value;
            currentHealth = Mathf.Max(0, currentHealth - attackDamage);
            _currentHealth.Value = currentHealth;
        }
        void IPlayerModel.RestoreHealth()
        {
            _currentHealth.Value = _maxHealth.Value;
        }

        void IPlayerModel.Reset()
        {
            _currentHealth.Value = _settings.StartHealth;
            _maxHealth.Value = _settings.StartHealth;

            _maxSpeed.Value = _settings.StartSpeed;
            _currentSpeed.Value = _settings.StartSpeed;

            _currentAttackSpeed.Value = _settings.StartAttackSpeed;
        }


        [Serializable]
        public class Settings
        {
            [Min(1)]
            public int StartHealth = 100;

            [Min(.2f)]
            public int StartAttackSpeed = 1;

            [Range(float.Epsilon, 5f)]
            public float StartSpeed = 3;
        }

    }
}