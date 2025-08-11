using _Project.Scripts.AssetLoaders;
using _Project.Scripts.Configs;
using _Project.Scripts.Services;
using _Project.Scripts.UI;
using Configs;
using Cysharp.Threading.Tasks;
using Services;
using Static;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class BootstrapInstaller : MonoInstaller<BootstrapInstaller>
    {
        [SerializeField] ProjectAssetLoader _projectAssetLoader;

        public override void InstallBindings()
        {
            Container.Bind<ProjectAssetLoader>()
                .FromInstance(_projectAssetLoader)
                .AsSingle();
        }
    }
}
