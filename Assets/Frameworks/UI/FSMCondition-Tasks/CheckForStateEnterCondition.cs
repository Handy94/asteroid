using HandyPackage.FSM;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UniRx;

[Category("@@-Handy Package/UI")]
public class CheckForStateEnterCondition : ConditionTask
{
    public string enteredState;
    private CompositeDisposable disposables = new CompositeDisposable();

    protected override string info => $"Is Entered State is: <b>{enteredState}</b>";


    protected override void OnEnable()
    {
        AppEventsManager.Evt_OnStatePreEnter.Listen(Handle_StatePreEnter).AddTo(disposables);
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

    private void Handle_StatePreEnter(UIBaseState enteredState)
    {
        // if (this.enteredState.Equals("Tutorial General"))
        //     UnityEngine.Debug.Log("State: " + enteredState.name);
        bool isTrue = enteredState.name.Equals(this.enteredState);

        if (invert)
            isTrue = !isTrue;

        YieldReturn(isTrue);
    }
}
