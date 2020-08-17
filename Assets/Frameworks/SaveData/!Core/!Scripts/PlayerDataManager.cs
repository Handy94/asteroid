namespace HandyPackage
{
    using Asteroid;
    using UniRx.Async;

    public class PlayerDataManager : IInitializable
    {
        private ES3File file;
        private ES3File localFile;

        private bool _isInitialized = false;

        private PlayerDataLoader _playerDataLoader;

        public UniTask Initialize()
        {
            _playerDataLoader = DIResolver.GetObject<PlayerDataLoader>();

            file = new ES3File(true);

            _isInitialized = true;

            return UniTask.CompletedTask;
        }

        public void TrySave<T>(string key, T value, bool isLocal = false)
        {
            ES3File targetFile = null;
            if (!isLocal) targetFile = file;
            else targetFile = localFile;

            if (targetFile == null) return;

            try
            {
                targetFile.Save<T>(key, value);
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
        }

        // Load single data
        public T TryLoad<T>(string key, T defaultValue = default(T), bool isLocal = false)
        {
            ES3File targetFile = null;
            if (!isLocal) targetFile = file;
            else targetFile = localFile;

            if (targetFile == null) return defaultValue;
            if (!targetFile.KeyExists(key)) return defaultValue;

            try
            {
                return targetFile.Load<T>(key, defaultValue);
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogException(e);
                return defaultValue;
            }
        }

        public void Sync()
        {
            if (localFile != null) localFile.Sync();
            if (file == null) return;
            file.Sync();
        }

        public void CreateBackup()
        {
            if (file == null) return;
            ES3.CreateBackup(file.settings);
        }

        public void RestoreBackup()
        {
            ES3.RestoreBackup(file.settings);
        }

        public bool IsSaveKeyExist(string key, bool isLocal = false)
        {
            ES3File targetFile = null;
            if (!isLocal) targetFile = file;
            else targetFile = localFile;

            if (targetFile == null) return false;

            bool result = targetFile.KeyExists(key);

            return result;
        }
    }
}
