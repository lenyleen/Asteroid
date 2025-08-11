using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
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

        public async UniTask FadeInAsync()
        {
            gameObject.SetActive(true);
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0);
            _loadingText.color = new Color(_loadingText.color.r, _loadingText.color.g, _loadingText.color.b, 0);

            var textFadeIn = _loadingText.DOFade(1, duration)
                .SetEase(Ease.InOutQuad)
                .AsyncWaitForCompletion()
                .AsUniTask();

            var imageFadeIn =  _image.DOFade(1, duration)
                .SetEase(Ease.InOutQuad)
                .AsyncWaitForCompletion()
                .AsUniTask();

            await UniTask.WhenAll(textFadeIn, imageFadeIn);
        }
    }
}
