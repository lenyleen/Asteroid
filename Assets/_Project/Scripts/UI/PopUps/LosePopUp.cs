using _Project.Scripts.Configs;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Other;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.UI.PopUps
{
    public class LosePopUp : PopUpBase, IDialogMenu<int, DialogResult>
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private string _message;

        [Inject]
        private void Construct(IAdvertisementService advertisementService)
        {
            advertisementService.CanShowInterstitialAds.Subscribe(canShowRewarded =>
                _restartButton.interactable = canShowRewarded).AddTo(_disposables);
        }

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
