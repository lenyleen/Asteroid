using System;
using _Project.Scripts.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.UI.ShipInfoInfo
{
    public class ShipInfoViewModel : IInitializable, IDisposable
    {
        public IObservable<Vector3> Position => _position;
        public IObservable<float> Rotation => _rotation;
        public IObservable<Vector2> Velocity => _velocity;
        public IObservable<bool> IsEnabled => _isEnabled;

        private readonly ReactiveProperty<Vector3> _position = new ();
        private readonly ReactiveProperty<float> _rotation = new ();
        private readonly ReactiveProperty<Vector2> _velocity = new ();
        private readonly ReactiveProperty<bool> _isEnabled = new (false);
        private readonly IPlayerStateProviderService _playerStateProviderServiceService;

        private readonly CompositeDisposable _disposables = new();

        public ShipInfoViewModel(IPlayerStateProviderService playerStateProviderService)
        {
            _playerStateProviderServiceService = playerStateProviderService;
        }

        public void Initialize()
        {
            _playerStateProviderServiceService.PositionProvider
                .Subscribe(OnPositionProviderChanged)
                .AddTo(_disposables);
        }

        public void Dispose() =>
            _disposables.Dispose();

        private void OnPositionProviderChanged(IPositionProvider positionProvider)
        {
            _isEnabled.Value = positionProvider != null;

            if(!_isEnabled.Value)
                return;

            positionProvider.Position.Subscribe(pos
                    => _position.Value = pos)
                .AddTo(_disposables);

            positionProvider.Velocity.Subscribe(vel
                    => _velocity.Value = vel)
                .AddTo(_disposables);

            positionProvider.Rotation
                .Subscribe(rot => _rotation.Value = rot)
                .AddTo(_disposables);
        }
    }
}
