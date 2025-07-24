using TMPro;
using UnityEngine;

namespace UI.PlayerInfo
{
    public class ShipInfoView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _position;
        [SerializeField] private TextMeshProUGUI _rotation;
        [SerializeField] private TextMeshProUGUI _velocity;
    }
}
