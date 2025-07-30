using System;
using _Project.Scripts.DTO;
using Cysharp.Threading.Tasks;
using Interfaces;
using ModestTree;
using UniRx;
using UnityEngine;

namespace Services
{
    public class PlayerProgressProvider : IDisposable
    {
        const string PlayerProgressKey = "PlayerData";

        public PlayerProgress PlayerProgress => _currentPlayerProgress;

        private readonly ISaveService _saveLoadService;
        private readonly IEnemyDiedNotifier _enemyDiedNotifier;
        private readonly CompositeDisposable _disposable = new ();
        private readonly PlayerProgress _currentPlayerProgress;

        private PlayerProgress _loadedPlayerProgress;

        public PlayerProgressProvider(ISaveService saveLoadService, IEnemyDiedNotifier enemyDiedNotifier)
        {
            _saveLoadService = saveLoadService;
            _enemyDiedNotifier = enemyDiedNotifier;

            _currentPlayerProgress = new PlayerProgress(0);
            _currentPlayerProgress.InitializeReactiveValues();
        }

        public async UniTask<string> TryInitializeAsync()
        {
            _enemyDiedNotifier.OnEnemyKilled
                .Subscribe(enemyData =>
                    _currentPlayerProgress.AddScore(enemyData.ScoreReward))
                .AddTo(_disposable);

            var result = await _saveLoadService.TryLoadData(PlayerProgressKey, out _loadedPlayerProgress);

            if (result.IsEmpty())
                return result;

            _loadedPlayerProgress = _currentPlayerProgress;
            return result;
        }

        public async UniTask<string> TrySetDataAsync()
        {
            if (_loadedPlayerProgress.Score > _currentPlayerProgress.Score)
                return "";

            return await _saveLoadService.TrySaveData(PlayerProgressKey, _currentPlayerProgress);
        }

        public void ToDefault() =>
            _currentPlayerProgress.ToDefault();

        public void Dispose() =>
            _disposable.Dispose();
    }
}
