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

        public void SetParams(PromoPopUpData data)
        {
            gameObject.SetActive(false);

            _title.text = data.ProductName;
            _productImage.sprite = data.ProductImage;
            _productDescription.text = data.ProductDescription;
            _prevPrice.text = data.ProductPrevPrice;
            _newPrice.text = data.ProductPrice;
        }

        public async UniTask<DialogResult> ShowDialogAsync(bool hideAfterChoice)
        {
            gameObject.SetActive(true);
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
    }
}
