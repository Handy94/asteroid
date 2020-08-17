namespace Asteroid
{
    using HandyPackage;
    using UniRx;
    using UniRx.Async;

    public partial class PlayerDataSubscribeManager : IInitializable, System.IDisposable
    {
        private CompositeDisposable disposables = new CompositeDisposable();
        private PlayerData _playerData;
        public PlayerDataSubscribeManager()
        {
        }

        public PlayerDataSubscribeManager(PlayerData playerData)
        {
            _playerData = playerData;
        }

        public UniTask Initialize()
        {
            SubscribeForSave();
            SubscribeForSaveCustom();
            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            disposables.Clear();
        }

        private void SubscribeForSave()
        {
            SubscribeForSave_Asteroid();
        }

        private void SubscribeForSaveCustom()
        {
        }
    }
}