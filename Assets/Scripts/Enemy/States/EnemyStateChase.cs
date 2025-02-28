using Test.Player;

namespace Test.Enemy
{
    public class EnemyStateChase : IEnemyState
    {
        readonly IEnemyState.Settings _settings;
        readonly EnemyStateManager _stateManager;
        readonly EnemyView _view;
        readonly PlayerPresenter _player;

        public EnemyStateChase(
            PlayerPresenter player,
            EnemyView view,
            EnemyStateManager stateManager,
            IEnemyState.Settings settings)
        {
            _settings = settings;
            _stateManager = stateManager;
            _view = view;
            _player = player;
        }

        void IEnemyState.EnterState()
        {
        }

        void IEnemyState.ExitState()
        {
        }

        void IEnemyState.ExecuteUpdate()
        {
            MoveTowardsPlayer();

            var distanceToPlayer = (_player.Position - _view.Position).magnitude;

            if (distanceToPlayer <= _settings.AttackRange)
            {
                _stateManager.ChangeState(EnemyStates.Attack);
            }
        }

        void MoveTowardsPlayer()
        {
            var dir = (_player.Position - _view.Position).normalized;

            //Align view towards the player
            _view.Rotate(dir);
            _view.Move(dir);
        }
    }
}

