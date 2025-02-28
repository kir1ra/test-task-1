using System;
using Zenject;

namespace Test.Enemy
{
    public class EnemyStateIdle : IEnemyState
    {
        private readonly IEnemyState.Settings _settings;

        public EnemyStateIdle(IEnemyState.Settings settings)
        {
            _settings = settings;
        }

        void IEnemyState.EnterState()
        {
        }

        void IEnemyState.ExitState()
        {
        }

        void IEnemyState.ExecuteUpdate()
        {
        }

    }
}
