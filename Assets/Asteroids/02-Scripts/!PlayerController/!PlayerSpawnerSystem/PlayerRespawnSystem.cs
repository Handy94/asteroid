namespace Asteroid
{
    using HandyPackage;
    using UniRx;
    using UniRx.Async;

    public class PlayerRespawnSystem : IInitializable, System.IDisposable
    {
        private GameSignals _gameSignals;
        private BookKeepingInGameData _bookKeepingInGameData;
        private AsteroidGameSettings _asteroidGameSettings;
        private PlayerSpawnerSystem _playerSpawnerSystem;

        private CompositeDisposable disposables = new CompositeDisposable();
        private bool _isRespawning = false;

        public UniTask Initialize()
        {
            _gameSignals = DIResolver.GetObject<GameSignals>();
            _bookKeepingInGameData = DIResolver.GetObject<BookKeepingInGameData>();
            _asteroidGameSettings = DIResolver.GetObject<AsteroidGameSettings>();
            _playerSpawnerSystem = DIResolver.GetObject<PlayerSpawnerSystem>();

            _gameSignals.PlayerDespawnedSignal.Listen(HandlePlayerDespawned, PlayerDespawnedPrioritySignal.Priority.RESPAWN_PLAYER).AddTo(disposables);

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            disposables.Clear();
        }

        private bool HandlePlayerDespawned(PlayerShipComponent player)
        {
            if (_bookKeepingInGameData.PlayerLife.Value > 0)
            {
                RespawnPlayerWithDelay(_asteroidGameSettings.respawnDelayInSeconds);
            }
            return true;
        }

        private async UniTask RespawnPlayerWithDelay(float delayInSeconds)
        {
            if (_isRespawning) return;
            _isRespawning = true;

            float millis = delayInSeconds * 1000;
            await UniTask.Delay((int)millis);

            _isRespawning = false;
            await _playerSpawnerSystem.SpawnPlayer();
        }
    }

}
