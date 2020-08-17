using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx;
using UniRx.Async;
using UnityEngine;

public abstract class UISceneBase : MonoBehaviour
{
    protected const int ANIMATOR_LAYER_INDEX = 0;
    protected const string ANIMATOR_STATE_NAME_ON = "UI-On";
    protected const string ANIMATOR_STATE_NAME_OFF = "UI-Off";

    protected const string ANIMATOR_PARAMETER_NAME_ON = "On";
    protected const string ANIMATOR_PARAMETER_NAME_OFF = "Off";

    protected static readonly int ANIMATOR_HASH_ON = Animator.StringToHash(ANIMATOR_PARAMETER_NAME_ON);
    protected static readonly int ANIMATOR_HASH_OFF = Animator.StringToHash(ANIMATOR_PARAMETER_NAME_OFF);

    protected CompositeDisposable uiDisposables = new CompositeDisposable();

    /*
    |-------------------......................................
    |   Virtual Methods
    |-------------------
    */
    protected abstract Task OnUISceneInit();
    protected virtual void OnActivated() { }
    protected virtual void OnDeactivated() { }
    protected virtual void OnShown() { }
    protected virtual void OnDestroyed() { }
    protected virtual async UniTask OnHidden() { await UniTask.CompletedTask; }

    private void OnDestroy()
    {
        uiDisposables.Dispose();
    }

    /*
    |-------------------
    |   Dependencies   
    |-------------------
    */

    public bool InitialActiveState;
    public List<GameObject> uiViewRoot;
    public List<Animator> uiAnimator;


    public virtual List<GameObject> UIViewRoot { get { return uiViewRoot; } }
    public virtual List<Animator> UIAnimator { get { return uiAnimator; } }

    protected bool activeStatus = false;
    public bool IsUIActive => activeStatus;

    protected bool shownStatus = false;
    public bool IsUIShown => shownStatus;

    /*
    |-------------------
    |   Public API   
    |-------------------
    */
    public virtual async Task Init()
    {
        // uiViewRoot?.ForEach(x => x.AddComponent<UICanvasSlicer>());
        UIViewRoot?.ForEach(x => x.SetActive(InitialActiveState));

        await OnUISceneInit();
    }

    public virtual void Activate()
    {
        if (!activeStatus)
        {
            activeStatus = true;
            UIViewRoot?.ForEach(x => x.SetActive(true));
            OnActivated();
        }
    }

    public virtual void Deactivate()
    {
        if (activeStatus)
        {
            activeStatus = false;
            OnDeactivated();
            UIViewRoot?.ForEach(x => x.SetActive(false));
            uiDisposables.Clear();
        }
    }

    public virtual async UniTask Show()
    {
        if (!shownStatus)
        {
            shownStatus = true;
            transform.SetAsLastSibling();

            if (UIAnimator != null && UIAnimator.Count > 0)
            {
                UIAnimator?.ForEach(x => x.SetTrigger(ANIMATOR_HASH_ON));
                await UniTask.DelayFrame(1);// wait one frame because sometimes animation state wont change until after frame ends
                await UniTask.WaitWhile(() =>
                {
                    bool isUIAnimating = false;
                    foreach (var uiAnim in UIAnimator)
                    {
                        var animState = uiAnim.GetCurrentAnimatorStateInfo(ANIMATOR_LAYER_INDEX);
                        bool isNameCorrect = animState.IsName(ANIMATOR_STATE_NAME_ON);
                        if (isNameCorrect && animState.normalizedTime < 1.0f)
                        {
                            isUIAnimating = true;
                        }
                    }
                    return isUIAnimating;
                });
            }
        }

        OnShown();


        await UniTask.CompletedTask;
    }

    public virtual async UniTask Hide(bool hideOnly = false)
    {
        if (shownStatus)
        {
            shownStatus = false;
            if (!hideOnly) uiDisposables.Clear();
            await OnHidden();

            if (UIAnimator != null && UIAnimator.Count > 0)
            {
                UIAnimator?.ForEach(x => x.SetTrigger(ANIMATOR_HASH_OFF));

                await UniTask.WaitWhile(() =>
                {
                    bool isUIAnimating = false;
                    foreach (var uiAnim in UIAnimator)
                    {
                        var animState = uiAnim.GetCurrentAnimatorStateInfo(ANIMATOR_LAYER_INDEX);
                        if (animState.IsName(ANIMATOR_STATE_NAME_OFF) && animState.normalizedTime < 1.0f)
                        {
                            isUIAnimating = true;
                        }
                    }
                    return isUIAnimating;
                });
            }
        }
        await UniTask.CompletedTask;
    }
}
