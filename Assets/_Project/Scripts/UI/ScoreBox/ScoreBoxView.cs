using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.UI.ScoreBox
{
    public class ScoreBoxView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _score;

        private readonly CompositeDisposable _disposable = new();

        private ScoreBoxModel _model;

        [Inject]
        public void Construct(ScoreBoxModel model)
        {
            _model = model;

            _model.Enabled.Subscribe(enabled =>
                    gameObject.SetActive(enabled))
                .AddTo(_disposable);

            _model.Score.Subscribe(score =>
                    _score.text = score.ToString())
                .AddTo(_disposable);
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}
