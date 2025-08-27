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

        private readonly ISaveService _saveLoadService;
        private readonly LocalSaveLoadService _localSaveLoadService;
        private readonly IEnemyDiedNotifier _enemyDiedNotifier;
        private readonly CompositeDisposable _disposable = new();

        private PlayerProgress _loadedPlayerProgress;

        public PlayerProgressProvider(ISaveService saveLoadService, IEnemyDiedNotifier enemyDiedNotifier,
            LocalSaveLoadService localSaveLoadService)
        {
            _saveLoadService = saveLoadService;
            _enemyDiedNotifier = enemyDiedNotifier;
            _localSaveLoadService = localSaveLoadService;

            PlayerProgress = new PlayerProgress(0);
            PlayerProgress.InitializeReactiveValues();
        }

        public async UniTask InitializeAsync()
        {
            _enemyDiedNotifier.OnEnemyKilled
                .Subscribe(enemyData =>
                    PlayerProgress.AddScore(enemyData.ScoreReward))
                .AddTo(_disposable);

            var result = await _saveLoadService.TryLoadData<PlayerProgress>(SaveKeys.PlayerProgressKey);

            if(!result.Success)
                result = await _localSaveLoadService.TryLoadData<PlayerProgress>(SaveKeys.PlayerProgressKey);

            if (!result.Success)
                _loadedPlayerProgress = PlayerProgress;
        }

        public async UniTask SetDataAsync()
        {
            var progressToSave = _loadedPlayerProgress.Score > PlayerProgress.Score ?
                _loadedPlayerProgress : PlayerProgress;

            try
            {
                await _saveLoadService.SaveData(progressToSave, SaveKeys.PlayerProgressKey, DateTime.UtcNow);
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
