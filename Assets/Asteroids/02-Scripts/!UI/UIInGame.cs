namespace Asteroid.UI
{
    using HandyPackage;
    using TMPro;
    using UniRx;
    using UnityEngine;

    public class UIInGame : MonoBehaviour
    {
        public TextMeshProUGUI textPlayerLife;
        public TextMeshProUGUI textScore;
        public TextMeshProUGUI textHighScore;

        private BookKeepingInGameData _bookKeepingInGameData;

        private CompositeDisposable disposables = new CompositeDisposable();

        private void Awake()
        {
            _bookKeepingInGameData = DIResolver.GetObject<BookKeepingInGameData>();
        }

        private void OnEnable()
        {
            _bookKeepingInGameData.PlayerLife.Subscribe(val =>
            {
                textPlayerLife.text = $"Player Life : {val}";
            }).AddTo(disposables);

            _bookKeepingInGameData.Score.Subscribe(val =>
            {
                textScore.text = $"Score : {val}";
            }).AddTo(disposables);

            _bookKeepingInGameData.HighScore.Subscribe(val =>
            {
                textHighScore.text = $"{val}";
            }).AddTo(disposables);
        }

        private void OnDisable()
        {
            disposables.Clear();
        }
    }

}
