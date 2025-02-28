using Test.Hud;
using Zenject;

namespace Test
{
    internal class HudInstaller : MonoInstaller<HudInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<HudPresenter>().AsSingle();
            //HudView is bound in ZenjectBinding
        }
    }
}