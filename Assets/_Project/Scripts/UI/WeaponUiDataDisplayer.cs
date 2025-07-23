using Interfaces;
using TMPro;
using UniRx;
using UnityEngine;

namespace UI
{
    public class WeaponUiDataDisplayer : MonoBehaviour, IWeaponUiDataDisplayer
    {
        [SerializeField] private TextMeshProUGUI _ammo;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _realodTime;

        private readonly CompositeDisposable _disposables = new();

        public string Name => _name.text;

        public void Initialize(IWeaponInfoProvider infoProvider)
        {
            _name.text = infoProvider.Name;

            infoProvider.ReloadTime.Subscribe(time
                    => _realodTime.text = $"{time} sec.")
                .AddTo(_disposables);

            infoProvider.AmmoCount.Subscribe(ammo
                    => _ammo.text = ammo.ToString())
                .AddTo(_disposables);
        }

        public void Hide()
        {
            _disposables.Dispose();
            Destroy(gameObject);
        }
    }
}
