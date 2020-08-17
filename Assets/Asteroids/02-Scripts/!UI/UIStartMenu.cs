using HandyPackage;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Asteroid
{
    public class UIStartMenu : UISceneBase
    {
        [SerializeField] private Button buttonStart;
        [SerializeField] private Button buttonQuit;

        protected override Task OnUISceneInit()
        {
            return Task.CompletedTask;
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            buttonStart?.onClick.SetupListener(HandleButtonStartClicked).AddToDisposables(uiDisposables);
            buttonQuit?.onClick.SetupListener(HandleButtonQuitClicked).AddToDisposables(uiDisposables);
        }

        private void HandleButtonStartClicked()
        {
            AppEventsManager.Publish_PlayerAction(PlayerAction.START_GAME);
        }

        private void HandleButtonQuitClicked()
        {
            AppEventsManager.Publish_PlayerAction(PlayerAction.QUIT_GAME);
        }
    }

}
