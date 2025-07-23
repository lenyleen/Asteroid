using System;
using System.Linq;
using Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI
{
    public class InGameUiViewModel : IInitializable, IDisposable
    {
        private readonly IFactory<IWeaponInfoProvider, IWeaponUiDataDisplayer> _displayersFactory;
        private readonly CompositeDisposable _disposables = new();
        private readonly IGameEvents _gameEvents;
        private readonly IPlayerPositionProvider _playerPositionProviderService;

        private readonly IPlayerWeaponInfoProviderService _playerWeaponInfoProviderService;

        public InGameUiViewModel(IPlayerWeaponInfoProviderService playerWeaponInfoProviderService,
            IFactory<IWeaponInfoProvider, IWeaponUiDataDisplayer> displayersFactory,
            IPlayerPositionProvider playerPositionProviderService, IGameEvents gameEvents)
        {
            Position = new ReactiveProperty<Vector3>(Vector3.zero);
            Rotation = new ReactiveProperty<float>(0);
            Velocity = new ReactiveProperty<Vector2>(Vector2.zero);

            _playerWeaponInfoProviderService = playerWeaponInfoProviderService;
            _playerPositionProviderService = playerPositionProviderService;
            _gameEvents = gameEvents;
            _displayersFactory = displayersFactory;
        }

        public ReactiveCollection<IWeaponUiDataDisplayer> _displayers { get; } = new();
        public ReactiveProperty<Vector3> Position { get; }
        public ReactiveProperty<float> Rotation { get; }
        public ReactiveProperty<Vector2> Velocity { get; }
        public ReactiveProperty<bool> IsStarted { get; } = new(false);

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public void Initialize()
        {
            _playerWeaponInfoProviderService.WeaponInfoProviders
                .ObserveAdd()
                .Subscribe(provider => AddWeaponDisplayer(provider.Value))
                .AddTo(_disposables);

            _playerWeaponInfoProviderService.WeaponInfoProviders
                .ObserveRemove()
                .Subscribe(provider => RemoveWeaponDisplayer(provider.Value))
                .AddTo(_disposables);

            _playerPositionProviderService.PositionProvider
                .Subscribe(OnPositionProviderChanged)
                .AddTo(_disposables);

            _gameEvents.OnGameStarted.Subscribe(_ =>
                    IsStarted.Value = true)
                .AddTo(_disposables);
        }

        private void AddWeaponDisplayer(IWeaponInfoProvider provider)
        {
            var displayer = _displayersFactory.Create(provider);
            _displayers.Add(displayer);
        }

        private void RemoveWeaponDisplayer(IWeaponInfoProvider provider)
        {
            var displayer = _displayers.FirstOrDefault(dspl => dspl.Name == provider.Name);
            if (displayer == null)
            {
                return;
            }

            displayer.Hide();
            _displayers.Remove(displayer);
        }

        private void OnPositionProviderChanged(IPositionProvider positionProvider)
        {
            if (positionProvider == null)
            {
                return;
            }

            positionProvider.Position.Subscribe(pos
                    => Position.Value = pos)
                .AddTo(_disposables);

            positionProvider.Velocity.Subscribe(vel
                    => Velocity.Value = vel)
                .AddTo(_disposables);

            positionProvider.Rotation
                .Subscribe(rot => Rotation.Value = rot)
                .AddTo(_disposables);
        }
    }
}
