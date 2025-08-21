using _Project.Scripts.Data;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Other;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.PopUps
{
    public class PromoByuPopUp : PopUpBase, IDialog<PromoPopUpData>
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private Image _productImage;
        [SerializeField] private TextMeshProUGUI _productDescription;
        [SerializeField] private TextMeshProUGUI _prevPrice;
        [SerializeField] private TextMeshProUGUI _newPrice;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _closeButton;

        public void SetParams(PromoPopUpData message)
        {
            gameObject.SetActive(false);

            _title.text = message.ProductName;
            _productImage.sprite = message.ProductImage;
            _productDescription.text = message.ProductDescription;
            _prevPrice.text = message.ProductPrevPrice;
            _newPrice.text = message.ProductPrice;
        }

        public async UniTask<DialogResult> ShowDialogAsync(bool hideAfterChoice = false)
        {
            var tcs = new UniTaskCompletionSource<DialogResult>();

            _buyButton.OnClickAsObservable().Subscribe(_ =>
            {
                tcs.TrySetResult(DialogResult.Yes);
                HideAfterChoice(hideAfterChoice);
            }).AddTo(_disposables);

            _closeButton.OnClickAsObservable().Subscribe(_ =>
            {
                tcs.TrySetResult(DialogResult.Cancel);
                HideAfterChoice(hideAfterChoice);
            }).AddTo(_disposables);

            return await tcs.Task;
        }

        public override void Show()
        {
            Debug.LogWarning("Not implemented");
        }
    }
}
