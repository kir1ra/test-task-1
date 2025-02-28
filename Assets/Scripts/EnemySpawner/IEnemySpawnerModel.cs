using UniRx;

namespace Test.Enemy
{
    public interface IEnemySpawnerModel
    {
        IReadOnlyReactiveProperty<bool> IsSpawning { get; }

        int StartEnemyCount { get; }
        int MaxEnemyCount { get; }

        IReadOnlyReactiveProperty<float> EnemySpawnRate { get; }
        
        float EnemySpawnMargin { get; }
        
        double RespawnCheckInterval { get; }
        double RespawnMargin { get; }

        void Reset();
        void SetSpawning(bool spawning);
    }
}