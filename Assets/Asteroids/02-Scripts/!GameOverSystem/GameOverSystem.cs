namespace Asteroid
{
    using HandyPackage;
    using UniRx;
    using UniRx.Async;

    public class GameOverSystem : IInitializable, System.IDisposable
    {
        private BookKeepingInGameData _bookKeepingInGameData;
        private GameSignals _gameSignal;

        private CompositeDisposable disposables = new CompositeDisposable();

        public UniTask Initialize()
        {
            _bookKeepingInGameData = DIResolver.GetObject<BookKeepingInGameData>();
            _gameSignal = DIResolver.GetObject<GameSignals>();

            _bookKeepingInGameData.PlayerLife.Subscribe(life =>
            {
                if (life == 0) GoToGameOverState();
            }).AddTo(disposables);

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            disposables.Clear();
        }

        private void GoToGameOverState()
        {
            _gameSignal.GameOverSignal.Fire();
            AppEventsManager.Publish_AppAction(AppAction.GOTO_GAME_OVER_STATE);
        }
    }
}
