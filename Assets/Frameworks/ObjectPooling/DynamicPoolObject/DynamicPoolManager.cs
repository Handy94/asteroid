using System.Collections.Generic;
using UnityEngine;

public class DynamicPoolManager
{
    private Dictionary<string, DynamicPoolObject.Pool> memoryPools = new Dictionary<string, DynamicPoolObject.Pool>();

    void AddNewPool(DynamicPoolObject poolObject, int initSize = 1, PoolExpandMethods poolExpandMethods = PoolExpandMethods.OneAtATime)
    {
        if (poolObject == null) return;

        if (!memoryPools.ContainsKey(poolObject.poolObjectId))
        {
            MemoryPoolSettings settings = new MemoryPoolSettings()
            {
                InitialSize = initSize,
                ExpandMethod = poolExpandMethods
            };

            var factory = new DynamicPoolObject.Factory(poolObject.gameObject, this);
            var pool = new DynamicPoolObject.Pool(settings, factory);

            memoryPools.Add(poolObject.poolObjectId, pool);
        }
    }

    public void AddNewPool(GameObject go, int initSize = 1, PoolExpandMethods poolExpandMethods = PoolExpandMethods.OneAtATime)
    {
        if (go != null)
        {
            DynamicPoolObject poolObject = go.GetComponent<DynamicPoolObject>();
            AddNewPool(poolObject, initSize, poolExpandMethods);
        }
    }

    public GameObject SpawnObject(GameObject go)
    {
        return SpawnObject(go, Vector3.zero);
    }

    public GameObject SpawnObject(GameObject go, Vector3 spawnPosition)
    {
        DynamicPoolObject poolObject = go.GetComponent<DynamicPoolObject>();

        if (poolObject != null)
        {
            if (!memoryPools.ContainsKey(poolObject.poolObjectId))
            {
                AddNewPool(go);
            }

            DynamicPoolObject tempGo = memoryPools[poolObject.poolObjectId].Spawn();
            tempGo.transform.position = spawnPosition;
            tempGo.transform.SetParent(new GameObject($"Pool - {poolObject.poolObjectId}").transform);
            tempGo.poolObjectId = poolObject.poolObjectId;

            return tempGo.gameObject;
        }
        else
        {
            Debug.LogError("DynamicPoolManager: Spawned object must have \"DynamicPoolObject\" component");
        }
        return null;
    }

    public void DespawnObject(DynamicPoolObject poolObject)
    {
        if (!memoryPools.ContainsKey(poolObject.poolObjectId))
        {
            Debug.Log("Pool Object Id \"" + poolObject.poolObjectId + "\" Not Found");
            return;
        }
        memoryPools[poolObject.poolObjectId].Despawn(poolObject);
        //		Debug.Log ("Pool Object Despawned");
    }
}