using UnityEngine;
using Zenject;

namespace Installers
{
    public class AsteroidInstaller : MonoInstaller<AsteroidInstaller>
    {
        [SerializeField] private PlayerInstaller.PlayerInstallData  _playerInstallData;
        public override void InstallBindings()
        {
            Container.Bind<Camera>().FromInstance(Camera.main).AsSingle();
            PlayerInstaller.Install(Container,  _playerInstallData);
            GameInstaller.Install(Container);
        }
    }
}