namespace Asteroid
{
    using HandyPackage;

    public class PlayerInputInstaller : MonoInstaller
    {
        public RocketMovement rocketMovement;
        public PlayerWeapon playerWeapon;

        public override void InstallDependencies()
        {
            InstallMovementSystem();
            InstallWeaponSystem();
        }

        private void InstallMovementSystem()
        {
            Container.Install(new PlayerMovementInputSystem(rocketMovement));
        }

        private void InstallWeaponSystem()
        {
            Container.Install(new PlayerShootInputSystem(playerWeapon));
        }
    }
}
