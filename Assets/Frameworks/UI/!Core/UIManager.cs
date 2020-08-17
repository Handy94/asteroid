namespace HandyPackage
{
    using HandyPackage.FSM;
    using NodeCanvas.Framework;
    using NodeCanvas.StateMachines;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UniRx;
    using UniRx.Async;
    using UnityEngine;

    public class UIManager : MonoBehaviour, IInitializable, System.IDisposable
    {
        [SerializeField] private Blackboard blackboard;
        [SerializeField] private FSMOwner uiGraph;
        [SerializeField] private Camera uiCamera;
        [SerializeField] private bool startGraphOnInit = true;

        private List<UISceneBase> uiCache = new List<UISceneBase>();
        private List<UIBaseState> uiStateStack = new List<UIBaseState>();
        private List<IPlayerActionEvaluator> playerActionEvaluators = new List<IPlayerActionEvaluator>();

        private bool IsOnUIEnterRunning = false;

        private List<Func<UniTask>> UIEnterSequence = new List<Func<UniTask>>();

        public UIBaseState CurrentState
        {
            get
            {
                if (uiStateStack.Count > 0) return uiStateStack[uiStateStack.Count - 1];
                return null;
            }
        }

        public async UniTask Initialize()
        {
            if (blackboard == null) blackboard = GameObject.FindObjectOfType<Blackboard>();
            await PopulateUI();
            await InitUIScenes();
            AppEventsManager.Evt_OnAppActionPublished.Listen(HandleAppActionPublished);

            if (startGraphOnInit) StartUIBehaviour();

            await UniTask.CompletedTask;
        }

        public void Dispose()
        {
            AppEventsManager.Evt_OnAppActionPublished.Unlisten(HandleAppActionPublished);

            for (int i = 0, count = uiStateStack.Count; i < count; i++)
                uiStateStack[i].UnlistenPlayerAction();
        }

        void HandleAppActionPublished(AppAction appAction, object payload)
        {
            switch (appAction)
            {
                case AppAction.GOTO_PREVIOUS_STATE:
                    GoToPreviousState();
                    break;
            }
        }

        public void StartUIBehaviour()
        {
            if (uiGraph != null) uiGraph.StartBehaviour();
        }

        private async UniTask PopulateUI()
        {
            uiCache = blackboard.variables.Where(val => val.Value.value != null && val.Value.value is UISceneBase)
                                                .Select(val => val.Value.value)
                                                .Cast<UISceneBase>()
                                                .ToList();
            await UniTask.CompletedTask;
        }

        private async UniTask InitUIScenes()
        {
            var uiInitTasks = uiCache.Select(x => x.Init().AsUniTask());
            await UniTask.WhenAll(uiInitTasks);
        }

        private void DeactivateAllUIScenes()
        {
            foreach (var uiScene in uiCache)
            {
                uiScene?.Deactivate();
            }
        }

        public void UnlistenPlayerActionPreviousUI()
        {
            int stateCount = uiStateStack.Count;
            if (stateCount > 0)
            {
                var prevState = uiStateStack[stateCount - 1];
                if (prevState != null) prevState.UnlistenPlayerAction();
            }
        }

        public async UniTask HidePreviousUI(List<UISceneBase> newUIScenes = null)
        {
            int stateCount = uiStateStack.Count;
            if (stateCount > 0)
            {
                var prevState = uiStateStack[stateCount - 1];
                await prevState.HideMyUIScene(newUIScenes);
            }
        }

        public void DeactivatePreviousUI(List<UISceneBase> newUIScenes = null)
        {
            int stateCount = uiStateStack.Count;
            if (stateCount > 0)
            {
                var prevState = uiStateStack[stateCount - 1];
                prevState.DeactivateMyUIScene(newUIScenes);
            }
        }

        public void EmptyUIStack()
        {
            uiStateStack.Clear();
        }

        public void AddUIState(UIBaseState uiState)
        {
            if (uiState == null) return;
            uiStateStack.Add(uiState);
        }

        public void RemoveUIState(UIBaseState uiState)
        {
            if (uiState == null) return;
            uiStateStack.Remove(uiState);
        }

        public void GoToState(UIBaseState targetState)
        {
            NodeCanvas.StateMachines.FSM fsm = uiGraph.behaviour;

            UIBaseState currState = null;
            if (uiStateStack.Count > 1)
            {
                currState = uiStateStack[uiStateStack.Count - 1];
            }
            AppEventsManager.Publish_StateTransition(currState, targetState);
            //Debug.Log($"GOTO STATE {targetState.name} in [{fsm.name}]");
            fsm.TriggerState(targetState.name);
        }


        public void GoToState(string targetStateName)
        {
            NodeCanvas.StateMachines.FSM rootFSM = uiGraph.behaviour;
            NodeCanvas.StateMachines.FSM graph = rootFSM;

            string[] targetStates = targetStateName.Split('/');
            for (int i = 0; i < targetStates.Length; i++)
            {
                if (graph == null) return;
                if (i == targetStates.Length - 1)
                {
                    graph.TriggerState(targetStates[i]);
                }
                else
                {
                    var node = (NestedFSMState)graph.allNodes.Find(x => x is NestedFSMState && x.name.Equals(targetStates[i]));
                    if (!node.nestedFSM.isRunning)
                    {
                        graph.TriggerState(targetStates[i]);
                    }
                    graph = node.nestedFSM;
                }
            }
        }

        public void UpdateStateStackOnEnter(UIBaseState uiState)
        {
            if (uiState == null) return;
            if (this.uiGraph == null) return;

            int stateIndex = uiStateStack.IndexOf(uiState);
            if (stateIndex != -1)
            {
                int removeIndex = stateIndex + 1;
                if (removeIndex < uiStateStack.Count)
                {
                    for (int i = removeIndex, count = uiStateStack.Count; i < count; i++)
                        uiStateStack[i].UnlistenPlayerAction();

                    uiStateStack.RemoveRange(removeIndex, uiStateStack.Count - removeIndex);
                }
            }
            else
            {
                AddUIState(uiState);
            }
        }

        public bool GoToPreviousState()
        {
            if (!AppEventsManager.CanPublishAppAction) return true;
            if (uiStateStack.Count > 1)
            {
                var currState = uiStateStack[uiStateStack.Count - 1];
                var targetState = uiStateStack[uiStateStack.Count - 2];

                AppEventsManager.Publish_StateTransition(currState, targetState);
                uiGraph.TriggerState(targetState.name);
                return true;
            }
            return false;
        }

        public void QueueOnUIEnterSequence(Func<UniTask> uiEnterSequence)
        {
            UIEnterSequence.Add(uiEnterSequence);
            if (!IsOnUIEnterRunning)
            {
                IsOnUIEnterRunning = true;
                RunOnUIEnterSequence();
            }
        }

        private async UniTask RunOnUIEnterSequence()
        {
            List<Func<UniTask>> executedEnterSequence = new List<Func<UniTask>>();
            for (int i = 0; i < UIEnterSequence.Count;)
            {
                await UIEnterSequence[0]();
                UIEnterSequence.RemoveAt(0);
            }
            IsOnUIEnterRunning = false;

            if (UIEnterSequence.Count > 0)
            {
                IsOnUIEnterRunning = true;
                RunOnUIEnterSequence();
            }
        }

        public void SetShouldListenPlayerAction(bool value)
        {
            AppEventsManager.ShouldListenPlayerAction = value;
            var currState = CurrentState;
            if (currState != null)
            {
                if (value) currState.ListenPlayerAction();
                else currState.UnlistenPlayerAction();
            }
        }

        #region Player Action Evaluator
        public void RegisterPlayerActionEvaluator(IPlayerActionEvaluator evaluator)
        {
            if (evaluator == null) return;
            if (playerActionEvaluators.Contains(evaluator)) return;

            playerActionEvaluators.Add(evaluator);
        }

        public void RemovePlayerActionEvaluator(IPlayerActionEvaluator evaluator)
        {
            if (evaluator == null) return;
            if (!playerActionEvaluators.Contains(evaluator)) return;

            playerActionEvaluators.Remove(evaluator);
        }

        public bool EvaluatePlayerAction(PlayerAction playerAction, object payload)
        {
            int count = playerActionEvaluators.Count;
            for (int i = 0; i < count; i++)
            {
                if (!playerActionEvaluators[i].EvaluatePlayerAction(playerAction, payload)) return false;
            }
            return true;
        }
        #endregion

    }
}
