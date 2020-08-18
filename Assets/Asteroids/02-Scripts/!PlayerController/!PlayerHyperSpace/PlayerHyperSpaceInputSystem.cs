using HandyPackage;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace Asteroid
{
    public class PlayerHyperSpaceInputSystem : IInitializable, System.IDisposable, IPlayerInputListener
    {
        private GameSignals _gameSignals;

        private IShipHyperSpace _shipHyperSpace;
        private CompositeDisposable disposables = new CompositeDisposable();
        private CompositeDisposable inputDisposables = new CompositeDisposable();

        public UniTask Initialize()
        {
            _gameSignals = DIResolver.GetObject<GameSignals>();

            _gameSignals.PlayerSpawnedSignal.Listen(HandlePlayerSpawned).AddTo(disposables);
            _gameSignals.PlayerDespawnedSignal.Listen(HandlePlayerDespawned, PlayerDespawnedPrioritySignal.Priority.UNLISTEN_PLAYER_MOVE_INPUT).AddTo(disposables);
            _gameSignals.PlayerDoHyperSpaceSignal.Listen(HandlePlayerDoHyperSpace).AddTo(disposables);
            _gameSignals.PlayerHyperSpaceFinishedSignal.Listen(HandlePlayerHyperSpaceFinished).AddTo(disposables);

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            UnlistenForPlayerInput();
            disposables.Clear();
        }

        private void HandlePlayerSpawned(PlayerShipComponent playerShipController)
        {
            _shipHyperSpace = playerShipController.HyperSpace;
            ListenForPlayerInput();
        }

        private bool HandlePlayerDespawned(PlayerShipComponent playerShipController)
        {
            UnlistenForPlayerInput();
            _shipHyperSpace = null;
            return true;
        }

        private void HandlePlayerDoHyperSpace()
        {
            UnlistenForPlayerInput();
        }

        private void HandlePlayerHyperSpaceFinished()
        {
            ListenForPlayerInput();
        }

        public void ListenForPlayerInput()
        {
            Observable.EveryUpdate().Subscribe(x =>
            {
                CheckInput();
            }).AddTo(inputDisposables);
        }

        public void UnlistenForPlayerInput()
        {
            inputDisposables.Clear();
        }

        private void CheckInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DoHyperSpace();
            }
        }

        private void DoHyperSpace()
        {
            _gameSignals.PlayerDoHyperSpaceSignal.Fire();
            _shipHyperSpace.DoHyperSpace(() =>
            {
                _gameSignals.PlayerHyperSpaceFinishedSignal.Fire();
            });
        }
    }
}
