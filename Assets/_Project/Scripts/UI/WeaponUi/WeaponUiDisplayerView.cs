using UniRx;
using UnityEngine;
using Zenject;

namespace UI.WeaponUi
{
    public class WeaponUiDisplayerView : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        private readonly CompositeDisposable _disposables = new();

        private WeaponUiDisplayerViewModel _viewModel;

        [Inject]
        private void Initialize(WeaponUiDisplayerViewModel viewModel)
        {
            _viewModel = viewModel;

            _viewModel.IsEnabled.Subscribe(enable =>
                    gameObject.SetActive(enable))
                .AddTo(_disposables);

            _viewModel.OnDisplayerAdded.Subscribe(ApplyDisplayer);
        }

        private void ApplyDisplayer(WeaponUiDataDisplayer displayer)
        {
            displayer.transform.SetParent(_rectTransform);
        }
    }
}
