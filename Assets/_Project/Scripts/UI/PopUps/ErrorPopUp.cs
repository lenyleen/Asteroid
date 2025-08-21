using _Project.Scripts.Interfaces;
using _Project.Scripts.Other;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.PopUps
{
    public class ErrorPopUp : PopUpBase, IDialog<string, DialogResult>
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _okButton;
        [SerializeField] private TextMeshProUGUI _message;

        public void SetParams(string message)
        {
            gameObject.SetActive(false);
            _message.text = message;
        }

        public async UniTask<DialogResult> ShowDialogAsync(bool hideAfterChoice = true)
        {
            gameObject.SetActive(true);

            var tcs = new UniTaskCompletionSource<DialogResult>();

            _closeButton.OnClickAsObservable().Subscribe(_ =>
            {
                tcs.TrySetResult(DialogResult.Cancel);
                HideAfterChoice(hideAfterChoice);
            }).AddTo(_disposables);

            _okButton.OnClickAsObservable().Subscribe(_ =>
            {
                tcs.TrySetResult(DialogResult.Cancel);
                HideAfterChoice(hideAfterChoice);
            }).AddTo(_disposables);

            return await tcs.Task;
        }

        public override void Show() =>
            Debug.LogWarning("Not implemented");
    }
}
