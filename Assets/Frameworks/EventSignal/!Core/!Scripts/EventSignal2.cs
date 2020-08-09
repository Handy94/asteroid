namespace HandyPackage
{
    using System;
    using System.Collections.Generic;

    public interface IEventSignal<T1, T2>
    {
        void Fire(T1 arg1, T2 arg2);

        IDisposable Listen(Action<T1, T2> listener);
        void Unlisten(Action<T1, T2> listener);
    }

    public class EventSignal<T1, T2> : IEventSignal<T1, T2>
    {
        protected List<Action<T1, T2>> listeners;
        public event Action<T1, T2> eventListener;

        public EventSignal()
        {
            listeners = new List<Action<T1, T2>>();
        }

        public void Fire(T1 arg1, T2 arg2)
        {
            if (eventListener != null)
            {
                eventListener.Invoke(arg1, arg2);
            }
        }

        public IDisposable Listen(Action<T1, T2> listener)
        {
            if (listener == null)
            {
                return null;
            }
            if (this.listeners.Contains(listener))
            {
                return null;
            }

            listeners.Add(listener);
            eventListener += listener;
            return new EventSignalDisposable(
                delegate
                {
                    Unlisten(listener);
                }
            );
        }

        public void Unlisten(Action<T1, T2> listener)
        {
            if (listener == null)
            {
                return;
            }
            if (!this.listeners.Contains(listener))
            {
                return;
            }
            listeners.Remove(listener);
            eventListener -= listener;
        }
    }

}