using HandyPackage;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace Asteroid
{
    public class PlayerMovementInputSystem : IInitializable, System.IDisposable, IPlayerInputListener
    {
        private const string INPUT_AXIS_VERTICAL = "Vertical";
        private const string INPUT_AXIS_HORIZONTAL = "Horizontal";

        private GameSignals _gameSignals;

        private IShipMovement _shipMovement;
        private bool _canInput;
        private CompositeDisposable disposables = new CompositeDisposable();
        private CompositeDisposable inputDisposables = new CompositeDisposable();

        public UniTask Initialize()
        {
            _gameSignals = DIResolver.GetObject<GameSignals>();

            _gameSignals.PlayerSpawnedSignal.Listen(HandlePlayerSpawned).AddTo(disposables);
            _gameSignals.PlayerDespawnedSignal.Listen(HandlePlayerDespawned, PlayerDespawnedPrioritySignal.Priority.UNLISTEN_PLAYER_MOVE_INPUT).AddTo(disposables);

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            UnlistenForPlayerInput();
            disposables.Clear();
        }

        private void HandlePlayerSpawned(PlayerShipComponent playerShipController)
        {
            _shipMovement = playerShipController.ShipMovement;
            ListenForPlayerInput();
        }

        private bool HandlePlayerDespawned(PlayerShipComponent playerShipController)
        {
            UnlistenForPlayerInput();
            _shipMovement = null;
            return true;
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
            float vAxis = Mathf.Clamp(Input.GetAxisRaw(INPUT_AXIS_VERTICAL), 0, 1);
            float hAxis = Input.GetAxisRaw(INPUT_AXIS_HORIZONTAL);

            if (vAxis > 0) _shipMovement?.MoveForward();
            else _shipMovement?.StopMoveForward();

            if (hAxis < 0) _shipMovement?.RotateCounterClockwise();
            else if (hAxis > 0) _shipMovement?.RotateClockwise();
            else _shipMovement.StopRotate();
        }
    }
}
