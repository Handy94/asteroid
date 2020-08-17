namespace Asteroid
{
    using HandyPackage;

    public class HighScoreManagerInstaller : MonoInstaller
    {
        public override void InstallDependencies()
        {
            Container.Install<HighScoreManager>();
        }
    }
}
