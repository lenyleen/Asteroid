using System;
using _Project.Scripts.Interfaces;
using Newtonsoft.Json;
using UniRx;

namespace _Project.Scripts.DTO
{
    [Serializable]
    public class PlayerProgress : ILoadedData
    {
        [JsonProperty] public int Score { get; private set; }

        [JsonIgnore]public ReadOnlyReactiveProperty<int> ReactiveScore { get; private set; }

        [JsonIgnore] private ReactiveProperty<int> _reactiveScore;

        public PlayerProgress(int score)
        {
            Score = score;
        }

        public void InitializeReactiveValues()
        {
            _reactiveScore = new ReactiveProperty<int>(Score);
            ReactiveScore = new ReadOnlyReactiveProperty<int>(_reactiveScore);
        }

        public void ToDefault()
        {
            Score = 0;
            _reactiveScore.Value = 0;
        }

        public void AddScore(int score)
        {
            Score += score;
            _reactiveScore.Value += score;
        }
    }
}
