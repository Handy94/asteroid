namespace Asteroid
{
    using HandyPackage;

    public class AsteroidGameInstaller : MonoInstaller
    {
        public override void InstallDependencies()
        {
            InstallFrameworkSystems();

            InstallMainDependencies();
            InstallGameSystem();

            InstallPlayerSystem();

            InstallBulletSystem();
            InstallAsteroidSystem();

            InstallStageWaveSystem();
            InstallScoreSystem();
            InstallHighScoreSystem();

            InstallEnemySystem();
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
            Container.Install<GameStarterSystem>();
            Container.Install<PositionWrapperSystem>();
            Container.Install<DamageOnCollideTriggerSystem>();
            Container.Install<DecreasePlayerLifeOnDeadSystem>();
            Container.Install<GameOverSystem>();
        }

        private void InstallStageWaveSystem()
        {
            Container.Install<StageWaveSystem>();
        }

        private void InstallScoreSystem()
        {
            Container.Install<ScoreSystem>();
            Container.Install<AddScoreOnDestroyingAsteroidSystem>();
            Container.Install<AddPlayerLifeAfterReachScoreMultiplierSystem>();
        }

        private void InstallHighScoreSystem()
        {
            Container.Install<InGameHighScoreManager>();
        }

        private void InstallEnemySystem()
        {
            Container.Install<EnemySaucerSpawnerSystem>();
        }
    }
}
