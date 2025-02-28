using Test.Enemy;
using Zenject;

namespace Test
{
    public class EnemyInstaller : Installer<EnemyInstaller>
    {
        public override void InstallBindings()
        {
            int guid = System.Guid.NewGuid().GetHashCode();
            Container.BindInstance(guid).WithId("EnemyId");

            Container.BindInterfacesTo<EnemyModel>().AsSingle();

            Container.BindInterfacesAndSelfTo<EnemyStateManager>().AsSingle();
            Container.Bind<EnemyStateIdle>().AsSingle();
            Container.Bind<EnemyStateAttack>().AsSingle();
            Container.Bind<EnemyStateChase>().AsSingle();

            Container.BindInterfacesAndSelfTo<EnemyPresenter>().AsSingle();

            //EnemyView is injected with ZenjectBinding Component
        }

    }
}