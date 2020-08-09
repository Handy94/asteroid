namespace Asteroid
{
    using HandyPackage;
    using UnityEngine;

    [CreateAssetMenu(fileName = "AsteroidGameSettingInstaller", menuName = "Installers/AsteroidGameSettingInstaller")]
    public class AsteroidGameSettingInstaller : ScriptableObjectInstaller
    {
        public AsteroidGameSettings AsteroidGameSettings;

        public override void InstallDependencies()
        {
            Container.Install(AsteroidGameSettings);
        }
    }
}
