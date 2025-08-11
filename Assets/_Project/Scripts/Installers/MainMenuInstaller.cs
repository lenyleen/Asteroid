using _Project.Scripts.Other;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class MainMenuInstaller : MonoInstaller //TODO: плейсхолдер до введения меню
    {
        [SerializeField] private MainMenuPlayButtonTemp _temp;

        public override void InstallBindings()
        {
            Container.Bind<MainMenuPlayButtonTemp>()
                .FromInstance(_temp)
                .AsSingle();
        }
    }
}
