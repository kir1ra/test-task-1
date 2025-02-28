using Test.Enemy;
using UnityEngine;
using Zenject;
namespace Test
{
    public class EnemySpawnerstaller : Installer<EnemySpawnerstaller>
    {
        readonly EnemySpawnerSettings _settings;

        public EnemySpawnerstaller(EnemySpawnerSettings settings)
        {
            _settings = settings;
        }

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<EnemySpawnerModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle();


            Container.BindInstance(_settings.EnemyPreset.Data).AsSingle();
            Container.BindInstance(_settings.EnemyPreset.Attack).AsSingle();

            Container.BindFactory<Vector2, EnemyPresenter, EnemyPresenter.Factory>()
                .FromPoolableMemoryPool<Vector2, EnemyPresenter, EnemyPresenter.MemoryPool>(poolBinder => poolBinder
                    .WithInitialSize(_settings.StartEnemyCount)
                    .FromSubContainerResolve().ByNewPrefabInstaller<EnemyInstaller>(_settings.EnemyPreset.Prefab)
                    .UnderTransformGroup("Enemies"));
        }

    }


}