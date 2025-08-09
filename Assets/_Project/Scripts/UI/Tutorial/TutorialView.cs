using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.UI.Tutorial
{
    public class TutorialView : MonoBehaviour
    {
        private readonly CompositeDisposable _disposables = new();

        [Inject]
        public void Initialize(TutorialViewModel viewModel)
        {
            viewModel.IsTutorialEnabled.Subscribe(enable
                    => gameObject.SetActive(enable))
                .AddTo(_disposables);
        }

        private void OnDestroy() =>
            _disposables.Dispose();
    }
}
