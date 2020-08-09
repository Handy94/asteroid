namespace Asteroid
{
    using HandyPackage;

    public class PlayerInputInstaller : MonoInstaller
    {
        public PlayerRocketMovementInputSystem playerRocketMovementInputSystem;

        public override void InstallDependencies()
        {
            Container.Install<InputManager>();

            InstallMovementSystem();
        }

        private void InstallMovementSystem()
        {
            Container.Install<PlayerRocketMovementInputSystem>(playerRocketMovementInputSystem);
        }
    }
}
