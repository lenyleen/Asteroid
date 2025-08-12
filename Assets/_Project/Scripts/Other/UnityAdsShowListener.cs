using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;

namespace _Project.Scripts.Other
{
    public class UnityAdsShowListener : IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private UniTaskCompletionSource<bool> _loadTcs;
        private UniTaskCompletionSource<UnityAdsShowCompletionState> _showTcs;

        public UniTask<bool> LoadAsync(string placementId)
        {
            _loadTcs = new UniTaskCompletionSource<bool>();
            Advertisement.Load(placementId, this);
            return _loadTcs.Task;
        }

        public UniTask<UnityAdsShowCompletionState> ShowAsync(string placementId)
        {
            _showTcs = new UniTaskCompletionSource<UnityAdsShowCompletionState>();
            Advertisement.Show(placementId, this);
            return _showTcs.Task;
        }

        public void OnUnityAdsAdLoaded(string placementId) =>
            _loadTcs?.TrySetResult(true);

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState state) =>
            _showTcs?.TrySetResult(state);

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) =>
            _loadTcs?.TrySetException(new Exception($"Load failed: {error} - {message}"));

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) =>
            _showTcs?.TrySetException(new Exception($"Show failed: {error} - {message}"));

        public void OnUnityAdsShowStart(string placementId) { }
        public void OnUnityAdsShowClick(string placementId) { }
    }
}
