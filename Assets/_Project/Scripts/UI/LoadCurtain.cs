using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LoadCurtain : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _loadingText;
        [SerializeField] private float duration = 1f;

        public async UniTask FadeOutAsync()
        {
            var textFadeOut = _loadingText.DOFade(0, duration)
                .SetEase(Ease.InOutQuad)
                .AsyncWaitForCompletion()
                .AsUniTask();

            var imageFadeout =  _image.DOFade(0, duration)
                .SetEase(Ease.InOutQuad)
                .AsyncWaitForCompletion()
                .AsUniTask();

            await UniTask.WhenAll(textFadeOut, imageFadeout);

            gameObject.SetActive(false);
        }
    }
}
