using System;
using UniRx;

namespace UI.ScoreBox
{
    public class ScoreBoxModel
    {
        public IObservable<int> Score;
        public IObservable<bool> Enabled;

        private readonly ReactiveProperty<int> _score;
        private readonly ReactiveProperty<bool> _enabled;

        public void Enable(bool enable) => _enabled.Value = enable;
    }
}
