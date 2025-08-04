using System;
using _Project.Scripts.DTO;
using _Project.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using ModestTree;
using UniRx;

namespace _Project.Scripts.Services
{
    public class PlayerProgressProvider : IDisposable
    {
        public PlayerProgress PlayerProgress { get; }

        private readonly ISaveService _saveLoadService;
        private readonly IEnemyDiedNotifier _enemyDiedNotifier;
        private readonly CompositeDisposable _disposable = new();

        private PlayerProgress _loadedPlayerProgress;

        public PlayerProgressProvider(ISaveService saveLoadService, IEnemyDiedNotifier enemyDiedNotifier)
        {
            _saveLoadService = saveLoadService;
            _enemyDiedNotifier = enemyDiedNotifier;

            PlayerProgress = new PlayerProgress(0);
            PlayerProgress.InitializeReactiveValues();
        }

        public async UniTask<string> TryInitializeAsync()
        {
            _enemyDiedNotifier.OnEnemyKilled
                .Subscribe(enemyData =>
                    PlayerProgress.AddScore(enemyData.ScoreReward))
                .AddTo(_disposable);

            var result = await _saveLoadService.TryLoadData(out _loadedPlayerProgress);

            if (result.IsEmpty())
                return result;

            _loadedPlayerProgress = PlayerProgress;
            return result;
        }

        public async UniTask<string> TrySetDataAsync()
        {
            if (_loadedPlayerProgress.Score > PlayerProgress.Score)
                return "";

            return await _saveLoadService.TrySaveData(PlayerProgress);
        }

        public void ToDefault()
        {
            PlayerProgress.ToDefault();
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
