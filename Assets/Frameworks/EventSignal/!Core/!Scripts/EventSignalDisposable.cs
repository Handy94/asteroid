namespace HandyPackage
{
    using System;

    public class EventSignalDisposable : IDisposable
    {
        public Action disposeAction;

        public EventSignalDisposable(Action disposeAction)
        {
            this.disposeAction = disposeAction;
        }

        public void Dispose()
        {
            if (disposeAction != null) disposeAction.Invoke();
        }
    }
}