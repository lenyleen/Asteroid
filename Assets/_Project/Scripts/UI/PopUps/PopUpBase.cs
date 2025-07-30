using System;
using Interfaces;
using UniRx;
using UnityEngine;

namespace UI.PopUps
{
    public abstract class PopUpBase : MonoBehaviour, IPopUp
    {
        public IObservable<IPopUp> OnClose => _closeCommand;

        private readonly ReactiveCommand<IPopUp> _closeCommand = new();

        protected readonly CompositeDisposable _disposables = new();

        public void Initialize(Transform parent)
        {
            transform.SetParent(parent,false);
            gameObject.SetActive(true);
        }

        public abstract void Show();

        protected void Hide()
        {
            gameObject.SetActive(false);
            _closeCommand.Execute(this);

            Destroy(gameObject);
            _disposables.Dispose();
        }
    }
}
