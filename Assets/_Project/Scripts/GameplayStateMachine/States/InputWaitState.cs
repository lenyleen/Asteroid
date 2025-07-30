using System;
using UI;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.States
{
    public class InputWaitState : IState, ITickable
    {
        public IObservable<Type> OnStateChanged => _changeStateCommand;

        private readonly TutorialViewModel _tutorialViewModel;
        private readonly PlayerInputController _playerInputController;
        private readonly ReactiveCommand<Type> _changeStateCommand = new();

        public InputWaitState(PlayerInputController playerInputController, TutorialViewModel tutorialViewModel)
        {
            _playerInputController = playerInputController;
            _tutorialViewModel = tutorialViewModel;
        }

        public void Enter() => _tutorialViewModel.Enable(true);

        public void Exit() => _tutorialViewModel.Enable(false);

        public void Tick()
        {
            if (!CheckAnyButtonPressed())
                return;

            _changeStateCommand.Execute(typeof(PlayState));
        }

        private bool CheckAnyButtonPressed()
        {
            var attackInput = _playerInputController.GetAttackInputData();

            return _playerInputController.GetInputValues() != Vector2.zero ||
                   attackInput.IsHeavyFirePressed || attackInput.IsMainFirePressed;
        }
    }
}
