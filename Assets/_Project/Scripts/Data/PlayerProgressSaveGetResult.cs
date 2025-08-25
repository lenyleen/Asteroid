using _Project.Scripts.Services;

namespace _Project.Scripts.Data
{
    public class PlayerProgressSaveGetResult
    {
        public bool Success { get; private set; }
        public SavedPlayerData Data { get; private set; }

        public PlayerProgressSaveGetResult(bool success, SavedPlayerData data)
        {
            Success = success;
            Data = data;
        }
    }
}
