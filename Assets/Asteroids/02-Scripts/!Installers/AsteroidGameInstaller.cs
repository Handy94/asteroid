namespace Asteroid
{
    using HandyPackage;

    public class AsteroidGameInstaller : MonoInstaller
    {
        public override void InstallDependencies()
        {
            Container.Install<PositionWrapperSystem>();
        }
    }
}
