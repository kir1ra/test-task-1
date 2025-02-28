using Test.Player.Weapon.Projectile;
using Zenject;

public abstract class ProjectileInstaller<T> : Installer<T> where T : Installer<T>
{
    public override void InstallBindings()
    {
        Container.Bind<ProjectileSettings>().FromInstance(GetSettings());
        InstallSpecificBindings();
    }

    protected abstract void InstallSpecificBindings();
    protected abstract ProjectileSettings GetSettings();
}