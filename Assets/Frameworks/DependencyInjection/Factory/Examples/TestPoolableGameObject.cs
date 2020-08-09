using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HandyPackage;
namespace HandyPackage.Examples
{
    public class TestPoolableGameObject : MonoBehaviour, IPoolable
    {
        public void OnCreated()
        {
        }

        public void OnSpawned()
        {
            Debug.Log("Spawned");
        }

        public void OnDespawned()
        {
            Debug.Log("Despaned");
        }

        public void OnDestroyed()
        {
        }
    }
}
