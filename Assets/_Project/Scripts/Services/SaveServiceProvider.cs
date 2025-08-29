using _Project.Scripts.Interfaces;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Services
{
    public class SaveServiceProvider : IProjectImportanceInitializable
    {
        public ISaveService SaveService { get; private set; }

        private readonly PlayerProgressSaveCheckHandler  _playerProgressSaveCheckHandler;

        public SaveServiceProvider(PlayerProgressSaveCheckHandler playerProgressSaveCheckHandler)
        {
            _playerProgressSaveCheckHandler = playerProgressSaveCheckHandler;
        }

        public async UniTask InitializeAsync()
        {
            SaveService = await _playerProgressSaveCheckHandler.SelectSaveService();
        }
    }
}
