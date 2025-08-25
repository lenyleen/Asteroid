namespace _Project.Scripts.Interfaces
{
    public interface IRemoteConfigService : IProjectImportanceInitializable
    {
        public T GetConfig<T>(string key);
    }
}
