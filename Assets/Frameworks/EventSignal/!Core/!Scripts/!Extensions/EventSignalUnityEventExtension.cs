namespace HandyPackage
{
    using System;
    using UnityEngine.Events;

    public static class EventSignalUnityEventExtension
    {
        public static IDisposable SetupListener(this UnityEvent evt, UnityAction call)
        {
            evt.AddListener(call);
            return new EventSignalDisposable(
                delegate ()
                {
                    evt.RemoveListener(call);
                }
            );
        }

        public static IDisposable SetupListener<T1>(this UnityEvent<T1> evt, UnityAction<T1> call)
        {
            evt.AddListener(call);
            return new EventSignalDisposable(
                delegate ()
                {
                    evt.RemoveListener(call);
                }
            );
        }

        public static IDisposable SetupListener<T1, T2>(this UnityEvent<T1, T2> evt, UnityAction<T1, T2> call)
        {
            evt.AddListener(call);
            return new EventSignalDisposable(
                delegate ()
                {
                    evt.RemoveListener(call);
                }
            );
        }

        public static IDisposable SetupListener<T1, T2, T3>(this UnityEvent<T1, T2, T3> evt, UnityAction<T1, T2, T3> call)
        {
            evt.AddListener(call);
            return new EventSignalDisposable(
                delegate ()
                {
                    evt.RemoveListener(call);
                }
            );
        }

        public static IDisposable SetupListener<T1, T2, T3, T4>(this UnityEvent<T1, T2, T3, T4> evt, UnityAction<T1, T2, T3, T4> call)
        {
            evt.AddListener(call);
            return new EventSignalDisposable(
                delegate ()
                {
                    evt.RemoveListener(call);
                }
            );
        }
    }
}