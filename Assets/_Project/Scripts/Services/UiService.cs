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

        public async UniTask<TResult> ShowDialogAwaitable<TPopUp, TParam, TResult>(TParam param)
            where TPopUp : class, IDialogMenu<TParam, TResult>
        {
            var popUp = await _popUpFactory.CreatePopUp<TPopUp>();

            popUp.OnClose.Take(1)
                .Subscribe(Despawn);
            _spawnedPopUps.Add(popUp);

            return await popUp.ShowDialogAsync(param);
        }

        private void Despawn(IPopUp popUp)
        {
            if (_spawnedPopUps.Contains(popUp))
                _spawnedPopUps.Remove(popUp);
        }
    }
}
