using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class PlayerUiView : MonoBehaviour
    {
        [SerializeField] NameEnterBox _nameEnterBox;
        [SerializeField] private TextMeshProUGUI _scoreText;
        
        private readonly CompositeDisposable _disposable = new ();
        
        private PlayerViewModel _playerViewModel;
        
        [Inject]
        public void Initialize(PlayerViewModel player)
        {
            _playerViewModel = player;

            _nameEnterBox._restartCommand.Subscribe(name => _playerViewModel.OnRestartClick(name))
                .AddTo(_disposable);
            
            _playerViewModel.Score.Subscribe(score => _scoreText.text = score.ToString())
                .AddTo(_disposable);
            
            _playerViewModel.OnEndScreenEnable.Subscribe(enable => _nameEnterBox.Display(enable))
                .AddTo(_disposable);
        }
    }
}