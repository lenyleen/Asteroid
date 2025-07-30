using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.States;
using UniRx;

namespace _Project.Scripts
{
    public class GameplayStateMachine : IDisposable
    {
        private readonly Dictionary<Type, IState> _states;
        private readonly CompositeDisposable _disposables = new ();

        private IState _currentState;

        public GameplayStateMachine(List<IState> states)
        {
            _states = states.ToDictionary(st =>
                st.GetType(), st => st);
        }

        public void Initialize(Type initialStateType) =>
            ChangeState(initialStateType);

        public void Dispose() =>
            _disposables.Dispose();

        private void ChangeState(Type type)
        {
            if (!_states.TryGetValue(type, out var newState))
                throw new Exception($"State {type.Name} not found in the state machine.");

            _currentState?.Exit();

            _currentState = newState;

            _currentState.OnStateChanged
                .Take(1)
                .Subscribe(ChangeState)
                .AddTo(_disposables);

            _currentState.Enter();
        }
    }
}
