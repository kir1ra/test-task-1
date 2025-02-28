using System;
using System.Collections.Generic;
using Test.Signals;
using Zenject;

namespace Test.Enemy
{
    public enum EnemyStates
    {
        Idle,
        Chase,
        Attack,
        None,
    }

    public class EnemyStateManager : ITickable, IInitializable, IDisposable
    {
        private List<IEnemyState> _states;
        private SignalBus _signalBus;
        private EnemyView _view;

        private IEnemyState _currentStateHandler;
        private EnemyStates _currentState = EnemyStates.None;


        public EnemyStates CurrentState
        {
            get { return _currentState; }
        }


        //We dont use constructor because circular dependencies
        [Inject]
        public void Construct(EnemyStateIdle idle,
                              EnemyStateAttack attack,
                              EnemyStateChase chase,
                              SignalBus signalBus,
                              EnemyView view)
        {
            _states = new List<IEnemyState>
            {
                // This needs to follow the enum order
                idle, chase, attack
            };

            _signalBus = signalBus;
            _view = view;
        }

        //Called on element created, not spawned.
        void IInitializable.Initialize()
        {
            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
            _signalBus.Subscribe<PlayerReviveSignal>(OnPlayerRevived);
        }

        private void OnPlayerDied()
        {
            ChangeState(EnemyStates.Idle);
        }
        private void OnPlayerRevived()
        {
            ChangeState(EnemyStates.Chase);
        }

        public void ChangeState(EnemyStates state)
        {
            if (_currentState == state)
                return;
            
            _currentState = state;

            if (_currentStateHandler != null)
            {
                _currentStateHandler.ExitState();
                _currentStateHandler = null;
            }

            _currentStateHandler = _states[(int)state];
            _currentStateHandler.EnterState();
        }

        void ITickable.Tick()
        {
            _currentStateHandler.ExecuteUpdate();
        }

        void IDisposable.Dispose()
        {
            _signalBus.TryUnsubscribe<PlayerDiedSignal>(OnPlayerDied);
            _signalBus.TryUnsubscribe<PlayerReviveSignal>(OnPlayerRevived);
        }

    }
}
