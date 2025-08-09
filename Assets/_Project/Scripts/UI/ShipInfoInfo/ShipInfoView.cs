using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.UI.ShipInfoInfo
{
    public class ShipInfoView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _position;
        [SerializeField] private TextMeshProUGUI _rotation;
        [SerializeField] private TextMeshProUGUI _velocity;

        private readonly CompositeDisposable _disposables = new();

        [Inject]
        private void Construct(ShipInfoViewModel viewModel)
        {
            viewModel.IsEnabled.Subscribe(enabled =>
                    gameObject.SetActive(enabled))
                .AddTo(_disposables);

            viewModel.Position.Subscribe(pos =>
                    _position.text = pos.ToString())
                .AddTo(_disposables);

            viewModel.Rotation.Subscribe(rot =>
                    _rotation.text = rot.ToString())
                .AddTo(_disposables);

            viewModel.Velocity.Subscribe(vel =>
                    _velocity.text = vel.ToString())
                .AddTo(_disposables);
        }
    }
}
