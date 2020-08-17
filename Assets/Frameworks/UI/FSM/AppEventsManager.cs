using System;
using NodeCanvas.StateMachines;
using HandyPackage;
using HandyPackage.FSM;

public static class AppEventsManager
{
    public static bool CanPublishPlayerAction = true;
    public static bool CanPublishAppAction = true;
    public static bool ShouldListenPlayerAction = true;

    public static EventSignal<PlayerAction, object> Evt_OnPlayerActionPublished = new EventSignal<PlayerAction, object>();
    public static void Publish_PlayerAction(PlayerAction playerAction, object payload = null, bool force = false)
    {
        if (!force && !CanPublishPlayerAction) return;
        Evt_OnPlayerActionPublished?.Fire(playerAction, payload);
    }

    public static EventSignal<AppAction, object> Evt_OnAppActionPublished = new EventSignal<AppAction, object>();
    public static void Publish_AppAction(AppAction appAction, object payload = null, bool force = false)
    {
        if (!force && !CanPublishAppAction) return;
        Evt_OnAppActionPublished?.Fire(appAction, payload);
    }

    public static EventSignal<UIBaseState> Evt_OnStatePreEnter = new EventSignal<UIBaseState>();

    public static void Publish_StatePreEnter(UIBaseState state)
    {
        Evt_OnStatePreEnter?.Fire(state);
    }


    public static EventSignal<UIBaseState> Evt_OnStateEnter = new EventSignal<UIBaseState>();

    public static void Publish_StateEnter(UIBaseState state)
    {
        Evt_OnStateEnter?.Fire(state);
    }

    public static EventSignal<UIBaseState> Evt_OnStatePostEnter = new EventSignal<UIBaseState>();
    public static void Publish_StatePostEnter(UIBaseState state)
    {
        Evt_OnStatePostEnter?.Fire(state);
    }

    public static EventSignal<UIBaseState> Evt_OnStateExit = new EventSignal<UIBaseState>();
    public static void Publish_StateExit(UIBaseState state)
    {
        Evt_OnStateExit?.Fire(state);
    }

    public static EventSignal<FSMState, FSMState> Evt_OnStateTransition = new EventSignal<FSMState, FSMState>();
    public static void Publish_StateTransition(FSMState sourceState, FSMState targetState)
    {
        Evt_OnStateTransition?.Fire(sourceState, targetState);
    }
}
