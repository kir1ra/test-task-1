using System.Collections.Generic;
using UniRx;

namespace Test.Player.Weapon
{
    public interface IWeaponBeltModel
    {
        IReadOnlyReactiveProperty<bool> Firing { get; }
        IEnumerable<WeaponPresenter> Weapons { get; }

        void AddWeapon(WeaponPresenter weapon);
        void Reset();
        void SetShooting(bool shooting);
    }
}