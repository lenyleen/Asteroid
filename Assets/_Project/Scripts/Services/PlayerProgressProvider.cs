using System;
using _Project.Scripts.DTO;
using _Project.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using ModestTree;
using UniRx;

namespace _Project.Scripts.Services
{
    public class PlayerProgressProvider : IDisposable, IAsyncInitializable
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

        public async UniTask InitializeAsync()
        {
            _enemyDiedNotifier.OnEnemyKilled
                .Subscribe(enemyData =>
                    PlayerProgress.AddScore(enemyData.ScoreReward))
                .AddTo(_disposable);

            var result = await _saveLoadService.TryLoadData(out _loadedPlayerProgress);

            if (!result)
                _loadedPlayerProgress = PlayerProgress;
        }

        public async UniTask SetDataAsync() =>
            await _saveLoadService.SaveData(PlayerProgress);

        public void ToDefault() =>
            PlayerProgress.ToDefault();

        public void Dispose() =>
            _disposable.Dispose();
    }
}
