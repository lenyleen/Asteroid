using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Interfaces
{
    public interface ISaveService
    {
        public UniTask<string> TrySaveData(string name, object data);

        public UniTask<string> TryLoadData<T>(string name, out T data) where T : class, ISavableData;
    }
}
