using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Interfaces
{
    public interface IAsyncInitializable
    {
        public UniTask InitializeAsync();
    }
}
