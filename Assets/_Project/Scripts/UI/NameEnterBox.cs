using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class NameEnterBox : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _enteredText;
        [SerializeField] private Button _restartButton;

        public ReactiveCommand<string> _restartCommand { get; } = new (); 
        
        private void Awake()
        {
            _restartButton.onClick.AddListener(RestartButtonOnClick);
        }

        public void Display(bool enable)
        {
            gameObject.SetActive(enable);
        }
        public void RestartButtonOnClick()
        {
            _restartCommand.Execute(_enteredText.text);
        }
    }
}