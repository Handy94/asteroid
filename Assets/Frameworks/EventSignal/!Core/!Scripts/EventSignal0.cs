namespace HandyPackage
{
    using System;
    using System.Collections.Generic;

    public interface IEventSignal
    {
        void Fire();

        IDisposable Listen(Action listener);
        void Unlisten(Action listener);
    }

    public class EventSignal : IEventSignal
    {
        protected List<Action> listeners;
        public event Action eventListener;

        public EventSignal()
        {
            this.listeners = new List<Action>();
        }

        public void Fire()
        {
            if (eventListener != null)
            {
                eventListener.Invoke();
            }
        }

        public IDisposable Listen(Action listener)
        {
            if (listener == null) return null;
            if (this.listeners.Contains(listener)) return null;

            listeners.Add(listener);
            eventListener += listener;
            return new EventSignalDisposable(
                delegate
                {
                    Unlisten(listener);
                }
            );
        }

        public void Unlisten(Action listener)
        {
            if (listener == null)
                return;
            if (!this.listeners.Contains(listener))
                return;
            listeners.Remove(listener);
            eventListener -= listener;
        }
    }
}