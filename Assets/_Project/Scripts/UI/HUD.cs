using _Project.Scripts.UI.ScoreBox;
using _Project.Scripts.UI.ShipInfoInfo;
using _Project.Scripts.UI.Tutorial;
using _Project.Scripts.UI.WeaponUi;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class HUD : MonoBehaviour
    {
        [field: SerializeField] public ShipInfoView ShipInfoView { get; private set; }
        [field: SerializeField] public WeaponUiDisplayerView WeaponUiDisplayerView { get; private set; }
        [field: SerializeField] public ScoreBoxView ScoreBoxView { get; private set; }
        [field: SerializeField] public TutorialView TutorialView { get; private set; }
    }
}
