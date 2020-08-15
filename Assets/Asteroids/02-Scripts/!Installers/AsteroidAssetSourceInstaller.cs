namespace Asteroid
{
    using HandyPackage;
    using UnityEngine;

    [CreateAssetMenu(fileName = "AsteroidAssetSourceInstaller", menuName = "Installers/AsteroidAssetSourceInstaller")]
    public class AsteroidAssetSourceInstaller : ScriptableObjectInstaller
    {
        public AsteroidAssetSource AsteroidAssetSource;

        public override void InstallDependencies()
        {
            Container.Install(AsteroidAssetSource);
        }
    }
}
