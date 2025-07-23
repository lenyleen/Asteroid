using System;
using _Project.Scripts.DTO;
using Interfaces;
using UniRx;

namespace UI
{
    [Serializable]
    public class PlayerModel
    {
        private readonly PlayerData _playerData;

        private readonly ISaveService _saveService;

        private string _name;
        private int _score;

        public PlayerModel(ISaveService saveService)
        {
            _saveService = saveService;
        }

        public ReactiveProperty<int> Score { get; } = new();

        public async void SavePlayerDataToScore(string playerName)
        {
            await _saveService.TrySaveData("PlayerProgress",
                new PlayerData { PlayerName = playerName, Score = Score.Value });
            //TODO
        }

        public void UpdateScore(int scoreToAdd)
        {
            Score.Value += scoreToAdd;
        }
    }
}
