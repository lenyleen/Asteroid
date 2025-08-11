using _Project.Scripts.DTO;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Interfaces
{
    public interface ISaveService
    {
        public UniTask SaveData(PlayerProgress data);

        public UniTask<bool> TryLoadData(out PlayerProgress data);
    }
}
