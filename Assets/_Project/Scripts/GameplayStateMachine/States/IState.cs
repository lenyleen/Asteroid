using System;

namespace _Project.Scripts.GameplayStateMachine.States
{
    public interface IState
    {
        public IObservable<Type> OnStateChanged { get; }
        public void Enter();
        public void Exit();
    }
}
