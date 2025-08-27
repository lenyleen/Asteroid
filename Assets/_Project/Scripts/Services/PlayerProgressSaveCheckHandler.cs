using System;
using System.Globalization;
using _Project.Scripts.Data;
using _Project.Scripts.DTO;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Static;
using _Project.Scripts.UI.PopUps;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Services
{
    public class PlayerProgressSaveCheckHandler
    {
        private readonly UiService _uiService;
        private readonly RemoteSaveLoadService _remoteSaveLoadService;
        private readonly LocalSaveLoadService _localSaveLoadService;

        public PlayerProgressSaveCheckHandler(UiService uiService,  RemoteSaveLoadService remoteSaveLoadService,
            LocalSaveLoadService localSaveLoadService)
        {
            _uiService = uiService;
            _remoteSaveLoadService = remoteSaveLoadService;
            _localSaveLoadService = localSaveLoadService;
        }

        public async UniTask<ISaveService> SelectSaveService()
        {
            if(!_remoteSaveLoadService.IsAvailable)
                return _remoteSaveLoadService;

            var localLoadResult = await _localSaveLoadService.TryLoadData<PlayerProgress>(SaveKeys.PlayerProgressKey);

            var remoteLoadResult = await _remoteSaveLoadService.TryLoadData<PlayerProgress>(SaveKeys.PlayerProgressKey);

            switch (localLoadResult.Success)
            {
                case false when remoteLoadResult.Success:
                    await SaveDataToSaveService(remoteLoadResult, _localSaveLoadService);
                    break;
                case true when !remoteLoadResult.Success:
                    await SaveDataToSaveService(localLoadResult, _remoteSaveLoadService);
                    break;
                case true when remoteLoadResult.Success:
                    await ShowSelectionDialog(localLoadResult, remoteLoadResult, _localSaveLoadService, _remoteSaveLoadService);
                    break;
            }

            return _localSaveLoadService;
        }

        private async UniTask ShowSelectionDialog(DataSaveGetResult<PlayerProgress> localLoadResult,
            DataSaveGetResult<PlayerProgress> remoteLoadResult, LocalSaveLoadService localSaveLoadService,
            RemoteSaveLoadService remoteSaveLoadService)
        {
            if(localLoadResult.Data.Created <= remoteLoadResult.Data.Created)
            {
                await SaveDataToSaveService(remoteLoadResult, localSaveLoadService);
                return;
            }

            var popUp = await _uiService.ShowDialogAwaitable<SaveSelectionPopUp, SaveSelectionPopUpData>(
                new SaveSelectionPopUpData(localLoadResult.Data.Created.ToString(CultureInfo.CurrentCulture),
                    remoteLoadResult.Data.Created.ToString(CultureInfo.CurrentCulture)));

            await popUp.ShowDialogAsync(true);

            if (popUp.SelectedSaveType == SaveSelectionDataType.Local)
                await SaveDataToSaveService(localLoadResult, remoteSaveLoadService);
            else
                await SaveDataToSaveService(remoteLoadResult, localSaveLoadService);
        }

        private async UniTask SaveDataToSaveService(DataSaveGetResult<PlayerProgress> save, ISaveService to) =>
            await to.SaveData(save.Data.LoadedData, SaveKeys.PlayerProgressKey, save.Data.Created);
    }
}
