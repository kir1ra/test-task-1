using Test.Enemy;
using Test.Hud;
using Test.Player;
using Test.Player.Weapon;
using Test.Signals;
using UniRx;
using Zenject;

namespace Test
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        private readonly CompositeDisposable _disposer = new();

        public override void InstallBindings()
        {
            InstallMisc();
            InstallSignals();

            InstallGame();
            InstallEnemySpawner();

            InstallWeapons();
        }

        void InstallMisc()
        {
            Container.BindInterfacesAndSelfTo<CompositeDisposable>().AsSingle();
            Container.Bind<ScreenBoundary>().AsSingle();
        }

        void InstallSignals()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<PlayerDiedSignal>();
            Container.DeclareSignal<PlayerReviveSignal>();

            Container.DeclareSignal<EnemyKilledSignal>();
        }

        void InstallGame()
        {
            Container.BindInterfacesTo<GameModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
        }


        void InstallEnemySpawner()
        {
            Container.Bind<EnemyRegistry>().AsSingle();

            Container.BindInterfacesAndSelfTo<EnemySpawner>()
                     .FromSubContainerResolve()
                     .ByInstaller<EnemySpawnerstaller>()
                     .AsSingle();
        }

        void InstallWeapons()
        {
            Container.BindInterfacesAndSelfTo<WeaponBeltController>().FromSubContainerResolve().ByInstaller<WeaponBeltInstaller>().AsSingle();
        }

        private void OnDestroy()
        {
            _disposer.Dispose();
        }
    }
}