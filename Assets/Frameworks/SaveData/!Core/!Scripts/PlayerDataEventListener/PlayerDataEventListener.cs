namespace HandyPackage
{
    using Asteroid;
    using UniRx.Async;

    public class PlayerDataEventListener : IInitializable, System.IDisposable, IMonoApplicationFocus
    {
        protected virtual bool ShouldSyncOnApplicationPause => true;
        protected virtual bool ShouldSyncOnApplicationQuit => true;
        protected virtual bool ShouldBackupOnApplicationPause => true;
        protected virtual bool ShouldBackupOnApplicationQuit => true;

        private MonoApplicationManager _monoApplicationManager;

        protected PlayerDataManager _playerDataManager;
        protected PlayerDataMutator _playerDataMutator;
        protected PlayerData _playerData;
        private DisposableCollection disposables = new DisposableCollection();

        public PlayerDataEventListener(PlayerDataManager pdm)
        {
            _playerDataManager = pdm;
        }

        public UniTask Initialize()
        {
            _playerDataMutator = DIResolver.GetObject<PlayerDataMutator>();
            _playerData = DIResolver.GetObject<PlayerData>();
            _monoApplicationManager = DIResolver.GetObject<MonoApplicationManager>();

            _monoApplicationManager.RegisterOnApplicationFocus(this);

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            disposables.Clear();

            _monoApplicationManager.RemoveOnApplicationFocus(this);

            if (ShouldSyncOnApplicationQuit) _playerDataManager.Sync();
            if (ShouldBackupOnApplicationQuit) _playerDataManager.CreateBackup();
        }

        public void OnApplicationFocus(bool focusStatus)
        {
            if (!focusStatus)
            {
                if (ShouldSyncOnApplicationPause) _playerDataManager.Sync();
                if (ShouldBackupOnApplicationPause) _playerDataManager.CreateBackup();
            }
        }

        private void Sync()
        {
            _playerDataManager.Sync();
        }

        private void Backup()
        {
            _playerDataManager.CreateBackup();
        }

        private void SyncAndBackup()
        {
            Sync();
            Backup();
        }
    }

}
