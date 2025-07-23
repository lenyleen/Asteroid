using Interfaces;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI
{
    public class InGameUi : MonoBehaviour
    {
        [SerializeField] private RectTransform _weaponReloadsTransform;
        [SerializeField] private TextMeshProUGUI _position;
        [SerializeField] private TextMeshProUGUI _rotation;
        [SerializeField] private TextMeshProUGUI _velocity;
        [SerializeField] private TextMeshProUGUI _inputText;

        private readonly CompositeDisposable _disposables = new();

        private InGameUiViewModel _viewModel;

        private void OnDestroy()
        {
            _disposables.Dispose();
        }

        [Inject]
        public void Initialize(InGameUiViewModel inGameUiViewModel)
        {
            _viewModel = inGameUiViewModel;

            _viewModel._displayers.ObserveAdd()
                .Subscribe(displayer
                    => ApplyDisplayer(displayer.Value))
                .AddTo(_disposables);

            _viewModel.Position.Subscribe(position
                    => _position.text = position.ToString())
                .AddTo(_disposables);

            _viewModel.Velocity.Subscribe(velocity
                    => _velocity.text = velocity.ToString())
                .AddTo(_disposables);

            _viewModel.Rotation.Subscribe(rotation =>
                    _rotation.text = rotation.ToString())
                .AddTo(_disposables);

            _viewModel.IsStarted.Subscribe(started
                    => _inputText.enabled = !started)
                .AddTo(_disposables);
        }

        private void ApplyDisplayer(IWeaponUiDataDisplayer displayer)
        {
            if (displayer is not WeaponUiDataDisplayer dataDisplayer)
            {
                return;
            }

            dataDisplayer.transform.SetParent(_weaponReloadsTransform);
        }
    }
}
