using UniRx;

namespace Test
{
    public interface IGameModel
    {
        IReadOnlyReactiveProperty<int> EnemyKillCount { get; }
        IReadOnlyReactiveProperty<float> SurvivalTime { get; }

        IReadOnlyReactiveProperty<GameStates> State { get; }

        void IncrementEnemyKillCount();
        void IncreaseSurvivalTime(float deltaTime);
        void Reset();
        void SetState(GameStates playing);
    }
}