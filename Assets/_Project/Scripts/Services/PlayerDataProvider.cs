using _Project.Scripts.DTO;
using Cysharp.Threading.Tasks;
using Interfaces;
using ModestTree;

namespace Services
{
    public class PlayerDataProvider
    {
        const string PlayerDataName = "PlayerData";

        private readonly ISaveService _saveLoadService;

        private PlayerData _playerData;

        public PlayerData PlayerData => _playerData;

        public async UniTask<string> TryInitializeAsync()
        {
            var result = await _saveLoadService.TryLoadData(PlayerDataName, out _playerData);

            if (!result.IsEmpty())
            {
                _playerData = new PlayerData { PlayerName = "", Score = 0 };
            }

            return result;
        }

        public async UniTask<string> TrySetDataAsync(PlayerData playerData)
        {
            if (_playerData.Score > playerData.Score)
            {
                return "";
            }

            return await _saveLoadService.TrySaveData(PlayerDataName, playerData);
        }
    }
}
