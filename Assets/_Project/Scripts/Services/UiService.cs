using System.Collections.Generic;
using _Project.Scripts.Factories;
using _Project.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using UniRx;

namespace _Project.Scripts.Services
{
    public class UiService
    {
        private readonly PopUpFactory _popUpFactory;
        private readonly HashSet<IPopUp> _spawnedPopUps = new();

        public UiService(PopUpFactory popUpFactory)
        {
            _popUpFactory = popUpFactory;
        }

        public async void ShowPopUp<TPopUp>() where TPopUp : IUnParametrizedPopUp
        {
            var popUp = await SpawnPopUp<TPopUp>();

            popUp.Show();
        }

        public async UniTask<TPopUp> ShowDialogAwaitable<TPopUp, TParams>(TParams param)
            where TPopUp : class, IDialog<TParams> where TParams : IPopUpParams<TPopUp>
        {
            var popUp = await SpawnPopUp<TPopUp>();

            popUp.SetParams(param);

            return popUp;
        }

        private async UniTask<TPopUp> SpawnPopUp<TPopUp>() where TPopUp : IPopUp
        {
            var popUp = await _popUpFactory.CreatePopUp<TPopUp>();

            popUp.OnClose.Take(1)
                .Subscribe(Despawn);

            _spawnedPopUps.Add(popUp);

            return popUp;
        }

        private void Despawn(IPopUp popUp)
        {
            if (_spawnedPopUps.Contains(popUp))
                _spawnedPopUps.Remove(popUp);
        }
    }
}
