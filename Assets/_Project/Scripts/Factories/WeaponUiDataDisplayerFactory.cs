using Interfaces;
using UI;
using Zenject;

namespace Factories
{
    public class WeaponUiDataDisplayerFactory : IFactory<IWeaponInfoProvider, WeaponUiDataDisplayer>
    {
        private readonly IInstantiator _instantiator;
        private readonly WeaponUiDataDisplayer _prefab;

        public WeaponUiDataDisplayerFactory(WeaponUiDataDisplayer prefab, DiContainer instantiator)
        {
            _prefab = prefab;
            _instantiator = instantiator;
        }

        public WeaponUiDataDisplayer Create(IWeaponInfoProvider provider)
        {
            var displayer = _instantiator.InstantiatePrefabForComponent<WeaponUiDataDisplayer>(_prefab);
            displayer.Initialize(provider);
            return displayer;
        }
    }
}
