using System;
using Cysharp.Threading.Tasks;
using Interfaces;
using Other;
using TMPro;
using UI.PopUps;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class LosePopUp : PopUpBase, IDialogMenu<int, DialogResult>
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private string _message;

        public async UniTask<DialogResult> ShowDialogAsync(int score)
        {
            gameObject.SetActive(true);
            _scoreText.text = _message + score;

            var tcs = new UniTaskCompletionSource<DialogResult>();

            _restartButton.OnClickAsObservable().Subscribe(_ =>
            {
                tcs.TrySetResult(DialogResult.Yes);
                Hide();
            }).AddTo(_disposables);

            _closeButton.OnClickAsObservable().Subscribe(_ =>
            {
                tcs.TrySetResult(DialogResult.Cancel);
                Hide();
            }).AddTo(_disposables);

            return await tcs.Task;
        }

        public override void Show()
        {
        }
    }
}
