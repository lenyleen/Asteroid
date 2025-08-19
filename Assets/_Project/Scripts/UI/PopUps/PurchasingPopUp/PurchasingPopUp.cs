using System;
using _Project.Scripts.Data;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Other;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.PopUps
{
    public class PurchasingPopUp : PopUpBase, IPopUp<PurchasingPopUpVm>
    {
        [SerializeField] private GameObject _pendingPanel;
        [SerializeField] private GameObject _confirmedPanel;
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private Button _okButton;
        [SerializeField] private Button _cancelButton;

        private GameObject _currentPanel;
        private IDisposable _currentBtnAction;
        private PurchasePopUpData _data;

        public void Show(PurchasingPopUpVm vm)
        {

        }

        public void SetState(PurchaseDialogStates state)
        {
            //TODO: нужна стейт машина для управления всем что будет на попапе(иконка, цена, сообщение, экшен кнопки)
        }

        private void ChangeState(GameObject panel, Button button, Action listener)
        {
            _currentBtnAction?.Dispose();
            _currentPanel?.SetActive(false);

            _currentBtnAction = button.OnClickAsObservable().Subscribe(_ => listener?.Invoke());
            _currentPanel = panel;
            _currentPanel.SetActive(true);
        }

        public override void Show() {}
    }
}
