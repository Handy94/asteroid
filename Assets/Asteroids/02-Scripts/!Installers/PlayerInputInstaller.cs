namespace Asteroid
{
    using HandyPackage;

    public class PlayerInputInstaller : Installer<PlayerInputInstaller>
    {
        public override void InstallDependencies()
        {
            Container.Install<InputSignal>();
            InstallMovementSystem();
            InstallWeaponSystem();
            InstallOtherSystem();
            Container.Install<StandalonePlayerInputListener>();
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
