using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Test.Enemy
{
    public class EnemyModel : IEnemyModel
    {
        [Inject(Id = "EnemyId")] public readonly int EnemyId;

        private readonly Settings _settings;

        private readonly ReactiveProperty<int> _currentHealth;
        private readonly ReactiveProperty<int> _maxHealth;

        private readonly ReactiveProperty<float> _currentSpeed;
        private readonly ReactiveProperty<float> _maxSpeed;


        public IReadOnlyReactiveProperty<int> CurrentHealth => _currentHealth;
        public IReadOnlyReactiveProperty<int> MaxHealth => _maxHealth;

        public IReadOnlyReactiveProperty<float> CurrentSpeed => _currentSpeed;
        public IReadOnlyReactiveProperty<float> MaxSpeed => _maxSpeed;

        public IReadOnlyReactiveProperty<bool> IsDead { get; private set; }


        public EnemyModel(
            Settings settings)
        {
            _settings = settings;

            _maxHealth = new(_settings.StartHealth);
            _currentHealth = new(_maxHealth.Value);

            _maxSpeed = new(_settings.StartSpeed);
            _currentSpeed = new(_maxSpeed.Value);

            IsDead = _currentHealth.Select(h => h <= 0).ToReactiveProperty();
        }

        void IEnemyModel.TakeDamage(int attackDamage)
        {
            int currentHealth = _currentHealth.Value;
            currentHealth = Mathf.Max(0, currentHealth - attackDamage);
            _currentHealth.Value = currentHealth;
        }
        void IEnemyModel.RestoreHealth()
        {
            _currentHealth.Value = _maxHealth.Value;
        }

        void IEnemyModel.Reset()
        {
            _currentHealth.Value = _settings.StartHealth;
            _maxHealth.Value = _settings.StartHealth;

            _maxSpeed.Value = _settings.StartSpeed;
            _currentSpeed.Value = _settings.StartSpeed;
        }


        [Serializable]
        public class Settings
        {
            [Min(1)]
            public int StartHealth;

            [Range(float.Epsilon, 5f)]
            public float StartSpeed;
        }

    }
}