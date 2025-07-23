namespace Interfaces
{
    public interface IWeaponUiDataDisplayer
    {
        public string Name { get; }

        public void Initialize(IWeaponInfoProvider infoProvider);

        public void Hide();
    }
}
