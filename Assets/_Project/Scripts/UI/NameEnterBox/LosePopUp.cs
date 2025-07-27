using System;
using Interfaces;
using Other;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class LosePopUp : MonoBehaviour, IDialogMenu<int>
    {
        [SerializeField] private TextMeshProUGUI _enteredText;
        [SerializeField] private Button _restartButton;

        public IObservable<IPopUp> OnClose => _closeCommand;
        public IObservable<bool> OnComplete => _confirmCommand;

        private readonly CompositeDisposable _closeDisposable = new ();

        private ReactiveCommand<IPopUp> _closeCommand;
        private ReactiveCommand<bool>  _confirmCommand;

        public void Show(int score)
        {
            gameObject.SetActive(true);
            _enteredText.text = score.ToString();

            _restartButton.OnClickAsObservable().Subscribe(_ =>
                    _confirmCommand.Execute(true))
                .AddTo(_closeDisposable);
        }

        public void Show()
        {}

        public void Hide()
        {
            gameObject.SetActive(false);
            _closeCommand.Execute(this);
        }
    }
}
