namespace Asteroid
{
    using HandyPackage;
    using UniRx;
    using UniRx.Async;

    public class AddScoreOnDestroyingAsteroidSystem : IInitializable, System.IDisposable
    {
        private GameSignals _gameSignals;
        private ScoreSystem _scoreSystem;
        private AsteroidGameSettings _asteroidGameSettings;

        private CompositeDisposable disposables = new CompositeDisposable();

        public UniTask Initialize()
        {
            _gameSignals = DIResolver.GetObject<GameSignals>();
            _scoreSystem = DIResolver.GetObject<ScoreSystem>();
            _asteroidGameSettings = DIResolver.GetObject<AsteroidGameSettings>();

            _gameSignals.AsteroidDespawnedSignal.Listen(HandleAsteroidDespawned).AddTo(disposables);

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            disposables.Clear();
        }

        private void HandleAsteroidDespawned(AsteroidComponent asteroid, GameEntityTag gameEntityTag)
        {
            UnityEngine.Debug.Log(gameEntityTag);
            if (gameEntityTag != GameEntityTag.BULLET) return;
            _scoreSystem.AddScore(_asteroidGameSettings.scorePerAsteroid);
        }
    }

}
