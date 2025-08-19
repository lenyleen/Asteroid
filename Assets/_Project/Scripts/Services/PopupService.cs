using System.Collections.Generic;
using _Project.Scripts.Factories;
using _Project.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using UniRx;

namespace _Project.Scripts.Services
{
    public class PopupService
    {
        private readonly PopUpFactory _popUpFactory;
        private readonly PopUpVmFactory _popUpVmFactory;
        private readonly HashSet<IPopUp> _spawnedPopUps = new();

        public PopupService(PopUpFactory popUpFactory, PopUpVmFactory  popUpVmFactory)
        {
            _popUpVmFactory  = popUpVmFactory;
            _popUpFactory = popUpFactory;
        }

        public async UniTask<TVm> ShowVmPopup<TPopUp, TParams, TVm>(TParams param) where TPopUp : class, IPopUp<TVm> where TVm : IPopUpViewModel
        {
            var popUp = await _popUpFactory.CreatePopUp<TPopUp>();

            var vm = _popUpVmFactory.Create<TVm, TParams>(param);

            AddPopUp(popUp);

            popUp.Show(vm);

            return vm;
        }

        public async UniTask<TResult> ShowDialogAwaitable<TPopUp, TParam, TResult>(TParam param)
            where TPopUp : class, IDialogMenu<TParam, TResult>
        {
            var popUp = await _popUpFactory.CreatePopUp<TPopUp>();

            AddPopUp(popUp);

            return await popUp.ShowDialogAsync(param);
        }

        private void AddPopUp(IPopUp popUp)
        {
            popUp.OnClose.Take(1)
                .Subscribe(Despawn);
            _spawnedPopUps.Add(popUp);
        }

        private void Despawn(IPopUp popUp)
        {
            if (_spawnedPopUps.Contains(popUp))
                _spawnedPopUps.Remove(popUp);
        }
    }
}
