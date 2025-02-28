using Test.Menu;
using Zenject;

namespace Test
{
    internal class MenuInstaller : MonoInstaller<MenuInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MenuPresenter>().AsSingle();
            //View is bound in ZenjectBinding
        }
    }
}