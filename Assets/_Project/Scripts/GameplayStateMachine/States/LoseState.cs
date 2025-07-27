using System;
using _Project.Scripts.DTO;
using Interfaces;
using ModestTree;
using Other;
using Services;
using UI;
using UniRx;
using Zenject;

namespace _Project.Scripts.States
{
    public class LoseState : IState
    {
        private readonly UiService _uiService;
        private readonly GameplayStateMachine _stateMachine;

        private CompositeDisposable _disposables = new();

        public LoseState(UiService uiService, GameplayStateMachine stateMachine)
        {
            _uiService = uiService;
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            var popUp = _uiService.ShowDialogMenu<LosePopUp, int>(10);

            popUp.OnComplete.Subscribe(result =>
                    Restart(result, popUp))
                .AddTo(_disposables);
        }

        public void Exit()
        {
            _disposables.Dispose();
            _disposables =  new CompositeDisposable();
        }

        private void Restart(bool result, IPopUp popUp)
        {
            if(!result)
                return;

            popUp.Hide();
            _stateMachine.ChangeState<PlayState>();
        }
    }
}
