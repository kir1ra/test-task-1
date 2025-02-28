using UniRx;
using Zenject;

namespace Test.Player.Weapon
{
    public class WeaponBeltController : IInitializable
    {
        readonly CrossbowPresenter.Factory _crossbowFactory;
        readonly IWeaponBeltModel _weaponBeltModel;

        public WeaponBeltController(CrossbowPresenter.Factory crossbowFactory,
                                    IWeaponBeltModel weaponBeltModel)
        {
            _crossbowFactory = crossbowFactory;
            _weaponBeltModel = weaponBeltModel;
        }

        void IInitializable.Initialize()
        {
            _weaponBeltModel.Firing
                .Select(isEnabled =>
                {
                    if (isEnabled)
                    {
                        return Observable.EveryUpdate().Select(_ => Unit.Default);
                    }
                    else
                    {
                        return Observable.Empty<Unit>();
                    }
                })
                .Switch()
                .SelectMany(_ => _weaponBeltModel.Weapons)
                .Subscribe(weapon => weapon.Update());
        }

        public void SetShooting(bool shootingEnabled)
        {
            _weaponBeltModel.SetShooting(shootingEnabled);
        }

        internal void AddWeapon(WeaponType weaponType)
        {
            WeaponPresenter weapon = null;
            switch (weaponType)
            {
                case WeaponType.Crossbow:
                    weapon = _crossbowFactory.Create();
                    break;
                default:
                    break;
            }
            _weaponBeltModel.AddWeapon(weapon);
        }

        internal void Reset()
        {
            _weaponBeltModel.Reset();
        }
    }
}