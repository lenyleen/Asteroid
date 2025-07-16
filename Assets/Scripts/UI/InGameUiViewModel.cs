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
        private readonly IPlayerWeaponInfoProviderService _playerWeaponInfoProviderService;
        private readonly IPlayerPositionProvider _playerPositionProviderService;
        private readonly CompositeDisposable _disposables = new ();
        private readonly IFactory<IWeaponInfoProvider, IWeaponUiDataDisplayer> _displayersFactory;
        
        public ReactiveCollection<IWeaponUiDataDisplayer> _displayers{get;} = new();
        public ReactiveProperty<Vector3> Position { get; private set; }
        public ReactiveProperty<float> Rotation { get; private set; }
        public ReactiveProperty<Vector2> Velocity { get; private set; }
        public ReactiveProperty<bool> IsStarted { get; private set; } =  new (false); 

        public InGameUiViewModel(IPlayerWeaponInfoProviderService playerWeaponInfoProviderService,
            IFactory<IWeaponInfoProvider,IWeaponUiDataDisplayer> displayersFactory,
            IPlayerPositionProvider playerPositionProviderService)
        {
            Position = new ReactiveProperty<Vector3>(Vector3.zero);
            Rotation = new ReactiveProperty<float>(0f);
            Velocity = new ReactiveProperty<Vector2>(Vector2.zero);
            
            _playerWeaponInfoProviderService = playerWeaponInfoProviderService;
            _playerPositionProviderService = playerPositionProviderService;
            _displayersFactory = displayersFactory;
        }

        public void Initialize()
        {
            _playerWeaponInfoProviderService.WeaponInfoProviders
                .ObserveAdd()
                .Subscribe(provider =>AddWeaponDisplayer(provider.Value))
                .AddTo(_disposables);
            
            _playerWeaponInfoProviderService.WeaponInfoProviders
                .ObserveRemove()
                .Subscribe(provider => RemoveWeaponDisplayer(provider.Value))
                .AddTo(_disposables);
            
            _playerPositionProviderService.PositionProvider
                .Subscribe(OnPositionProviderChanged)
                .AddTo(_disposables);
        }

        public void Start()
        {
            IsStarted.Value = true;    
        }
        
        private void AddWeaponDisplayer(IWeaponInfoProvider  provider)
        {
            var displayer = _displayersFactory.Create(provider);
            _displayers.Add(displayer);
        }

        public void RemoveWeaponDisplayer(IWeaponInfoProvider provider)
        {
            var displayer = _displayers.FirstOrDefault(dspl => dspl.Name == provider.Name);
            if(displayer == null)
                return;
            
            displayer.Hide();
            _displayers.Remove(displayer);
        }

        private void OnPositionProviderChanged(IPositionProvider positionProvider)
        {
            if (positionProvider == null)
            {
                Position = new ReactiveProperty<Vector3>(Vector3.zero);
                Rotation = new ReactiveProperty<float>(0f);
                Velocity = new ReactiveProperty<Vector2>(Vector2.zero);
                return;
            }
            Position = positionProvider.Position;
            Velocity = positionProvider.Velocity;
            Rotation = positionProvider.Rotation;
        }
        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}