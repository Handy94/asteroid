namespace Asteroid.UI
{
    using HandyPackage;
    using System.Threading.Tasks;
    using TMPro;
    using UniRx;

    public class UIInGame : UISceneBase
    {
        public TextMeshProUGUI textPlayerLife;
        public TextMeshProUGUI textScore;
        public TextMeshProUGUI textHighScore;

        private BookKeepingInGameData _bookKeepingInGameData;

        protected override Task OnUISceneInit()
        {
            _bookKeepingInGameData = DIResolver.GetObject<BookKeepingInGameData>();
            return Task.CompletedTask;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            _bookKeepingInGameData.PlayerLife.Subscribe(val =>
            {
                textPlayerLife.text = $"Player Life : {val}";
            }).AddTo(uiDisposables);

            _bookKeepingInGameData.Score.Subscribe(val =>
            {
                textScore.text = $"Score : {val}";
            }).AddTo(uiDisposables);

            _bookKeepingInGameData.HighScore.Subscribe(val =>
            {
                textHighScore.text = $"{val}";
            }).AddTo(uiDisposables);
        }
    }

}
