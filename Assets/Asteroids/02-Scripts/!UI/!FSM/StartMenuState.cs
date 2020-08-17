namespace Asteroid
{
    using HandyPackage.FSM;
    using ParadoxNotion.Design;
    using UniRx.Async;
    using UnityEngine;

    [Category("@@-Handy Package/States")]
    [Name("Start Menu State")]
    public class StartMenuState : UIBaseState
    {
        public override UniTask OnPlayerAction(PlayerAction playerAction, object payload)
        {
            switch (playerAction)
            {
                case PlayerAction.START_GAME:
                    GoToInGameState();
                    break;
                case PlayerAction.QUIT_GAME:
                    QuitGame();
                    break;
            }
            return UniTask.CompletedTask;
        }

        private void GoToInGameState()
        {
            AppEventsManager.Publish_AppAction(AppAction.GOTO_IN_GAME_STATE);
        }

        private void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

}
