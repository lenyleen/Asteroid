using System;
using Interfaces;
using UniRx;
using Zenject;

namespace UI.ScoreBox
{
    public class ScoreBoxModel : IInitializable, IDisposable
    {
        public readonly ReadOnlyReactiveProperty<int> Score;
        public IObservable<bool> Enabled => _enabled;

        private readonly ReactiveProperty<int> _score = new(0);
        private readonly ReactiveProperty<bool> _enabled = new(true);
        private readonly IEnemyDiedNotifier _enemyDiedNotifier;
        private readonly CompositeDisposable _disposables = new();

        public ScoreBoxModel(IEnemyDiedNotifier enemyDiedNotifier)
        {
            Score = new ReadOnlyReactiveProperty<int>(_score);
            _enemyDiedNotifier = enemyDiedNotifier;
        }

        public void Initialize()
        {
            _enemyDiedNotifier.OnEnemyKilled
                .Subscribe(AddScore)
                .AddTo(_disposables);
        }

        public void Enable(bool enable) => _enabled.Value = enable;

        public void ToDefault() => _score.Value = 0;

        public void Dispose() => _disposables.Dispose();

        private void AddScore(KilledEnemyData killedEnemyData)
        {
            _score.Value += killedEnemyData.ScoreReward;
        }
    }
}
