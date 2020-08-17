using HandyPackage;
using System.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine.UI;

namespace Asteroid
{
    public class UIGameOver : UISceneBase
    {
        public TextMeshProUGUI textScore;
        public TextMeshProUGUI textHighScore;
        public Button buttonRetry;
        public Button buttonBackToMainMenu;

        private BookKeepingInGameData _bookKeepingInGameData;

        protected override Task OnUISceneInit()
        {
            _bookKeepingInGameData = DIResolver.GetObject<BookKeepingInGameData>();
            return Task.CompletedTask;
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            _bookKeepingInGameData.Score.Subscribe(score =>
            {
                SetScoreText(score);
            }).AddTo(uiDisposables);

            _bookKeepingInGameData.HighScore.Subscribe(highScore =>
            {
                SetHighScoreText(highScore);
            }).AddTo(uiDisposables);

            buttonRetry.onClick.SetupListener(HandleButtonRetryClicked).AddTo(uiDisposables);
            buttonBackToMainMenu.onClick.SetupListener(HandleButtonBackToMainMenuClicked).AddTo(uiDisposables);
        }

        private void SetScoreText(int score)
        {
            textScore.text = $"Score : {score}";
        }

        private void SetHighScoreText(int highScore)
        {
            textHighScore.text = $"High Score : {highScore}";
        }

        private void HandleButtonRetryClicked()
        {
            AppEventsManager.Publish_PlayerAction(PlayerAction.START_GAME);
        }

        private void HandleButtonBackToMainMenuClicked()
        {
            AppEventsManager.Publish_PlayerAction(PlayerAction.BACK_TO_MAIN_MENU);
        }
    }

}
