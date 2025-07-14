using System;
using DataObjects;
using Player;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Installers
{
    public class PlayerInstaller : Installer<PlayerInstaller.PlayerInstallData,PlayerInstaller>
    {
        private PlayerInstallData _playerInstallData;

        public PlayerInstaller(PlayerInstallData playerInstallData)
        {
            _playerInstallData = playerInstallData;
        }

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerModel>().AsSingle()
                .WithArguments(_playerInstallData.PlayerPreferences);
            Container.BindInterfacesAndSelfTo<PlayerViewModel>().AsSingle();
            
            var player = Container.InstantiatePrefabForComponent<Player.Player>(_playerInstallData.PlayerPrefab
                ,_playerInstallData.PlayerSpawnPosition.position,Quaternion.identity,null);

            Container.BindInterfacesAndSelfTo<Player.Player>()
                .FromInstance(player).AsSingle();
        }
        
        [Serializable]
        public class PlayerInstallData
        {
            [field: SerializeField] public Player.Player PlayerPrefab { get; private set; }
            [field: SerializeField] public Transform PlayerSpawnPosition { get;  private set; } 
            [field: SerializeField] public PlayerPreferences PlayerPreferences { get; private set; }
        }
    }
}