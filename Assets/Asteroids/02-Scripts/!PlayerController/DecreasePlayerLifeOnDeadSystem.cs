namespace Asteroid
{
    using HandyPackage;
    using UniRx;
    using UniRx.Async;

    public class DecreasePlayerLifeOnDeadSystem : IInitializable, System.IDisposable
    {
        private GameSignals _gameSignals;
        private BookKeepingInGameData _bookKeepingInGameData;

        private CompositeDisposable disposables = new CompositeDisposable();

        public UniTask Initialize()
        {
            _gameSignals = DIResolver.GetObject<GameSignals>();
            _bookKeepingInGameData = DIResolver.GetObject<BookKeepingInGameData>();

            _gameSignals.PlayerDespawnedSignal.Listen(HandlePlayerDespawned, PlayerDespawnedPrioritySignal.Priority.DECREASE_PLAYER_LIFE).AddTo(disposables);

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
                _bookKeepingInGameData.PlayerLife.Value--;
            }
            return true;
        }
    }
}
