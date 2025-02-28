using Test.Player.Weapon.Projectile;

internal class BoltInstaller : ProjectileInstaller<BoltInstaller>
{
    readonly BoltSettings _settings;

    public BoltInstaller(BoltSettings settings)
    {
        _settings = settings;
    }

    protected override void InstallSpecificBindings()
    {
        Container.Bind<BoltSettings>().FromInstance(_settings);

        Container.BindInterfacesTo<BoltModel>().AsSingle();
        Container.BindInterfacesAndSelfTo<BoltPresenter>().AsSingle();

        //BoltView is injected with ZenjectBinding Component
    }

    protected override ProjectileSettings GetSettings()
    {
        return _settings;
    }
}