using _Project.Scripts.DTO;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Interfaces
{
    public interface ISaveService
    {
        public UniTask<string> TrySaveData(object data);

        public UniTask<string> TryLoadData(out PlayerProgress data);
    }
}
