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
            _bookKeepingInGameData.PlayerLife.Subscribe(lifeValue =>
            {
                textPlayerLife.text = $"Player Life : {lifeValue}";
            }).AddTo(disposables);
        }

        private void OnDisable()
        {
            disposables.Clear();
        }
    }

}
