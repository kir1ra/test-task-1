using UniRx;

namespace Test.Enemy
{
    public class EnemySpawnerModel : IEnemySpawnerModel
    {
        readonly EnemySpawnerSettings _settings;

        readonly ReactiveProperty<bool> _isSpawning;

        readonly int _startEnemyCount;
        readonly int _maxEnemyCount;

        readonly ReactiveProperty<float> _enemySpawnRate;
        //public float _enemySpawnAmountIncrement;

        readonly float _enemySpawnMargin;

        readonly float _respawnCheckInterval;
        readonly float _respawnMargin;

        IReadOnlyReactiveProperty<bool> IEnemySpawnerModel.IsSpawning => _isSpawning;

        int IEnemySpawnerModel.StartEnemyCount => _startEnemyCount;
        int IEnemySpawnerModel.MaxEnemyCount => _maxEnemyCount;

        IReadOnlyReactiveProperty<float> IEnemySpawnerModel.EnemySpawnRate => _enemySpawnRate;

        float IEnemySpawnerModel.EnemySpawnMargin => _enemySpawnMargin;

        double IEnemySpawnerModel.RespawnCheckInterval => _respawnCheckInterval;
        double IEnemySpawnerModel.RespawnMargin => _respawnMargin;

        public EnemySpawnerModel(EnemySpawnerSettings settings)
        {
            _settings = settings;

            _isSpawning = new(false);

            _startEnemyCount = _settings.StartEnemyCount;
            _maxEnemyCount = _settings.MaxEnemyCount;

            _enemySpawnRate = new(_settings.EnemySpawnRate);

            _enemySpawnMargin = _settings.EnemySpawnMargin;

            _respawnCheckInterval = _settings.RespawnCheckInterval;
            _respawnMargin = _settings.RespawnMargin;
        }


        void IEnemySpawnerModel.SetSpawning(bool spawning)
        {
            _isSpawning.Value = spawning; 
        }

        void IEnemySpawnerModel.Reset()
        {
            _isSpawning.Value = false;
            _enemySpawnRate.Value = _settings.EnemySpawnRate;
        }
    }
}
    