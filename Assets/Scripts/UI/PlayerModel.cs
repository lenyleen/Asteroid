using System;
using Enemy;
using UniRx;

namespace UI
{
    [Serializable]
    public class PlayerModel
    {
        public ReactiveProperty<int> Score { get; } = new();

        private int _score;
        private string _name;
        
        public void SavePlayerDataToScore(string playerName)
        {
            Score.Value = 0;
        }

        public void UpdateScore(int scoreToAdd)
        {
            Score.Value += scoreToAdd;
        }
    }
}