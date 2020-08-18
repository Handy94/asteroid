using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UniRx;

[Category("@@-Handy Package/UI")]
public class CheckForAppActionEventsCondition : ConditionTask
{
    public AppAction checkedAppAction;

    protected override string info => $"Is AppAction Event {checkedAppAction}";

    private CompositeDisposable disposables = new CompositeDisposable();

    protected override void OnEnable()
    {
        AppEventsManager.Evt_OnAppActionPublished.Listen(HandleAppActionAction).AddTo(disposables);
    }

    protected override void OnDisable()
    {
        disposables.Clear();
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
