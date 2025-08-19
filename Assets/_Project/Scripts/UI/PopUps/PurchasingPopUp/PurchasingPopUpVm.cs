using System;
using _Project.Scripts.Configs.PopUpsConfigs;
using _Project.Scripts.Data;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Other;
using _Project.Scripts.Services;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Purchasing;

namespace _Project.Scripts.UI.PopUps
{
    public class PurchasingPopUpVm : IPopUpViewModel
    {
        public IObservable<Sprite> Sprite => _sprite;
        public IObservable<string> Message => _message;
        public IObservable<string> Title => _title;
        public IObservable<decimal> Price => _price;
        public IObservable<int> Quantity => _quantity;
        public IObservable<PurchaseDialogStates> State => _state;

        private readonly IRemoteConfigService _configService;
        private readonly IProjectAssetProvider _assetsProvider;
        private readonly PurchasingService _purchasingService;
        private readonly PurchasePopUpData _data;

        private ReactiveProperty<Sprite> _sprite;
        private ReactiveProperty<string> _message;
        private ReactiveProperty<PurchaseDialogStates> _state;
        private ReactiveProperty<string> _title;
        private ReactiveProperty<decimal>  _price;
        private ReactiveProperty<int> _quantity;

        private PurchasePopUpConfig _config;

        public PurchasingPopUpVm(IRemoteConfigService configService, IProjectAssetProvider assetsProvider,
            PurchasingService purchasingService, PurchasePopUpData data)
        {
            _configService = configService;
            _assetsProvider = assetsProvider;
            _purchasingService = purchasingService;
            _data = data;
        }

        public async UniTask Initialize()
        {
            _config = _configService.GetConfig<PurchasePopUpConfig>(_data.ConfigId);

            _state.Value = PurchaseDialogStates.Idle;
            _message.Value = _config.Description;
            _title.Value = _config.Name;
            _price.Value = _config.Price;
            _quantity.Value = _config.Quantity;

            _sprite.Value = await _assetsProvider.Load<Sprite>(_config.SpriteAddress);
        }

        public async void Buy()
        {
            try
            {
                await _purchasingService.Buy(_config.ProductId);
                //меняем стейт, мессадж и кнопку(часть в стейт машине попаппа)
            }
            catch (OperationCanceledException exception)
            {
                //см. ниже
            }
            catch (Exception e)
            {
                //меняем состояние на ошибку
            }

            //purchaseService.ConfimrOrder(order)
            //в стейт конфирмед и позволяем закрыться без ошибок
        }

        public void Cancel()
        {
            //if confirmed - закрываем
            //если еще ждет то зкрываем токен и ловим ошибку
        }
    }
}

