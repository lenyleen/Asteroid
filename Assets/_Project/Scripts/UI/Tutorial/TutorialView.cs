using UniRx;
using UnityEngine;
using Zenject;

namespace UI
{
    public class TutorialView : MonoBehaviour
    {
        private readonly CompositeDisposable _disposables = new();

        private void OnDestroy()
        {
            _disposables.Dispose();
        }

        [Inject]
        public void Initialize(TutorialViewModel viewModel)
        {
            viewModel.IsTutorialEnabled.Subscribe(enable
                    => gameObject.SetActive(enable))
                .AddTo(_disposables);
        }
    }
}
