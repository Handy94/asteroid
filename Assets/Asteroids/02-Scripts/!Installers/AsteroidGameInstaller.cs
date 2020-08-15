namespace Asteroid
{
    using HandyPackage;

    public class AsteroidGameInstaller : MonoInstaller
    {
        public override void InstallDependencies()
        {
            InstallFrameworkSystems();

            InstallMainDependencies();

            InstallPlayerSystem();

            InstallBulletSystem();
            InstallAsteroidSystem();

            InstallGameSystem();

            InstallScoreSystem();
        }

        private void InstallFrameworkSystems()
        {
            Container.Install<MultiplePrefabMemoryPool>();
        }

        private void InstallMainDependencies()
        {
            Container.Install<GameSignals>();
            Container.Install<BookKeepingInGameData>();
        }

        private void InstallPlayerSystem()
        {
            PlayerInputInstaller.Install(Container);
            Container.Install<PlayerSpawnerSystem>();
            Container.Install<PlayerRespawnSystem>();
        }

        private void InstallBulletSystem()
        {
            Container.Install<BulletSpawnerSystem>();
        }

        private void InstallAsteroidSystem()
        {
            Container.Install<AsteroidSpawnerSystem>();
            Container.Install<AsteroidSplitSystem>();
        }

        private void InstallGameSystem()
        {
            Container.Install<PositionWrapperSystem>();
            Container.Install<DamageOnCollideTriggerSystem>();
            Container.Install<DecreasePlayerLifeOnDeadSystem>();
        }

        private void InstallScoreSystem()
        {
            Container.Install<ScoreSystem>();
            Container.Install<AddScoreOnDestroyingAsteroidSystem>();
        }
    }
}
