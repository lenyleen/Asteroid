using UI;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.States
{
    public class InputWaitState : IState, ITickable
    {
        private readonly TutorialViewModel _tutorialViewModel;
        private readonly PlayerInputController _playerInputController;
        private readonly GameplayStateMachine _stateMachine;

        public InputWaitState(PlayerInputController playerInputController, TutorialViewModel tutorialViewModel,
            GameplayStateMachine gameplayStateMachine)
        {
            _playerInputController = playerInputController;
            _tutorialViewModel = tutorialViewModel;
            _stateMachine = gameplayStateMachine;
        }

        public void Enter()
        {
            _tutorialViewModel.Enable(true);
        }

        public void Exit()
        {
            _tutorialViewModel.Enable(false);
        }

        public void Tick()
        {
            if (!CheckAnyButtonPressed())
            {
                return;
            }

            _stateMachine.ChangeState<PlayState>();
        }

        private bool CheckAnyButtonPressed()
        {
            var attackInput = _playerInputController.GetAttackInputData();

            return _playerInputController.GetInputValues() != Vector2.zero ||
                   attackInput.IsHeavyFirePressed || attackInput.IsMainFirePressed;
        }
    }
}
