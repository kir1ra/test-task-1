using Test.Player;
using UnityEngine;

namespace Test.Enemy
{
    public class EnemyStateAttack : IEnemyState
    {
        readonly IEnemyState.Settings _settings;
        readonly EnemyStateManager _stateManager;
        readonly EnemyView _view;
        readonly PlayerPresenter _player;

        float _lastAttackTime;

        public EnemyStateAttack(
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

        public void EnterState()
        {
        }

        public void ExitState()
        {
        }

        public void ExecuteUpdate()
        {
            _view.Rotate((_player.Position - _view.Position).normalized);

            if (Time.realtimeSinceStartup - _lastAttackTime > 1f / _settings.AttackSpeed)
            {
                _lastAttackTime = Time.realtimeSinceStartup;
                Fire();
            }

            // If the player is to far away then chase it
            if ((_player.Position - _view.Position).magnitude > _settings.AttackRange)
            {
                _stateManager.ChangeState(EnemyStates.Chase);
            }
        }

        void Fire()
        {
            _player.TakeDamage(_settings.AttackDamage);
        }
    }
}
