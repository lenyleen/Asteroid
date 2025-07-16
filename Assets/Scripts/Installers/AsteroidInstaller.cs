using System.Collections.Generic;
using DataObjects;
using Projectiles;
using Signals;
using UnityEngine;
using Weapon;
using Zenject;

namespace Installers
{
    public class AsteroidInstaller : MonoInstaller<AsteroidInstaller>
    {
        [SerializeField] private PlayerInstaller.PlayerInstallData  _playerInstallData;
        [SerializeField] private WeaponView _weaponViewPrefab;
        [SerializeField] private List<ProjectileData> _projectileDatas;
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private Enemy.Enemy _enemyPrefab;
        [SerializeField] private int _maxEnemies;
        [SerializeField] private List<EnemyData> _enemyDatas;
        [SerializeField] private Camera _camera;
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<EnemyDestroyedSignal>();
            Container.Bind<Camera>().FromInstance(_camera).AsSingle();
            GameInstaller.Install(Container);
            PlayerInstaller.Install(Container,  _playerInstallData,_projectilePrefab, _projectileDatas, _weaponViewPrefab);
            EnemyInstaller.Install(Container, _enemyPrefab, _maxEnemies, _enemyDatas);
        }
        
    }
}