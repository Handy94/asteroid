namespace Asteroid
{
    using HandyPackage;
    using UnityEngine;

    [CreateAssetMenu(fileName = "AsteroidAssetSourceInstaller", menuName = "Installers/AsteroidAssetSourceInstaller")]
    public class AsteroidAssetSourceInstaller : ScriptableObjectInstaller
    {
        public AsteroidGameAssetSource AsteroidAssetSource;
        public AsteroidScoreDataSourceScriptableObject AsteroidScoreDataSource;

        public override void InstallDependencies()
        {
            Container.Install(AsteroidAssetSource);
            Container.Install(AsteroidScoreDataSource);
        }
    }
}
