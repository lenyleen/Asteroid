using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Extensions
{
    public static class AudioAsyncExtension
    {
        public static UniTask PlayAsync(this AudioSource audioSource)
        {
            if (audioSource.clip == null)
                return UniTask.CompletedTask;

            audioSource.Play();

            return UniTask.WaitWhile(() => !audioSource.isPlaying);
        }
    }
}
