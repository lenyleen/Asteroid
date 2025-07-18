using Interfaces;
using UI;
using Zenject;

namespace Factories
{
    public class WeaponUiDataDisplayerFactory : IFactory<IWeaponInfoProvider,IWeaponUiDataDisplayer>
    {
        private readonly WeaponUiDataDisplayer _prefab;
        private readonly IInstantiator _instantialor;

        public WeaponUiDataDisplayerFactory(WeaponUiDataDisplayer prefab, DiContainer instantialor)
        {
            _prefab = prefab;
            _instantialor = instantialor;
        }
        public IWeaponUiDataDisplayer Create(IWeaponInfoProvider provider)
        {
            var displayer = _instantialor.InstantiatePrefabForComponent<IWeaponUiDataDisplayer>(_prefab);
            displayer.Initialize(provider);
            return displayer;
        }
    }
}