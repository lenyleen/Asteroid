using _Project.Scripts.Interfaces;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class WeaponUiDataDisplayer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _ammo;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private Image _reloadTimeCircle;

        private readonly CompositeDisposable _disposables = new();

        public string Name => _name.text;

        public void Initialize(IWeaponInfoProvider infoProvider)
        {
            _name.text = infoProvider.Name;

            infoProvider.ReloadTimePercent.Subscribe(percent
                    => _reloadTimeCircle.fillAmount = percent)
                .AddTo(_disposables);

            infoProvider.AmmoCount.Subscribe(ammo
                    => _ammo.text = ammo.ToString())
                .AddTo(_disposables);

            infoProvider.OnDeath.Subscribe(_ =>
                    Hide())
                .AddTo(_disposables);

        }

        public void Hide()
        {
            _disposables.Dispose();
            Destroy(gameObject);
        }
    }
}
