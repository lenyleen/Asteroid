using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Extensions;
using _Project.Scripts.Input;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Player;
using _Project.Scripts.Weapon;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Vector3 = System.Numerics.Vector3;

namespace _Project.Scripts.Factories
{
    public class PlayerShipFactory : ISceneInitializable
    {
        private const string ShipViewRemoteConfig = "CommonShipConfig"; //плейсхолдер до задания по скинам для корабля
        private const string PlayerSihipConfigRemoteKey = "PlayerShipConfig";

        private readonly IInstantiator _instantiator;
        private readonly IPlayerStateProviderService _playerDataProviderService;
        private readonly PlayerInputController _playerInputController;
        private readonly IPlayerWeaponInfoProviderService _playerWeaponInfoProviderService;
        private readonly WeaponFactory _weaponFactory;
        private readonly IScenesAssetProvider _assetProvider;
        private readonly IRemoteConfigService _remoteConfigService;

        private ShipViewConfig _shipViewConfig;
        private ShipPreferences _shipPreferences;
        private Ship _shipPrefab;
        private Sprite _shipSprite;
        private ShipViewModel _shipViewModel;

        public PlayerShipFactory(WeaponFactory weaponFactory, PlayerInputController playerInputController,
            IInstantiator instantiator,
            IPlayerStateProviderService playerDataProviderService, IScenesAssetProvider assetProvider,
            IPlayerWeaponInfoProviderService playerWeaponInfoProviderService, IRemoteConfigService remoteConfigService)
        {
            _weaponFactory = weaponFactory;
            _playerInputController = playerInputController;
            _playerDataProviderService = playerDataProviderService;
            _playerWeaponInfoProviderService = playerWeaponInfoProviderService;
            _assetProvider = assetProvider;
            _instantiator = instantiator;
            _remoteConfigService = remoteConfigService;
        }

        public async UniTask InitializeAsync()
        {
            _shipPreferences = _remoteConfigService.GetConfig<ShipPreferences>(PlayerSihipConfigRemoteKey);
            _shipViewConfig = _remoteConfigService.GetConfig<ShipViewConfig>(ShipViewRemoteConfig);

            _shipPrefab = (await _assetProvider.Load<GameObject>(_shipPreferences.PlayerShipPrefabAddress))
                .GetComponent<Ship>();

            _shipSprite = await _assetProvider.Load<Sprite>(_shipViewConfig.ShipSpriteAdress);
        }

        public async UniTask SpawnShip()
        {
            var shipModel = _instantiator.Instantiate<ShipModel>(new object[] { _shipPreferences });

            _shipViewModel = _instantiator.Instantiate<ShipViewModel>(new object[] { shipModel });

            _playerDataProviderService.ApplyPlayer(_shipViewModel);

            var shipView = _instantiator.InstantiatePrefabForComponent<Ship>(
                _shipPrefab,
                _shipPreferences.StartPosition.ToUnityVector3(),
                Quaternion.identity,
                null
            );

            await SpawnPlayerWeapons(shipView.PlayerWeapons);

            shipView.Initialize(_shipViewModel, _shipSprite);
        }

        private async UniTask SpawnPlayerWeapons(PlayerWeapons playerWeapons)
        {
            var heavyWeapons =
                await CreateWeapons(_shipViewConfig.HeavyWeaponType, _shipViewConfig.HeavyWeaponSlots, playerWeapons,
                    true);

            var mainWeapons =
                await CreateWeapons(_shipViewConfig.MainWeaponType, _shipViewConfig.LightWeaponSlots, playerWeapons);

            var weaponsViewModel = _instantiator.Instantiate<PlayerWeaponsViewModel>(new object[]
            {
                mainWeapons, heavyWeapons, _playerInputController,
                _playerDataProviderService.PositionProvider.Value
            });

            weaponsViewModel.Initialize();
            playerWeapons.Initialize(weaponsViewModel);
        }

        private async UniTask<List<WeaponViewModel>> CreateWeapons(WeaponType type, List<Vector3> positions,
            PlayerWeapons playerWeapons, bool addToUi = false)
        {
            var weapons = new List<WeaponViewModel>();
            var weaponIndex = 1;

            foreach (var position in positions)
            {
                var name = $"{type} {weaponIndex}";
                var newWeapon = await _weaponFactory.Create(type, name,
                    playerWeapons, position.ToUnityVector3());

                weapons.Add(newWeapon);

                if (addToUi)
                    _playerWeaponInfoProviderService.ApplyWeaponInfoProvider(newWeapon);

                weaponIndex++;
            }

            return weapons;
        }
    }
}
