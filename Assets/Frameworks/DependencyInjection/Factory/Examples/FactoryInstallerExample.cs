using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HandyPackage;
using UniRx;
namespace HandyPackage.Examples
{
    public class FactoryInstallerExample : MonoInstaller
    {
        public GameObject testgo;

        public override void InstallDependencies()
        {
            testgo = FindObjectOfType<TestPoolableGameObject>().gameObject;

            Container.InstallInterfaceAndSelf(new PrefabFactory<TestPoolableGameObject>(testgo));
            Container.InstallInterfaceAndSelf<GameObjectMemoryPool<TestPoolableGameObject>>()
                     .FromFactory<PrefabFactory<TestPoolableGameObject>>().WithInitialPoolSize(10);

            Container.Install<Testclass>();
        }
    }
}