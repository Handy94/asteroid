using Asteroid;
using HandyPackage;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataInstaller", menuName = "Installers/PlayerDataInstaller")]
public class PlayerDataInstaller : ScriptableObjectInstaller
{
    public override void InstallDependencies()
    {
        PlayerDataManager playerDataManager = Container.Install<PlayerDataManager>();
        PlayerData playerData = Container.Install<PlayerData>();
        Container.Install<PlayerDataGetter>(new PlayerDataGetter(playerData));
        Container.Install<PlayerDataMutator>(new PlayerDataMutator(playerData));
        Container.Install<PlayerDataKeyValueGetter>();
        Container.Install<PlayerDataLoader>(new PlayerDataLoader(playerDataManager, playerData));
        Container.Install<PlayerDataSubscribeManager>(new PlayerDataSubscribeManager(playerData));

        Container.Install<PlayerDataEventListener>(new PlayerDataEventListener(playerDataManager));

        Container.Install<PlayerDataSaver>(new PlayerDataSaver(playerDataManager, playerData));

    }
}
