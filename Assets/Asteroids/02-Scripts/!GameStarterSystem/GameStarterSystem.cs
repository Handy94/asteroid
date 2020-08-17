namespace Asteroid
{
    using HandyPackage;
    using UniRx;
    using UniRx.Async;

    public class GameStarterSystem : IInitializable, System.IDisposable
    {
        private AsteroidGameSettings _asteroidGameSettings;
        private GameSignals _gameSignal;
        private BookKeepingInGameData _bookKeepingInGameData;

        private CompositeDisposable disposables = new CompositeDisposable();

        public UniTask Initialize()
        {
            _asteroidGameSettings = DIResolver.GetObject<AsteroidGameSettings>();
            _gameSignal = DIResolver.GetObject<GameSignals>();
            _bookKeepingInGameData = DIResolver.GetObject<BookKeepingInGameData>();

            _gameSignal.GameOverSignal.Listen(() =>
            {
                CompositeDisposable tempDisposable = new CompositeDisposable();
                Observable.EveryUpdate().Subscribe(x =>
                {
                    if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Space))
                    {
                        StartGame();
                        tempDisposable.Dispose();
                    }
                }).AddTo(tempDisposable);
            }).AddTo(disposables);

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            disposables.Clear();
        }

        public void StartGame()
        {
            InitData();
            _gameSignal.GameStartSignal.Fire();
        }

        private void InitData()
        {
            _bookKeepingInGameData.PlayerLife.Value = _asteroidGameSettings.InitialPlayerLife;
            _bookKeepingInGameData.Score.Value = _asteroidGameSettings.InitialPlayerScore;
            _bookKeepingInGameData.CurrentStage.Value = _asteroidGameSettings.InitialPlayerStage;
        }
    }

}
