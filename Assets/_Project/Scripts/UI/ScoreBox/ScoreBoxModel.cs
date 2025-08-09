using System;
using _Project.Scripts.Services;
using UniRx;
using Zenject;

namespace _Project.Scripts.UI.ScoreBox
{
    public class ScoreBoxModel : IInitializable, IDisposable
    {
        public readonly ReadOnlyReactiveProperty<int> Score;
        public IObservable<bool> Enabled => _enabled;

        private readonly ReactiveProperty<int> _score = new(0);
        private readonly ReactiveProperty<bool> _enabled = new(true);
        private readonly PlayerProgressProvider _playerProgressProvider;
        private readonly CompositeDisposable _disposables = new();

        public ScoreBoxModel(PlayerProgressProvider playerProgressProvider)
        {
            Score = new ReadOnlyReactiveProperty<int>(_score);
            _playerProgressProvider = playerProgressProvider;
        }

        public void Initialize()
        {
            _playerProgressProvider.PlayerProgress.ReactiveScore
                .Subscribe(score =>  _score.Value = score)
                .AddTo(_disposables);
        }

        public void Enable(bool enable) => _enabled.Value = enable;

        public void Dispose() => _disposables.Dispose();
    }
}
