using NodeCanvas.Framework;
using ParadoxNotion.Design;

[Category("@@-Handy Package/UI")]
public class CheckForAppActionEventsCondition : ConditionTask
{
    public AppAction checkedAppAction;

    protected override string info => $"Is AppAction Event {checkedAppAction}";

    protected override void OnEnable()
    {
        AppEventsManager.Evt_OnAppActionPublished.Listen(HandleAppActionAction);
    }

    protected override void OnDisable()
    {
        AppEventsManager.Evt_OnAppActionPublished.Unlisten(HandleAppActionAction);
    }

    protected override bool OnCheck()
    {
        if (invert) return true;
        return false;
    }

    private void HandleAppActionAction(AppAction action, object payload)
    {
        if (invert)
        {
            if (action == checkedAppAction) return;
            YieldReturn(false);
        }
        else
        {
            if (action != checkedAppAction) return;
            YieldReturn(true);
        }
    }
}
