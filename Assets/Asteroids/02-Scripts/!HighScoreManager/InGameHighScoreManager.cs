namespace Asteroid
{
    using HandyPackage;
    using UniRx;
    using UniRx.Async;

    public class InGameHighScoreManager : IInitializable, System.IDisposable
    {
        private HighScoreManager _highScoreManager;
        private GameSignals _gameSignals;
        private BookKeepingInGameData _bookKeepingInGameData;

        private CompositeDisposable disposables = new CompositeDisposable();

        public UniTask Initialize()
        {
            _highScoreManager = DIResolver.GetObject<HighScoreManager>();
            _gameSignals = DIResolver.GetObject<GameSignals>();
            _bookKeepingInGameData = DIResolver.GetObject<BookKeepingInGameData>();

            _gameSignals.GameOverSignal.Listen(HandleGameOver).AddToDisposables(disposables);

            _bookKeepingInGameData.HighScore.Value = _highScoreManager.GetCurrentHighScore();

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            disposables.Clear();
        }

        private void HandleGameOver()
        {
            _highScoreManager.TryToSetNewHighScore(_bookKeepingInGameData.Score.Value);
            _bookKeepingInGameData.HighScore.Value = _highScoreManager.GetCurrentHighScore();
        }
    }

}
