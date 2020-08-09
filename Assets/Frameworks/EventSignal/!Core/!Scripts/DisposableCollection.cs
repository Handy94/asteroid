namespace HandyPackage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class DisposableCollection : ICollection<IDisposable>, IDisposable
    {
        public List<IDisposable> _disposables;

        public DisposableCollection()
        {
            _disposables = new List<IDisposable>();
        }

        public DisposableCollection(List<IDisposable> disposables)
        {
            _disposables = disposables;
        }

        public DisposableCollection(params IDisposable[] disposables)
        {
            _disposables = new List<IDisposable>(disposables);
        }

        public int Count => _disposables.Count;

        public bool IsReadOnly => false;

        public void Add(IDisposable item)
        {
            _disposables.Add(item);
        }

        public void Clear()
        {
            Dispose();
        }

        public void ClearItem()
        {
            _disposables.Clear();
        }

        public bool Contains(IDisposable item)
        {
            return _disposables.Contains(item);
        }

        public void CopyTo(IDisposable[] array, int arrayIndex)
        {
            var disArray = new List<IDisposable>();
            foreach (var item in _disposables)
            {
                if (item != null) disArray.Add(item);
            }

            Array.Copy(disArray.ToArray(), 0, array, arrayIndex, array.Length - arrayIndex);
        }

        public void Dispose()
        {
            foreach (var d in _disposables)
            {
                if (d != null) d.Dispose();
            }
            ClearItem();
        }

        public IEnumerator<IDisposable> GetEnumerator()
        {
            var res = new List<IDisposable>();

            foreach (var d in _disposables)
            {
                if (d != null) res.Add(d);
            }

            return res.GetEnumerator();
        }

        public bool Remove(IDisposable item)
        {
            if (_disposables.Contains(item))
            {
                _disposables.Remove(item);
                return true;
            }
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}