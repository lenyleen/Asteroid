using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Interfaces
{
    public interface IAsyncInitializable
    {
        public UniTask InitializeAsync();
    }

    public interface IBootstrapInitializable : IAsyncInitializable
    {
    }

    public interface ISceneInitializable : IAsyncInitializable
    {
    }

    public interface IProjectImportanceInitializable : IAsyncInitializable
    {
    }
}
