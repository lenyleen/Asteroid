using System;
using Interfaces;
using UniRx;
using Zenject;

namespace UI.WeaponUi
{
    public class WeaponUiDisplayerViewModel : IInitializable, IDisposable
    {
        private readonly ReactiveCommand<WeaponUiDataDisplayer> _displayerAddedCommand = new();
        private readonly CompositeDisposable _disposables = new();
        private readonly ReactiveProperty<bool> _isEnabled = new(false);
        private readonly IPlayerWeaponInfoProviderService _playerWeaponInfoProviderService;
        private readonly IFactory<IWeaponInfoProvider, WeaponUiDataDisplayer> _displayersFactory;
        public IObservable<WeaponUiDataDisplayer> OnDisplayerAdded { get; }
        public IObservable<bool> IsEnabled => _isEnabled;

        public WeaponUiDisplayerViewModel(IPlayerWeaponInfoProviderService playerWeaponInfoProviderService,
            IFactory<IWeaponInfoProvider, WeaponUiDataDisplayer> displayersFactory)
        {
            _playerWeaponInfoProviderService = playerWeaponInfoProviderService;
            _displayersFactory = displayersFactory;

            OnDisplayerAdded = _displayerAddedCommand;
        }

        public void Dispose()
        {
            _disposables.Dispose();
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

        private void AddDisplayer(IWeaponInfoProvider provider)
        {
            if (!_isEnabled.Value)
            {
                _isEnabled.Value = true;
            }

            var displayerView = _displayersFactory.Create(provider);
            _displayerAddedCommand.Execute(displayerView);
        }

        private void OnProviderRemoved()
        {
            if (_playerWeaponInfoProviderService.WeaponInfoProviders.Count > 0)
            {
                return;
            }

            _isEnabled.Value = false;
        }
    }
}
