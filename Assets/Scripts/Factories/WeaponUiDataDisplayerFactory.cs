using Interfaces;
using UI;
using Zenject;

namespace Factories
{
    public class WeaponUiDataDisplayerFactory : IFactory<IWeaponInfoProvider,IWeaponUiDataDisplayer>
    {
        private readonly WeaponUiDataDisplayer _prefab;
        private readonly DiContainer _container;

        public WeaponUiDataDisplayerFactory(WeaponUiDataDisplayer prefab, DiContainer container)
        {
            _prefab = prefab;
            _container = container;
        }
        public IWeaponUiDataDisplayer Create(IWeaponInfoProvider provider)
        {
            var displayer = _container.InstantiatePrefabForComponent<IWeaponUiDataDisplayer>(_prefab);
            displayer.Initialize(provider);
            return displayer;
        }
    }
}