using Test.Player.Weapon;
using Zenject;

namespace Test
{
    public class WeaponBeltInstaller : Installer<WeaponBeltInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<WeaponBeltController>().AsSingle();
            Container.BindInterfacesTo<WeaponBeltModel>().AsSingle();

            Container.BindFactory<CrossbowPresenter, CrossbowPresenter.Factory>()
                     .FromSubContainerResolve().ByInstaller<CrossbowInstaller>()
                     .AsSingle();
        }
    }
}