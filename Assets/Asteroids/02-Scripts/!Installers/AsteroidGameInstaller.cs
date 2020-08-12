namespace Asteroid
{
    using HandyPackage;

    public class AsteroidGameInstaller : MonoInstaller
    {
        public override void InstallDependencies()
        {
            InstallFrameworkSystems();

            Container.Install<GameSignals>();
            Container.Install<PositionWrapperSystem>();
            Container.Install<BulletSpawnerSystem>();
            Container.Install<AsteroidSpawnerSystem>();

            Container.Install<DamageOnCollideTriggerSystem>();
        }

        private void InstallFrameworkSystems()
        {
            Container.Install<MultiplePrefabMemoryPool>();
        }
    }
}
