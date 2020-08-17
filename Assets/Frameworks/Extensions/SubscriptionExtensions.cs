namespace HandyPackage
{
    using System.Linq;
    using UniRx;

    public static class SubscriptionExtensions
    {
        public static void SubscribeToPersistence<T>(this ReactiveProperty<T> property, string saveKey, CompositeDisposable disposables, bool isLocal = false)
        {
            property.Subscribe(value => DIResolver.GetObject<PlayerDataManager>().TrySave(saveKey, value, isLocal)
            ).AddTo(disposables);
        }

        public static void SubscribeToPersistence<T>(this ReactiveCollection<T> collection, string saveKey, CompositeDisposable disposables, bool isLocal = false)
        {
            collection.ObserveCountChanged().Subscribe(value =>
                DIResolver.GetObject<PlayerDataManager>().TrySave(saveKey, collection.ToList(), isLocal)
            ).AddTo(disposables);

            collection.ObserveReplace().Subscribe(value =>
                DIResolver.GetObject<PlayerDataManager>().TrySave(saveKey, collection.ToList(), isLocal)
            ).AddTo(disposables);
        }

        public static void SubscribeToPersistence<T1, T2>(this ReactiveDictionary<T1, T2> dictionary, string saveKey, CompositeDisposable disposables, bool isLocal = false)
        {
            dictionary.ObserveCountChanged().Subscribe(value =>
                DIResolver.GetObject<PlayerDataManager>().TrySave(saveKey, dictionary.ToDictionary(x => x.Key, x => x.Value), isLocal)
            ).AddTo(disposables);

            dictionary.ObserveReplace().Subscribe(value =>
                DIResolver.GetObject<PlayerDataManager>().TrySave(saveKey, dictionary.ToDictionary(x => x.Key, x => x.Value), isLocal)
            ).AddTo(disposables);
        }
    }

}
