using System;
using Cysharp.Threading.Tasks;
using Interfaces;
using Other;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LosePopUp : MonoBehaviour, IDialogMenu<int, DialogResult>
    {
        [SerializeField] private TextMeshProUGUI _enteredText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;

        private readonly ReactiveCommand<IPopUp> _closeCommand = new();

        private CompositeDisposable _closeDisposable = new();

        public IObservable<IPopUp> OnClose => _closeCommand;

        public async UniTask<DialogResult> ShowDialogAsync(int score)
        {
            gameObject.SetActive(true);
            _enteredText.text = score.ToString();

            var tcs = new UniTaskCompletionSource<DialogResult>();

            _restartButton.OnClickAsObservable().Subscribe(_ =>
            {
                tcs.TrySetResult(DialogResult.Yes);
                Hide();
            }).AddTo(_closeDisposable);

            _exitButton.OnClickAsObservable().Subscribe(_ =>
            {
                tcs.TrySetResult(DialogResult.Cancel);
                Hide();
            }).AddTo(_closeDisposable);

            return await tcs.Task;
        }

        private void Hide()
        {
            gameObject.SetActive(false);
            _closeCommand.Execute(this);

            _closeDisposable.Dispose();
            _closeDisposable = new CompositeDisposable();
        }

        public void Show()
        {
        }
    }
}
