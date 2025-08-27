using System;
using _Project.Scripts.Interfaces;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.PopUps
{
    public abstract class PopUpBase : MonoBehaviour, IPopUp
    {
        [SerializeField] protected Button _closeButton;
        public IObservable<IPopUp> OnClose => _closeCommand;

        private readonly ReactiveCommand<IPopUp> _closeCommand = new();

        protected readonly CompositeDisposable _disposables = new();

        public void Initialize(Transform parent)
        {
            transform.SetParent(parent,false);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _closeCommand.Execute(this);

            Destroy(gameObject);
        }

        protected void HideAfterChoice(bool hideAfterChoice)
        {
            if(!hideAfterChoice)
                return;

            Hide();
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}
