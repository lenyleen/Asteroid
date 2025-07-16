using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WeaponReload
    {
        [SerializeField] private TextMeshProUGUI _weaponName;
        [SerializeField] private Image _status;

        public void Initialize(string weaponName)
        {
            _weaponName.text = weaponName;
        }
        
        public void ChangeStatus(float percentage)
        {
            _status.fillAmount = percentage;
        }
    }
}