using _Project.Scripts.Data;
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
    public class LosePopUp : PopUpBase, IDialog<LosePopUpData>
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Button _restartButton;

        [SerializeField] private string _message;

        [Inject]
        private void Construct(IAdvertisementService advertisementService)
        {
            advertisementService.CanShowRewardedAds.Subscribe(canShowRewarded =>
                _restartButton.interactable = canShowRewarded).AddTo(this);
        }

        public void SetParams(LosePopUpData data)
        {
            gameObject.SetActive(false);
            _scoreText.text = _message + data.Score;
        }

        public async UniTask<DialogResult> ShowDialogAsync(bool hideAfterChoice)
        {
            gameObject.SetActive(true);

            var tcs = new UniTaskCompletionSource<DialogResult>();

            _restartButton.OnClickAsObservable().Subscribe(_ =>
            {
                tcs.TrySetResult(DialogResult.Yes);
                HideAfterChoice(hideAfterChoice);
            }).AddTo(this);

            _closeButton.OnClickAsObservable().Subscribe(_ =>
            {
                tcs.TrySetResult(DialogResult.Cancel);
                HideAfterChoice(hideAfterChoice);
            }).AddTo(this);

            return await tcs.Task;
        }
    }
}
