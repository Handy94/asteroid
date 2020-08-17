using HandyPackage.FSM;
using ParadoxNotion.Design;
using UniRx.Async;

namespace Asteroid
{
    [Category("@@-Handy Package/States")]
    [Name("Game Over State")]
    public class GameOverState : UIBaseState
    {
        public override UniTask OnPlayerAction(PlayerAction playerAction, object payload)
        {
            switch (playerAction)
            {
                case PlayerAction.START_GAME:
                    GoToInGameState();
                    break;
                case PlayerAction.BACK_TO_MAIN_MENU:
                    BackToMainMenu();
                    break;
            }
            return UniTask.CompletedTask;
        }

        private void GoToInGameState()
        {
            AppEventsManager.Publish_AppAction(AppAction.GOTO_IN_GAME_STATE);
        }

        private void BackToMainMenu()
        {
            AppEventsManager.Publish_AppAction(AppAction.GOTO_START_STATE);
        }
    }
}
