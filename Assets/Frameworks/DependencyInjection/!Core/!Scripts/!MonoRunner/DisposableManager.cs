namespace HandyPackage
{
    using System;
    using System.Collections.Generic;

    public class DisposableManager
    {
        private List<IDisposable> _disposables = new List<IDisposable>();
        private List<ILateDisposable> _lateDisposables = new List<ILateDisposable>();

        public void RegisterDisposable(IDisposable disposable)
        {
            if (_disposables.Contains(disposable)) return;
            _disposables.Add(disposable);
        }

        public void RegisterLateDisposable(ILateDisposable lateDisposable)
        {
            if (_lateDisposables.Contains(lateDisposable)) return;
            _lateDisposables.Add(lateDisposable);
        }

        public void Dispose()
        {
            int count = _disposables.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                _disposables[i].Dispose();
            }
        }

        public void LateDispose()
        {
            int count = _lateDisposables.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                _lateDisposables[i].LateDispose();
            }
        }
    }
}
