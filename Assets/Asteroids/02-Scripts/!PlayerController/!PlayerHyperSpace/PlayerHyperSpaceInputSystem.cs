namespace Asteroid
{
    public class PlayerHyperSpaceInputSystem : BasePlayerInputSystem
    {
        private IShipHyperSpace _shipHyperSpace;

        protected override void HandlePlayerSpawned(PlayerShipComponent playerShipController)
        {
            base.HandlePlayerSpawned(playerShipController);
            _shipHyperSpace = playerShipController.HyperSpace;
        }

        protected override bool HandlePlayerDespawned(PlayerShipComponent playerShipController)
        {
            base.HandlePlayerDespawned(playerShipController);
            _shipHyperSpace = null;
            return true;
        }

        protected override void HandlePlayerInput(PlayerShipInputKey inputKey)
        {
            switch (inputKey)
            {
                case PlayerShipInputKey.HYPER_SPACE:
                    DoHyperSpace();
                    break;
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
