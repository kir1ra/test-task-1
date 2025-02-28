using UniRx;

namespace Test.Player.Weapon
{
    public class WeaponModel : IWeaponModel
    {
        readonly WeaponSettings _settings;

        float IWeaponModel.BurstCount => _settings.BurstCount;
        float IWeaponModel.BurstInterval => _settings.BurstInterval;

        float IWeaponModel.Cooldown => _settings.Cooldown;

        public ReactiveProperty<float> TimeUntilNextFire { get; }

        public WeaponModel(WeaponSettings settings)
        {
            _settings = settings;

            TimeUntilNextFire = new(1);
        }
    }
}