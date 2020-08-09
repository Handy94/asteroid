namespace HandyPackage
{
    using System;
    using System.Collections;
    using UnityEngine;

    public class CoroutineManager : MonoBehaviour
    {
        public Coroutine StartDelayCoroutine(Action action, float delay)
        {
            return StartCoroutine(DelayedCoroutine(action, delay));
        }

        IEnumerator DelayedCoroutine(Action action, float delay)
        {
            if (delay > 0) yield return new WaitForSeconds(delay);
            if (action != null) action.Invoke();
        }
    }
}
