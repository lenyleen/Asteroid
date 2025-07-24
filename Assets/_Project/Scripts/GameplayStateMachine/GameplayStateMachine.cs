using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.States;

namespace _Project.Scripts
{
    public class GameplayStateMachine
    {
        private readonly Dictionary<Type, IState> _states;

        private IState _currentState;

        public GameplayStateMachine(List<IState> states)
        {
            _states = states.ToDictionary(st =>
                st.GetType(), st => st);
        }

        public void ChangeState<T>() where T : IState
        {
            var newState = _states[typeof(T)];

            _currentState?.Exit();

            _currentState = newState;
            _currentState.Enter();
        }
    }
}
