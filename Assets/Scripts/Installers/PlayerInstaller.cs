using Test.Player;
using Zenject;

namespace Test
{
    public class PlayerInstaller : MonoInstaller<PlayerInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<PlayerModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerPresenter>().AsSingle().NonLazy();
            ///PlayerView binding with Zenject Binding Component.
        }
    }

}