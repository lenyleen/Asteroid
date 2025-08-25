using _Project.Scripts.Data;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Other
{
    public class SaveSelectionToggle : MonoBehaviour
    {
        [field:SerializeField] public SaveSelectionDataType SelectedSaveType { get; private set; }

        [SerializeField] private TextMeshProUGUI _createdAt;
        [SerializeField] private TextMeshProUGUI _name;

        public void Initialize(string createdAt)
        {
            _name.text = SelectedSaveType.ToString();
            _createdAt.text = createdAt;
        }
    }
}
