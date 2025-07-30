using System;
using Interfaces;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.DTO
{
    [Serializable]
    public class PlayerProgress : ISavableData
    {
        [field:SerializeField] public int Score { get; private set; }

        [field: NonSerialized]public ReadOnlyReactiveProperty<int> ReactiveScore { get; private set; }

        [NonSerialized]private ReactiveProperty<int> _reactiveScore;

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
