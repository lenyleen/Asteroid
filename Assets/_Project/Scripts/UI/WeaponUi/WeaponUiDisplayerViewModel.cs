using System;
using _Project.Scripts.Factories;
using _Project.Scripts.Interfaces;
using UniRx;
using Zenject;

namespace _Project.Scripts.UI.WeaponUi
{
    public class WeaponUiDisplayerViewModel : IInitializable, IDisposable
    {
        private readonly ReactiveCommand<WeaponUiDataDisplayer> _displayerAddedCommand = new();
        private readonly CompositeDisposable _disposables = new();
        private readonly ReactiveProperty<bool> _isEnabled = new(false);
        private readonly IPlayerWeaponInfoProviderService _playerWeaponInfoProviderService;
        private readonly WeaponUiDataDisplayerFactory _displayersFactory;
        public IObservable<WeaponUiDataDisplayer> OnDisplayerAdded { get; }
        public IObservable<bool> IsEnabled => _isEnabled;

        public WeaponUiDisplayerViewModel(IPlayerWeaponInfoProviderService playerWeaponInfoProviderService,
            WeaponUiDataDisplayerFactory displayersFactory)
        {
            _playerWeaponInfoProviderService = playerWeaponInfoProviderService;
            _displayersFactory = displayersFactory;

            OnDisplayerAdded = _displayerAddedCommand;
        }

        public void Initialize()
        {
            _playerWeaponInfoProviderService.WeaponInfoProviders.ObserveAdd()
                .Subscribe(provider =>
                    AddDisplayer(provider.Value))
                .AddTo(_disposables);

            _playerWeaponInfoProviderService.WeaponInfoProviders.ObserveRemove()
                .Subscribe(_ => OnProviderRemoved())
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        private void AddDisplayer(IWeaponInfoProvider provider)
        {
            if (!_isEnabled.Value)
                _isEnabled.Value = true;

            var displayerView = _displayersFactory.Create(provider);
            _displayerAddedCommand.Execute(displayerView);
        }

        private void OnProviderRemoved()
        {
            if (_playerWeaponInfoProviderService.WeaponInfoProviders.Count > 0)
                return;

            _isEnabled.Value = false;
        }
    }
}
