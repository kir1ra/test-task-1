using UniRx;

namespace Test.Player.Weapon
{
    public interface IWeaponModel
    {
        float BurstCount { get; }
        float BurstInterval { get; }

        float Cooldown { get; }

        ReactiveProperty<float> TimeUntilNextFire { get; }
    }
}
