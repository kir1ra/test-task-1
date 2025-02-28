using UniRx;

namespace Test.Player
{
    public interface IPlayerModel
    {
        IReadOnlyReactiveProperty<int> CurrentHealth { get; }
        IReadOnlyReactiveProperty<int> MaxHealth { get; }
        IReadOnlyReactiveProperty<bool> IsDead { get; }

        IReadOnlyReactiveProperty<float> CurrentSpeed { get; }
        IReadOnlyReactiveProperty<float> MaxSpeed { get; }

        IReadOnlyReactiveProperty<float> CurrentAttackSpeed { get; }

        void TakeDamage(int damage);
        void RestoreHealth();
        void Reset();
    }
}