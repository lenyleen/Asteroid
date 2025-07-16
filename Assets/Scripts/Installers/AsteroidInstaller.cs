using System.Collections.Generic;
using DataObjects;
using Projectiles;
using Signals;
using UI;
using UnityEngine;
using Weapon;
using Zenject;

namespace Installers
{
    public class AsteroidInstaller : MonoInstaller<AsteroidInstaller>
    {
        [SerializeField] private ShipInstaller.PlayerInstallData  _playerInstallData;
        [SerializeField] private WeaponView _weaponViewPrefab;
        [SerializeField] private List<ProjectileData> _projectileDatas;
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private Enemy.Enemy _enemyPrefab;
        [SerializeField] private int _maxEnemies;
        [SerializeField] private List<EnemyData> _enemyDatas;
        [SerializeField] private Camera _camera;
        [SerializeField]private InGameUi _inGameUi;
        [SerializeField]private WeaponUiDataDisplayer  _dataDisplayerPrefab;
        [SerializeField]private PlayerUiView _playerUi;
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            InstallSignals();
            Container.Bind<Camera>().FromInstance(_camera).AsSingle();
            GameInstaller.Install(Container);
            ShipInstaller.Install(Container,  _playerInstallData,_projectilePrefab, _projectileDatas, _weaponViewPrefab);
            EnemyInstaller.Install(Container, _enemyPrefab, _maxEnemies, _enemyDatas);
            UiInstaller.Install(Container, _inGameUi,_dataDisplayerPrefab, _playerUi);
        }


        private void InstallSignals()
        {
            Container.DeclareSignal<EnemyDestroyedSignal>();
            Container.DeclareSignal<GameStarted>();
            Container.DeclareSignal<GameEnded>();
        }
    }
}