using System;
using _Project.Scripts.Services;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

namespace _Project.Scripts.Other
{
    public class PurchaseBtn : MonoBehaviour
    {
        private PurchasingService _service;
        private Button _button;
        /*void Start()
        {
            _service = new PurchasingService();
            _service.Init();
            _button = gameObject.GetComponentInChildren<Button>();
            _button.onClick.AsObservable().Subscribe(_ => sus());
        }

        void sus()
        {
            _service.Buy();
        }*/
    }
}
