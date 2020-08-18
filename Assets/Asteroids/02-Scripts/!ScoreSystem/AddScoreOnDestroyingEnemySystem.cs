namespace Asteroid
{
    using HandyPackage;
    using UniRx;
    using UniRx.Async;

    public class AddScoreOnDestroyingEnemySystem : IInitializable, System.IDisposable
    {
        private GameSignals _gameSignals;
        private ScoreSystem _scoreSystem;
        private IScoreDataSource<string> _asteroidScoreDataSource;

        private CompositeDisposable disposables = new CompositeDisposable();
        private bool _canAddScore = false;

        public UniTask Initialize()
        {
            _gameSignals = DIResolver.GetObject<GameSignals>();
            _scoreSystem = DIResolver.GetObject<ScoreSystem>();
            _asteroidScoreDataSource = DIResolver.GetObject<EnemyScoreDataSourceScriptableObject>();

            _gameSignals.GameStartSignal.Listen(HandleGameStart, GameStartPrioritySignal.PRIORITY_SETUP_ADD_SCORE_SYSTEM).AddTo(disposables);
            _gameSignals.GameOverSignal.Listen(HandleGameOver).AddTo(disposables);
            _gameSignals.EnemyDespawnedSignal.Listen(HandleEnemyDespawned).AddTo(disposables);

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            disposables.Clear();
        }

        private bool HandleGameStart()
        {
            _canAddScore = true;
            return true;
        }

        private void HandleGameOver()
        {
            _canAddScore = false;
        }

        private void HandleEnemyDespawned(EnemyComponent enemy, GameEntityTag gameEntityTag)
        {
            if (gameEntityTag != GameEntityTag.BULLET) return;
            if (!_canAddScore) return;

            int score = _asteroidScoreDataSource.GetScore(enemy.EnemyData.EnemyID);
            _scoreSystem.AddScore(score);
        }
    }

}
