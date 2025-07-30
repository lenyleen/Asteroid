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
    public class ErrorPopUp : PopUpBase, IDialogMenu<string, DialogResult>
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _okButton;
        [SerializeField] private TextMeshProUGUI _message;

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

        public override void Show() { }
    }
}
