using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Factories;
using Interfaces;
using UniRx;

namespace Services
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
            var popUp = _popUpFactory.CreatePopUp<TPopUp>();

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
