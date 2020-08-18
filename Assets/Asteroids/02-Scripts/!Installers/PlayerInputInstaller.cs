namespace Asteroid
{
    using HandyPackage;

    public class PlayerInputInstaller : Installer<PlayerInputInstaller>
    {
        public override void InstallDependencies()
        {
            InstallMovementSystem();
            InstallWeaponSystem();
            InstallOtherSystem();
        }

        private void InstallMovementSystem()
        {
            Container.Install<PlayerMovementInputSystem>();
        }

        private void InstallWeaponSystem()
        {
            Container.Install<PlayerShootInputSystem>();
        }

        private void InstallOtherSystem()
        {
            Container.Install<PlayerHyperSpaceInputSystem>();
        }
    }
}
