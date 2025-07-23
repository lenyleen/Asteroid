using Interfaces;
using UI;
using Zenject;

namespace Factories
{
    public class WeaponUiDataDisplayerFactory : IFactory<IWeaponInfoProvider, IWeaponUiDataDisplayer>
    {
        private readonly IInstantiator _instantiator;
        private readonly WeaponUiDataDisplayer _prefab;

        public WeaponUiDataDisplayerFactory(WeaponUiDataDisplayer prefab, DiContainer instantiator)
        {
            _prefab = prefab;
            _instantiator = instantiator;
        }

        public IWeaponUiDataDisplayer Create(IWeaponInfoProvider provider)
        {
            var displayer = _instantiator.InstantiatePrefabForComponent<IWeaponUiDataDisplayer>(_prefab);
            displayer.Initialize(provider);
            return displayer;
        }
    }
}
