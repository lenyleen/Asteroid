using System;
using Cysharp.Threading.Tasks;
using Interfaces;
using Other;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PopUps
{
    public class ErrorPopUp : MonoBehaviour, IDialogMenu<string, DialogResult>
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _okButton;
        [SerializeField] private TextMeshProUGUI _message;

        public IObservable<IPopUp> OnClose => _closeCommand;

        private CompositeDisposable _disposables = new();
        private readonly ReactiveCommand<IPopUp> _closeCommand = new();

        public async UniTask<DialogResult> ShowDialogAsync(string message)
        {
            gameObject.SetActive(true);
            _message.text = message;

            var tcs = new UniTaskCompletionSource<DialogResult>();

            _closeButton.OnClickAsObservable().Subscribe(_ =>
            {
                tcs.TrySetResult(DialogResult.Cancel);
                Hide();
            }).AddTo(_disposables);

            _okButton.OnClickAsObservable().Subscribe(_ =>
            {
                tcs.TrySetResult(DialogResult.Cancel);
                Hide();
            }).AddTo(_disposables);

            return await tcs.Task;
        }

        public void Show() { }

        private void Hide()
        {
            gameObject.SetActive(false);
            _closeCommand.Execute(this);

            _disposables.Dispose();
            _disposables = new CompositeDisposable();
        }
    }
}
