using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class PromoButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _promoName;

        public IObservable<Unit> OnSelected { get; private set; }

        public void Initialize(Sprite sprite, string productName)
        {
            _image.sprite = sprite;
            _promoName.text = productName;
            OnSelected = _button.OnClickAsObservable();
        }

        public void Hide()
        {
            Destroy(this.gameObject);
        }
    }
}
