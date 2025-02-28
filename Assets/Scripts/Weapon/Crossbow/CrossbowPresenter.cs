using Cysharp.Threading.Tasks;
using Test.Player.Weapon.Projectile;
using UniRx;
using Zenject;

namespace Test.Player.Weapon
{
    public class CrossbowPresenter : WeaponPresenter
    {
        readonly BoltPresenter.Factory _projectileFactory;

        public CrossbowPresenter(PlayerPresenter player,
                                 CompositeDisposable disposer,
                                 IWeaponModel weaponModel,
                                 BoltPresenter.Factory projectileFactory)
            : base(player,
                   disposer,
                   weaponModel)
        {
            _projectileFactory = projectileFactory;
        }

        protected async override UniTaskVoid Fire()
        {
            float burstCount = _weaponModel.BurstCount;
            float burstInterval = _weaponModel.BurstInterval;

            for (int burstId = 0; burstId < burstCount; burstId++)
            {
                _projectileFactory.Create(_player.Position);
                // Apply interval between each projectile in the burst.
                if (burstInterval > 0 && burstId < burstCount - 1)
                    await UniTask.WaitForSeconds(burstInterval);
            }
        }

        public class Factory : PlaceholderFactory<CrossbowPresenter>
        {

        }
    }
}