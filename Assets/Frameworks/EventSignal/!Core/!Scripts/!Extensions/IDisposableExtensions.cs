namespace HandyPackage
{
    using System;
    using System.Collections.Generic;

    public static partial class IDisposableExtensions
    {
        public static void AddToDisposables(this IDisposable disposable, ICollection<IDisposable> disposables)
        {
            if (disposable == null) return;
            if (disposables == null) return;

            disposables.Add(disposable);
        }
    }
}