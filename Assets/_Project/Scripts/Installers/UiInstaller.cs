using Factories;
using UI;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class UiInstaller : MonoInstaller<UiInstaller>
    {
        [SerializeField] private WeaponUiDataDisplayer _dataDisplayerPrefab;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<WeaponUiDataDisplayerFactory>()
                .AsSingle()
                .WithArguments(_dataDisplayerPrefab);
        }
    }
}
