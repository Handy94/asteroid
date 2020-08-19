using HandyPackage;
using UniRx;
using UniRx.Async;

namespace Asteroid
{
    public abstract class BasePlayerInputSystem : IInitializable, System.IDisposable
    {
        protected GameSignals _gameSignals;
        protected InputSignal _inputSignal;

        protected CompositeDisposable disposables = new CompositeDisposable();
        protected CompositeDisposable inputDisposables = new CompositeDisposable();

        public virtual UniTask Initialize()
        {
            _gameSignals = DIResolver.GetObject<GameSignals>();
            _inputSignal = DIResolver.GetObject<InputSignal>();

            _gameSignals.PlayerSpawnedSignal.Listen(HandlePlayerSpawned).AddTo(disposables);
            _gameSignals.PlayerDespawnedSignal.Listen(HandlePlayerDespawned, PlayerDespawnedPrioritySignal.Priority.UNLISTEN_PLAYER_INPUT).AddTo(disposables);
            _gameSignals.PlayerDoHyperSpaceSignal.Listen(HandlePlayerDoHyperSpace).AddTo(disposables);
            _gameSignals.PlayerHyperSpaceFinishedSignal.Listen(HandlePlayerHyperSpaceFinished).AddTo(disposables);

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            UnlistenForPlayerInput();
            disposables.Clear();
        }

        protected virtual void HandlePlayerSpawned(PlayerShipComponent playerShipController)
        {
            ListenForPlayerInput();
        }

        protected virtual bool HandlePlayerDespawned(PlayerShipComponent playerShipController)
        {
            UnlistenForPlayerInput();
            return true;
        }

        protected virtual void HandlePlayerDoHyperSpace()
        {
            UnlistenForPlayerInput();
        }

        protected virtual void HandlePlayerHyperSpaceFinished()
        {
            ListenForPlayerInput();
        }

        public void ListenForPlayerInput()
        {
            _inputSignal.Listen(HandlePlayerInput).AddTo(inputDisposables);
        }

        public void UnlistenForPlayerInput()
        {
            inputDisposables.Clear();
        }

        protected abstract void HandlePlayerInput(PlayerShipInputKey inputKey);
    }

}
