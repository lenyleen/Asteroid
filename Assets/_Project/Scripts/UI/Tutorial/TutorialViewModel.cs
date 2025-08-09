using System;
using UniRx;

namespace _Project.Scripts.UI.Tutorial
{
    public class TutorialViewModel
    {
        private readonly ReactiveProperty<bool> _isEnabled = new(false);
        public IObservable<bool> IsTutorialEnabled => _isEnabled;

        public void Enable(bool enable) =>
            _isEnabled.Value = enable;
    }
}
