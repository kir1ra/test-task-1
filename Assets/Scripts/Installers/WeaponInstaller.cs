using Test.Player.Weapon;
using Zenject;

public abstract class WeaponInstaller<T> : Installer<T> where T : Installer<T>
{
    public override void InstallBindings()
    {
        Container.Bind<WeaponSettings>().FromInstance(GetSettings());
        InstallSpecificBindings();
    }

    protected abstract void InstallSpecificBindings();
    protected abstract WeaponSettings GetSettings();
}