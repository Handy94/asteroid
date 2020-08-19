namespace Asteroid
{
    public class PlayerMovementInputSystem : BasePlayerInputSystem
    {
        private IShipMovement _shipMovement;

        protected override void HandlePlayerSpawned(PlayerShipComponent playerShipController)
        {
            base.HandlePlayerSpawned(playerShipController);
            _shipMovement = playerShipController.ShipMovement;
        }

        protected override bool HandlePlayerDespawned(PlayerShipComponent playerShipController)
        {
            base.HandlePlayerDespawned(playerShipController);
            _shipMovement = null;
            return true;
        }

        protected override void HandlePlayerInput(PlayerShipInputKey inputKey)
        {
            switch (inputKey)
            {
                case PlayerShipInputKey.MOVE_FORWARD:
                    _shipMovement?.MoveForward();
                    break;
                case PlayerShipInputKey.STOP_MOVE_FORWARD:
                    _shipMovement?.StopMoveForward();
                    break;
                case PlayerShipInputKey.ROTATE_COUNTERCLOCKWISE:
                    _shipMovement?.RotateCounterClockwise();
                    break;
                case PlayerShipInputKey.ROTATE_CLOCKWISE:
                    _shipMovement?.RotateClockwise();
                    break;
                case PlayerShipInputKey.STOP_ROTATE:
                    _shipMovement?.StopRotate();
                    break;
                default:
                    break;
            }
        }
    }
}
