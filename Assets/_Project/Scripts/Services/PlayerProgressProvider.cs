using System;
using _Project.Scripts.DTO;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Static;
using Cysharp.Threading.Tasks;
using UniRx;

namespace _Project.Scripts.Services
{
    public class PlayerProgressProvider : IDisposable, ISceneInitializable
    {
        public PlayerProgress PlayerProgress { get; }

        private readonly SaveServiceProvider _saveServiceProvider;
        private readonly LocalSaveLoadService _localSaveLoadService;
        private readonly IEnemyDiedNotifier _enemyDiedNotifier;
        private readonly CompositeDisposable _disposable = new();

        private PlayerProgress _loadedPlayerProgress;

        public PlayerProgressProvider(IEnemyDiedNotifier enemyDiedNotifier,
            LocalSaveLoadService localSaveLoadService, SaveServiceProvider saveServiceProvider)
        {
            _saveServiceProvider = saveServiceProvider;
            _enemyDiedNotifier = enemyDiedNotifier;
            _localSaveLoadService = localSaveLoadService;
            _saveServiceProvider = saveServiceProvider;

            PlayerProgress = new PlayerProgress(0);
            PlayerProgress.InitializeReactiveValues();
        }

        public async UniTask InitializeAsync()
        {
            _enemyDiedNotifier.OnEnemyKilled
                .Subscribe(enemyData =>
                    PlayerProgress.AddScore(enemyData.ScoreReward))
                .AddTo(_disposable);

            var result = await _saveServiceProvider.SaveService.TryLoadData<PlayerProgress>(SaveKeys.PlayerProgressKey);

            if(!result.Success)
                result = await _localSaveLoadService.TryLoadData<PlayerProgress>(SaveKeys.PlayerProgressKey);

            _loadedPlayerProgress = !result.Success ? PlayerProgress : result.Data.LoadedData;
        }

        public async UniTask SetDataAsync()
        {
            var progressToSave = _loadedPlayerProgress.Score > PlayerProgress.Score ?
                _loadedPlayerProgress : PlayerProgress;

            try
            {
                await _saveServiceProvider.SaveService.SaveData(progressToSave, SaveKeys.PlayerProgressKey, DateTime.UtcNow);
            }
            catch (Exception)
            {
                await _localSaveLoadService.SaveData(progressToSave, SaveKeys.PlayerProgressKey, DateTime.UtcNow);
                throw;
            }
        }

        public void ToDefault() =>
            PlayerProgress.ToDefault();

        public void Dispose() =>
            _disposable.Dispose();
    }
}
