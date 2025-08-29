using _Project.Scripts.Factories;
using _Project.Scripts.Other;
using _Project.Scripts.Pools;
using _Project.Scripts.Services;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class FxInstaller : MonoInstaller<FxInstaller>
    {
        [SerializeField] private SoundPlayer _soundPlayerPrefab;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ParticleFactory>()
                .AsSingle();

            Container.Bind<ParticlePool>()
                .AsSingle();

            Container.BindMemoryPool<SoundPlayer, SoundPlayer.Pool>()
                .WithInitialSize(20)
                .FromComponentInNewPrefab(_soundPlayerPrefab)
                .UnderTransformGroup("SoundPlayers");

            Container.BindInterfacesAndSelfTo<FxService>()
                .AsSingle();
        }
    }
}
