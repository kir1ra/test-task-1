using UniRx;

namespace Test
{
    public enum GameStates
    {
        Idle,
        Playing,
        GameOver
    }

    public class GameModel : IGameModel
    {
        private readonly ReactiveProperty<GameStates> _state;

        private readonly ReactiveProperty<float> _survivalTime;
        private readonly ReactiveProperty<int> _enemyKillCount;

        IReadOnlyReactiveProperty<int> IGameModel.EnemyKillCount => _enemyKillCount;
        IReadOnlyReactiveProperty<float> IGameModel.SurvivalTime => _survivalTime;

        IReadOnlyReactiveProperty<GameStates> IGameModel.State => _state;
        public GameModel()
        {
            _state = new(GameStates.Idle);
            _survivalTime = new(0);
            _enemyKillCount = new(0);
        }

        void IGameModel.SetState(GameStates state) => _state.Value = state;

        void IGameModel.IncreaseSurvivalTime(float deltaTime)
        {
            _survivalTime.Value += deltaTime;
        }
        void IGameModel.IncrementEnemyKillCount()
        {
            _enemyKillCount.Value++;
        }

        void IGameModel.Reset()
        {
            _survivalTime.Value = 0;
            _enemyKillCount.Value = 0;
        }
    }
}