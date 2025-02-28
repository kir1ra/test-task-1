using System.Collections.Generic;
using UniRx;

namespace Test.Player.Weapon
{
    public class WeaponBeltModel : IWeaponBeltModel
    {
        private readonly List<WeaponPresenter> _weapons;
        private readonly ReactiveProperty<bool> _firing;
        
        IEnumerable<WeaponPresenter> IWeaponBeltModel.Weapons => _weapons;
        IReadOnlyReactiveProperty<bool> IWeaponBeltModel.Firing => _firing;

        public WeaponBeltModel()
        {
            _weapons = new List<WeaponPresenter>();
            _firing = new(false);
        }

        void IWeaponBeltModel.AddWeapon(WeaponPresenter weapon)
        {
            _weapons.Add(weapon);
        }

        void IWeaponBeltModel.SetShooting(bool shooting)
        {
            _firing.Value = shooting;
        }

        void IWeaponBeltModel.Reset()
        {
            _weapons.Clear();
        }
    }

}