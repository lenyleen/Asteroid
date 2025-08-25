using _Project.Scripts.Data;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Other;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.PopUps
{
    public class SaveSelectionPopUp : PopUpBase, IDialog<SaveSelectionPopUpData>
    {
        [SerializeField] private ToggleGroup _toggleGroup;
        [SerializeField] private SaveSelectionToggle _localSaveToggle;
        [SerializeField] private SaveSelectionToggle _remoteSaveToggle;
        [SerializeField] private Button _okButton;


        public SaveSelectionDataType SelectedSaveType { get; private set; }

        private SaveSelectionPopUpData _data;

        public void SetParams(SaveSelectionPopUpData data)
        {
            _data = data;
        }

        public async UniTask<DialogResult> ShowDialogAsync(bool hideAfterChoice)
        {
            _localSaveToggle.Initialize(_data.LocalSaveCreatedAt);
            _remoteSaveToggle.Initialize(_data.RemoteSaveCreatedAt);

            var tcs = new UniTaskCompletionSource<DialogResult>();

            gameObject.SetActive(true);

            _okButton.OnClickAsObservable().Subscribe(_ =>
            {
                var activeToggle = _toggleGroup.GetFirstActiveToggle();
                SelectedSaveType = activeToggle.GetComponent<SaveSelectionToggle>().SelectedSaveType;

                tcs.TrySetResult(DialogResult.Yes);

                HideAfterChoice(hideAfterChoice);
            }).AddTo(_disposables);

            _closeButton.OnClickAsObservable().Subscribe(_ =>
            {
                tcs.TrySetResult(DialogResult.Cancel);
                HideAfterChoice(hideAfterChoice);
            }).AddTo(_disposables);

            return await tcs.Task;
        }

        public override void Show() =>
            Debug.LogWarning("Can't show SaveSelectionPopUp without parameters");
    }
}
