using Test.Player.Weapon;
using Test.Player.Weapon.Projectile;
using UnityEngine;
using Zenject;


public class CrossbowInstaller : WeaponInstaller<CrossbowInstaller>//Installer<CrossbowInstaller>
{
    readonly CrossbowSettings _settings;

    public CrossbowInstaller(CrossbowSettings settings)
    {
        _settings = settings;
    }

    protected override void InstallSpecificBindings()
    {
        //Container.Bind<WeaponSettings>().FromInstance(_settings);
        Container.Bind<CrossbowSettings>().FromInstance(_settings);

        Container.BindInterfacesAndSelfTo<CrossbowPresenter>().AsSingle();
        Container.BindInterfacesTo<WeaponModel>().AsSingle();

        InstallProjectileFactoryBindings();
    }

    public void InstallProjectileFactoryBindings()
    {
        Container.Bind<BoltSettings>()
                 .FromInstance(_settings.ProjectileSettings as BoltSettings)
                 .WhenInjectedInto<BoltInstaller>();

        Container.BindFactory<Vector2, BoltPresenter, BoltPresenter.Factory>()
            .FromPoolableMemoryPool<Vector2, BoltPresenter, BoltPresenter.MemoryPool>(poolBinder => poolBinder
                .WithInitialSize(_settings.BurstCount)
                .FromSubContainerResolve().ByNewPrefabInstaller<BoltInstaller>(_settings.ProjectileSettings.Prefab)
                .UnderTransformGroup("Projectiles/Bolts")
                );
    }

    protected override WeaponSettings GetSettings()
    {
        return _settings;
    }
}
