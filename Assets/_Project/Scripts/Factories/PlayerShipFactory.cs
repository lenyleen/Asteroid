using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Input;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Player;
using _Project.Scripts.Services;
using _Project.Scripts.Weapon;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Factories
{
    public class PlayerShipFactory : IAsyncInitializable
    {
        private const string ShipViewConfigAddress = "CommonShipConfig"; //плейсхолдер до задания по скинам для корабля

        private readonly IInstantiator _instantiator;
        private readonly IPlayerStateProviderService _playerDataProviderService;
        private readonly PlayerInputController _playerInputController;
        private readonly ShipPreferences _shipPreferences;
        private readonly IPlayerWeaponInfoProviderService _playerWeaponInfoProviderService;
        private readonly WeaponFactory _weaponFactory;
        private readonly AssetProvider _assetProvider;

        private ShipViewConfig _shipViewConfig;
        private Ship _shipPrefab;
        private Sprite _shipSprite;
        private ShipViewModel _shipViewModel;
        private WeaponConfig _mainWeaponConfig;
        private WeaponConfig _heavyWeaponConfig;

        public PlayerShipFactory(WeaponFactory weaponFactory, ShipPreferences shipPreferences,
            PlayerInputController playerInputController, IInstantiator instantiator,
            IPlayerStateProviderService playerDataProviderService, AssetProvider assetProvider,
            IPlayerWeaponInfoProviderService playerWeaponInfoProviderService)
        {
            _weaponFactory = weaponFactory;
            _playerInputController = playerInputController;
            _playerDataProviderService = playerDataProviderService;
            _playerWeaponInfoProviderService = playerWeaponInfoProviderService;
            _shipPreferences = shipPreferences;
            _assetProvider = assetProvider;
            _instantiator = instantiator;
        }

        public async UniTask InitializeAsync()
        {
            _shipViewConfig = await _assetProvider.Load<ShipViewConfig>(ShipViewConfigAddress);
            _shipPrefab = (await _assetProvider.Load<GameObject>(_shipPreferences.PlayerShipPrefabReference))
                .GetComponent<Ship>();

            _shipSprite = await _assetProvider.Load<Sprite>(_shipViewConfig.ShipSprite);

            _mainWeaponConfig = await _assetProvider.Load<WeaponConfig>(_shipViewConfig.MainWeaponConfig);
            _heavyWeaponConfig = await _assetProvider.Load<WeaponConfig>(_shipViewConfig.HeavyWeaponConfig);
        }

        public async UniTask SpawnShip()
        {
            var shipModel = _instantiator.Instantiate<ShipModel>(new object[] { _shipPreferences });

            _shipViewModel = _instantiator.Instantiate<ShipViewModel>(new object[] { shipModel });

            _playerDataProviderService.ApplyPlayer(_shipViewModel);

            var shipView = _instantiator.InstantiatePrefabForComponent<Ship>(
                _shipPrefab,
                _shipPreferences.StartPosition,
                Quaternion.identity,
                null
            );

            await SpawnPlayerWeapons(shipView.PlayerWeapons);

            shipView.Initialize(_shipViewModel, _shipSprite);
        }

        private async UniTask SpawnPlayerWeapons(PlayerWeapons playerWeapons)
        {
            var heavyWeapons =
                await CreateWeapons(_heavyWeaponConfig, _shipViewConfig.HeavyWeaponSlots, playerWeapons);

            var mainWeapons =
                await CreateWeapons(_mainWeaponConfig, _shipViewConfig.LightWeaponSlots, playerWeapons);

            var weaponsViewModel = _instantiator.Instantiate<PlayerWeaponsViewModel>(new object[]
            {
                mainWeapons, heavyWeapons, _playerInputController,
                _playerDataProviderService.PositionProvider.Value
            });

            weaponsViewModel.Initialize();
            playerWeapons.Initialize(weaponsViewModel);
        }

        private async UniTask<List<WeaponViewModel>> CreateWeapons(WeaponConfig config, List<Vector3> positions,
            PlayerWeapons playerWeapons)
        {
            var weapons = new List<WeaponViewModel>();
            var weaponIndex = 1;

            foreach (var position in positions)
            {
                var name = $"{config.Type} {weaponIndex}";
                var newWeapon = await _weaponFactory.Create(config, name,
                    playerWeapons, position);

                weapons.Add(newWeapon);
                _playerWeaponInfoProviderService.ApplyWeaponInfoProvider(newWeapon);

                weaponIndex++;
            }

            return weapons;
        }
    }
}
