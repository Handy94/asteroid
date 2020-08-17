using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Async;

namespace HandyPackage.FSM
{
    [Category("@@-Handy Package/UI")]
    [Name("UI Base State")]
    public class UIBaseState : FSMState
    {
        public List<BBParameter<UISceneBase>> activeScenes;

        protected UIManager uiManager;

        protected virtual bool placeOnUIStateStack => true;
        protected virtual bool triggerOnUIEnter => true;
        private CompositeDisposable playerActionDisposable = new CompositeDisposable();

        private bool isProcessingPlayerAction = false;

        /*
        |-------------------
        |   Overrides   
        |-------------------
        */
        protected override void OnInit()
        {
            base.OnInit();
            uiManager = DIResolver.GetObject<UIManager>();
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            AppEventsManager.Publish_StatePreEnter(this);
            AppEventsManager.Publish_StateEnter(this);
            uiManager.QueueOnUIEnterSequence(OnUIEnterSequence);
        }

        protected override async void OnExit()
        {
            if (!placeOnUIStateStack && triggerOnUIEnter)
            {
                await HideMyUIScene();
                DeactivateMyUIScene();
            }

            UnlistenPlayerAction();
            AppEventsManager.Publish_StateExit(this);
            base.OnExit();
        }

        // Public Functions
        public void ListenPlayerAction()
        {
            if (!AppEventsManager.ShouldListenPlayerAction) return;
            AppEventsManager.Evt_OnPlayerActionPublished.Listen(Handle_PublishedPlayerAction).AddToDisposables(playerActionDisposable);
        }

        public void UnlistenPlayerAction()
        {
            playerActionDisposable.Clear();
        }

        public void ActivateMyUIScene()
        {
            activeScenes?.ForEach(x => x.value?.Activate());
        }

        public void DeactivateMyUIScene(List<UISceneBase> excludedUIScenes = null)
        {
            activeScenes?.ForEach(x =>
            {
                if (excludedUIScenes != null)
                {
                    if (excludedUIScenes.Contains(x.value)) return;
                }
                x.value?.Deactivate();
            });
        }

        public virtual async UniTask ShowMyUIScene()
        {
            var showTasks = activeScenes?.Where(x => x != null && x.value != null).Select(x => x.value.Show()).ToList();
            if (showTasks != null && showTasks.Count > 0) await UniTask.WhenAll(showTasks);

            await UniTask.CompletedTask;
        }

        public async UniTask HideMyUIScene(List<UISceneBase> excludedUIScenes = null)
        {
            var uiScenes = activeScenes?.Where(x => x != null && x.value != null);
            if (excludedUIScenes != null)
            {
                uiScenes = uiScenes?.Where(x => !excludedUIScenes.Contains(x.value));
            }

            var hideTasks = uiScenes.Select(x => x.value.Hide()).ToList();
            if (hideTasks != null && hideTasks.Count > 0) await UniTask.WhenAll(hideTasks);

            await UniTask.CompletedTask;
        }

        /*
        |-------------------
        |   Logic   
        |-------------------
        */
        private async UniTask OnUIEnterSequence()
        {
            try
            {
                if (triggerOnUIEnter)
                {
                    List<UISceneBase> uiScenes = activeScenes.Select(x => x.value).ToList();
                    uiManager.UnlistenPlayerActionPreviousUI();
                    await uiManager.HidePreviousUI(uiScenes);
                    uiManager.DeactivatePreviousUI(uiScenes);

                    if (placeOnUIStateStack)
                        uiManager.UpdateStateStackOnEnter(this);
                    ActivateMyUIScene();
                    await ShowMyUIScene();
                    ListenPlayerAction();
                }
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError($"{ex.GetType()} : {ex.Message}\n{ex.StackTrace}");
            }

            AppEventsManager.Publish_StatePostEnter(this);
            await UniTask.CompletedTask;
        }

        public virtual async UniTask OnPlayerAction(PlayerAction playerAction, object payload) { await UniTask.CompletedTask; }

        protected async void Handle_PublishedPlayerAction(PlayerAction playerAction, object payload)
        {
            await Internal_OnPlayerAction(playerAction, payload);
        }

        private async UniTask Internal_OnPlayerAction(PlayerAction playerAction, object payload)
        {
            if (isProcessingPlayerAction) return;
            if (!uiManager.EvaluatePlayerAction(playerAction, payload)) return;

            isProcessingPlayerAction = true;
            await OnPlayerAction(playerAction, payload);
            isProcessingPlayerAction = false;
        }

        protected void GoToPreviousState()
        {
            AppEventsManager.Publish_AppAction(AppAction.GOTO_PREVIOUS_STATE);
        }
    }
}
