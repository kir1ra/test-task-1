using UniRx;

namespace Test.Enemy
{
    public interface IEnemyModel
    {
        IReadOnlyReactiveProperty<int> CurrentHealth { get; }
        IReadOnlyReactiveProperty<int> MaxHealth { get; }

        IReadOnlyReactiveProperty<float> CurrentSpeed { get; }
        IReadOnlyReactiveProperty<float> MaxSpeed { get; }
        IReadOnlyReactiveProperty<bool> IsDead { get; }

        void TakeDamage(int damage);
        void Reset();
        void RestoreHealth();
    }
}