namespace Asteroid
{
    public class PlayerShootInputSystem : BasePlayerInputSystem
    {
        private IWeapon _playerWeapon;

        protected override void HandlePlayerSpawned(PlayerShipComponent playerShipController)
        {
            base.HandlePlayerSpawned(playerShipController);
            _playerWeapon = playerShipController.PlayerWeapon;
        }

        protected override bool HandlePlayerDespawned(PlayerShipComponent playerShipController)
        {
            base.HandlePlayerDespawned(playerShipController);
            _playerWeapon = null;
            return true;
        }

        protected override void HandlePlayerInput(PlayerShipInputKey inputKey)
        {
            switch (inputKey)
            {
                case PlayerShipInputKey.SHOOT:
                    _playerWeapon?.Shoot();
                    break;
                case PlayerShipInputKey.STOP_SHOOT:
                    _playerWeapon?.StopShoot();
                    break;
            }
        }
    }

}
