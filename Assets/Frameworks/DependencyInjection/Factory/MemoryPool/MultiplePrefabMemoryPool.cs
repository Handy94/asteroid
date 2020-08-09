namespace HandyPackage
{
    using UniRx.Async;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class MultiplePrefabMemoryPoolBase
    {
        protected Dictionary<string, IMemoryPool> memoryPools = new Dictionary<string, IMemoryPool>();
        protected Dictionary<string, Transform> poolParents = new Dictionary<string, Transform>();
        protected const string GAME_OBJECT_CLONE_POSTFIX = "(Clone)";

        protected string GetPrefabID(GameObject poolablePrefab)
        {
            string prefabID = poolablePrefab.name;
            if (string.IsNullOrEmpty(prefabID))
            {
                prefabID = prefabID.Replace(GAME_OBJECT_CLONE_POSTFIX, string.Empty);
            }
            return prefabID;
        }

        protected bool IsPrefabPoolExist(GameObject prefab)
        {
            string prefabID = GetPrefabID(prefab);
            return memoryPools.ContainsKey(prefabID);
        }

        protected TValue SetupSpawnedObject<TValue>(TValue spawnedObject, string prefabID) where TValue : Component
        {
            if (spawnedObject.GetComponent<PoolObjectID>() == null)
            {
                spawnedObject.gameObject.AddComponent<PoolObjectID>().poolID = prefabID;
            }
            spawnedObject.transform.SetParent(poolParents[prefabID], false);
            return spawnedObject;
        }

        public void DespawnObject(GameObject go)
        {
            DespawnObject(go.GetComponent<PoolableGameObject>());
        }

        public void DespawnObject<TValue>(TValue poolableObject) where TValue : Component, IPoolableBase
        {
            if (poolableObject == null) return;

            string prefabID = poolableObject.GetComponent<PoolObjectID>().poolID;
            if (!memoryPools.ContainsKey(prefabID)) return;

            IDespawnableMemoryPool<TValue> pool = memoryPools[prefabID] as IDespawnableMemoryPool<TValue>;
            if (pool != null)
            {
                pool.Despawn(poolableObject);
            }
            else
            {
                Debug.LogError($"MultiPrefabPool Not Exists : {prefabID}");
            }
        }
    }

    public class MultiplePrefabMemoryPool : MultiplePrefabMemoryPoolBase
    {
        public void AddNewPool<TValue>(GameObject prefab, int initialPoolSize = 1, PoolExpandMethod poolExpandMethod = PoolExpandMethod.ExpandByOne)
            where TValue : Component, IPoolable
        {
            string prefabID = GetPrefabID(prefab.gameObject);
            if (!memoryPools.ContainsKey(prefabID))
            {
                IMemoryPool mp = new GameObjectMemoryPool<TValue>()
                    .WithInitialPoolSize(initialPoolSize)
                    .WithExpandMethod(poolExpandMethod)
                    .FromFactory(new PrefabFactory<TValue>(prefab.gameObject));
                memoryPools.Add(prefabID, mp);
                poolParents.Add(prefabID, new GameObject($"Pool - {prefabID}").transform);
            }
        }

        public async UniTask<GameObject> SpawnObject(GameObject prefab)
        {
            return await SpawnObject(prefab, Vector3.zero);
        }

        public async UniTask<GameObject> SpawnObject(GameObject prefab, Vector3 pos)
        {
            if (prefab == null) return null;

            string prefabID = GetPrefabID(prefab.gameObject);
            if (!memoryPools.ContainsKey(prefabID))
            {
                AddNewPool<PoolableGameObject>(prefab);
            }

            var pool = memoryPools[prefabID] as GameObjectMemoryPool<PoolableGameObject>;
            PoolableGameObject spawnedObject = await pool.Spawn();
            if (spawnedObject != null)
            {
                spawnedObject.transform.position = pos;
                return SetupSpawnedObject(spawnedObject, prefabID).gameObject;
            }
            else
            {
                Debug.LogError($"MultiPrefabPool Not Exists : {prefabID}");
                return null;
            }
        }

        public async UniTask<TValue> SpawnObject<TValue>(GameObject prefab)
            where TValue : Component, IPoolable
        {
            if (prefab == null) return null;

            string prefabID = GetPrefabID(prefab.gameObject);
            if (!memoryPools.ContainsKey(prefabID))
            {
                AddNewPool<TValue>(prefab);
            }

            var pool = memoryPools[prefabID] as GameObjectMemoryPool<TValue>;
            TValue spawnedObject = await pool.Spawn();
            if (spawnedObject != null)
            {
                return SetupSpawnedObject(spawnedObject, prefabID);
            }
            else
            {
                Debug.LogError($"MultiPrefabPool Not Exists : {prefabID}");
                return null;
            }
        }
    }

    public class MultiplePrefabMemoryPool<TParam1> : MultiplePrefabMemoryPoolBase
    {
        public void AddNewPool<TValue>(GameObject prefab, int initialPoolSize = 1, PoolExpandMethod poolExpandMethod = PoolExpandMethod.ExpandByOne)
            where TValue : Component, IPoolable<TParam1>
        {
            string prefabID = GetPrefabID(prefab.gameObject);
            if (!memoryPools.ContainsKey(prefabID))
            {
                IMemoryPool mp = new GameObjectMemoryPool<TParam1, TValue>()
                    .WithInitialPoolSize(initialPoolSize)
                    .WithExpandMethod(poolExpandMethod)
                    .FromFactory(new PrefabFactory<TValue>(prefab.gameObject));
                memoryPools.Add(prefabID, mp);
                poolParents.Add(prefabID, new GameObject($"Pool - {prefabID}").transform);
            }
        }

        public async UniTask<TValue> SpawnObject<TValue>(GameObject prefab, TParam1 param1)
            where TValue : Component, IPoolable<TParam1>
        {
            if (prefab == null) return null;

            string prefabID = GetPrefabID(prefab.gameObject);
            if (!memoryPools.ContainsKey(prefabID))
            {
                AddNewPool<TValue>(prefab);
            }

            var pool = memoryPools[prefabID] as GameObjectMemoryPool<TParam1, TValue>;
            TValue spawnedObject = await pool.Spawn(param1);
            if (spawnedObject != null)
            {
                return SetupSpawnedObject(spawnedObject, prefabID);
            }
            else
            {
                Debug.LogError($"MultiPrefabPool Not Exists : {prefabID}");
                return null;
            }
        }
    }

    public class MultiplePrefabMemoryPool<TParam1, TParam2> : MultiplePrefabMemoryPoolBase
    {
        public void AddNewPool<TValue>(GameObject prefab, int initialPoolSize = 1, PoolExpandMethod poolExpandMethod = PoolExpandMethod.ExpandByOne)
            where TValue : Component, IPoolable<TParam1, TParam2>
        {
            string prefabID = GetPrefabID(prefab.gameObject);
            if (!memoryPools.ContainsKey(prefabID))
            {
                IMemoryPool mp = new GameObjectMemoryPool<TParam1, TParam2, TValue>()
                    .WithInitialPoolSize(initialPoolSize)
                    .WithExpandMethod(poolExpandMethod)
                    .FromFactory(new PrefabFactory<TValue>(prefab.gameObject));
                memoryPools.Add(prefabID, mp);
                poolParents.Add(prefabID, new GameObject($"Pool - {prefabID}").transform);
            }
        }

        public async UniTask<TValue> SpawnObject<TValue>(GameObject prefab, TParam1 param1, TParam2 param2)
            where TValue : Component, IPoolable<TParam1, TParam2>
        {
            if (prefab == null) return null;

            string prefabID = GetPrefabID(prefab.gameObject);
            if (!memoryPools.ContainsKey(prefabID))
            {
                AddNewPool<TValue>(prefab);
            }

            var pool = memoryPools[prefabID] as GameObjectMemoryPool<TParam1, TParam2, TValue>;
            TValue spawnedObject = await pool.Spawn(param1, param2);
            if (spawnedObject != null)
            {
                return SetupSpawnedObject(spawnedObject, prefabID);
            }
            else
            {
                Debug.LogError($"MultiPrefabPool Not Exists : {prefabID}");
                return null;
            }
        }
    }

    public class MultiplePrefabMemoryPool<TParam1, TParam2, TParam3> : MultiplePrefabMemoryPoolBase
    {
        public void AddNewPool<TValue>(GameObject prefab, int initialPoolSize = 1, PoolExpandMethod poolExpandMethod = PoolExpandMethod.ExpandByOne)
            where TValue : Component, IPoolable<TParam1, TParam2, TParam3>
        {
            string prefabID = GetPrefabID(prefab.gameObject);
            if (!memoryPools.ContainsKey(prefabID))
            {
                IMemoryPool mp = new GameObjectMemoryPool<TParam1, TParam2, TParam3, TValue>()
                    .WithInitialPoolSize(initialPoolSize)
                    .WithExpandMethod(poolExpandMethod)
                    .FromFactory(new PrefabFactory<TValue>(prefab.gameObject));
                memoryPools.Add(prefabID, mp);
                poolParents.Add(prefabID, new GameObject($"Pool - {prefabID}").transform);
            }
        }

        public async UniTask<TValue> SpawnObject<TValue>(GameObject prefab, TParam1 param1, TParam2 param2, TParam3 param3)
            where TValue : Component, IPoolable<TParam1, TParam2, TParam3>
        {
            if (prefab == null) return null;

            string prefabID = GetPrefabID(prefab.gameObject);
            if (!memoryPools.ContainsKey(prefabID))
            {
                AddNewPool<TValue>(prefab);
            }

            var pool = memoryPools[prefabID] as GameObjectMemoryPool<TParam1, TParam2, TParam3, TValue>;
            TValue spawnedObject = await pool.Spawn(param1, param2, param3);
            if (spawnedObject != null)
            {
                return SetupSpawnedObject(spawnedObject, prefabID);
            }
            else
            {
                Debug.LogError($"MultiPrefabPool Not Exists : {prefabID}");
                return null;
            }
        }
    }

    public class MultiplePrefabMemoryPool<TParam1, TParam2, TParam3, TParam4> : MultiplePrefabMemoryPoolBase
    {
        public void AddNewPool<TValue>(GameObject prefab, int initialPoolSize = 1, PoolExpandMethod poolExpandMethod = PoolExpandMethod.ExpandByOne)
            where TValue : Component, IPoolable<TParam1, TParam2, TParam3, TParam4>
        {
            string prefabID = GetPrefabID(prefab.gameObject);
            if (!memoryPools.ContainsKey(prefabID))
            {
                IMemoryPool mp = new GameObjectMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>()
                    .WithInitialPoolSize(initialPoolSize)
                    .WithExpandMethod(poolExpandMethod)
                    .FromFactory(new PrefabFactory<TValue>(prefab.gameObject));
                memoryPools.Add(prefabID, mp);
                poolParents.Add(prefabID, new GameObject($"Pool - {prefabID}").transform);
            }
        }

        public async UniTask<TValue> SpawnObject<TValue>(GameObject prefab, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
            where TValue : Component, IPoolable<TParam1, TParam2, TParam3, TParam4>
        {
            if (prefab == null) return null;

            string prefabID = GetPrefabID(prefab.gameObject);
            if (!memoryPools.ContainsKey(prefabID))
            {
                AddNewPool<TValue>(prefab);
            }

            var pool = memoryPools[prefabID] as GameObjectMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>;
            TValue spawnedObject = await pool.Spawn(param1, param2, param3, param4);
            if (spawnedObject != null)
            {
                return SetupSpawnedObject(spawnedObject, prefabID);
            }
            else
            {
                Debug.LogError($"MultiPrefabPool Not Exists : {prefabID}");
                return null;
            }
        }
    }

    public class MultiplePrefabMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5> : MultiplePrefabMemoryPoolBase
    {
        public void AddNewPool<TValue>(GameObject prefab, int initialPoolSize = 1, PoolExpandMethod poolExpandMethod = PoolExpandMethod.ExpandByOne)
            where TValue : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5>
        {
            string prefabID = GetPrefabID(prefab.gameObject);
            if (!memoryPools.ContainsKey(prefabID))
            {
                IMemoryPool mp = new GameObjectMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>()
                    .WithInitialPoolSize(initialPoolSize)
                    .WithExpandMethod(poolExpandMethod)
                    .FromFactory(new PrefabFactory<TValue>(prefab.gameObject));
                memoryPools.Add(prefabID, mp);
                poolParents.Add(prefabID, new GameObject($"Pool - {prefabID}").transform);
            }
        }

        public async UniTask<TValue> SpawnObject<TValue>(GameObject prefab, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
            where TValue : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5>
        {
            if (prefab == null) return null;

            string prefabID = GetPrefabID(prefab.gameObject);
            if (!memoryPools.ContainsKey(prefabID))
            {
                AddNewPool<TValue>(prefab);
            }

            var pool = memoryPools[prefabID] as GameObjectMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>;
            TValue spawnedObject = await pool.Spawn(param1, param2, param3, param4, param5);
            if (spawnedObject != null)
            {
                return SetupSpawnedObject(spawnedObject, prefabID);
            }
            else
            {
                Debug.LogError($"MultiPrefabPool Not Exists : {prefabID}");
                return null;
            }
        }
    }

    public class MultiplePrefabMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> : MultiplePrefabMemoryPoolBase
    {
        public void AddNewPool<TValue>(GameObject prefab, int initialPoolSize = 1, PoolExpandMethod poolExpandMethod = PoolExpandMethod.ExpandByOne)
            where TValue : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>
        {
            string prefabID = GetPrefabID(prefab.gameObject);
            if (!memoryPools.ContainsKey(prefabID))
            {
                IMemoryPool mp = new GameObjectMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>()
                    .WithInitialPoolSize(initialPoolSize)
                    .WithExpandMethod(poolExpandMethod)
                    .FromFactory(new PrefabFactory<TValue>(prefab.gameObject));
                memoryPools.Add(prefabID, mp);
                poolParents.Add(prefabID, new GameObject($"Pool - {prefabID}").transform);
            }
        }

        public async UniTask<TValue> SpawnObject<TValue>(GameObject prefab, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6)
            where TValue : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>
        {
            if (prefab == null) return null;

            string prefabID = GetPrefabID(prefab.gameObject);
            if (!memoryPools.ContainsKey(prefabID))
            {
                AddNewPool<TValue>(prefab);
            }

            var pool = memoryPools[prefabID] as GameObjectMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>;
            TValue spawnedObject = await pool.Spawn(param1, param2, param3, param4, param5, param6);
            if (spawnedObject != null)
            {
                return SetupSpawnedObject(spawnedObject, prefabID);
            }
            else
            {
                Debug.LogError($"MultiPrefabPool Not Exists : {prefabID}");
                return null;
            }
        }
    }

    public class MultiplePrefabMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7> : MultiplePrefabMemoryPoolBase
    {
        public void AddNewPool<TValue>(GameObject prefab, int initialPoolSize = 1, PoolExpandMethod poolExpandMethod = PoolExpandMethod.ExpandByOne)
            where TValue : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7>
        {
            string prefabID = GetPrefabID(prefab.gameObject);
            if (!memoryPools.ContainsKey(prefabID))
            {
                IMemoryPool mp = new GameObjectMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue>()
                    .WithInitialPoolSize(initialPoolSize)
                    .WithExpandMethod(poolExpandMethod)
                    .FromFactory(new PrefabFactory<TValue>(prefab.gameObject));
                memoryPools.Add(prefabID, mp);
                poolParents.Add(prefabID, new GameObject($"Pool - {prefabID}").transform);
            }
        }

        public async UniTask<TValue> SpawnObject<TValue>(GameObject prefab, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7)
            where TValue : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7>
        {
            if (prefab == null) return null;

            string prefabID = GetPrefabID(prefab.gameObject);
            if (!memoryPools.ContainsKey(prefabID))
            {
                AddNewPool<TValue>(prefab);
            }

            var pool = memoryPools[prefabID] as GameObjectMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue>;
            TValue spawnedObject = await pool.Spawn(param1, param2, param3, param4, param5, param6, param7);
            if (spawnedObject != null)
            {
                return SetupSpawnedObject(spawnedObject, prefabID);
            }
            else
            {
                Debug.LogError($"MultiPrefabPool Not Exists : {prefabID}");
                return null;
            }
        }
    }

    public class MultiplePrefabMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8> : MultiplePrefabMemoryPoolBase
    {
        public void AddNewPool<TValue>(GameObject prefab, int initialPoolSize = 1, PoolExpandMethod poolExpandMethod = PoolExpandMethod.ExpandByOne)
            where TValue : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8>
        {
            string prefabID = GetPrefabID(prefab.gameObject);
            if (!memoryPools.ContainsKey(prefabID))
            {
                IMemoryPool mp = new GameObjectMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TValue>()
                    .WithInitialPoolSize(initialPoolSize)
                    .WithExpandMethod(poolExpandMethod)
                    .FromFactory(new PrefabFactory<TValue>(prefab.gameObject));
                memoryPools.Add(prefabID, mp);
                poolParents.Add(prefabID, new GameObject($"Pool - {prefabID}").transform);
            }
        }

        public async UniTask<TValue> SpawnObject<TValue>(GameObject prefab, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8)
            where TValue : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8>
        {
            if (prefab == null) return null;

            string prefabID = GetPrefabID(prefab.gameObject);
            if (!memoryPools.ContainsKey(prefabID))
            {
                AddNewPool<TValue>(prefab);
            }

            var pool = memoryPools[prefabID] as GameObjectMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TValue>;
            TValue spawnedObject = await pool.Spawn(param1, param2, param3, param4, param5, param6, param7, param8);
            if (spawnedObject != null)
            {
                return SetupSpawnedObject(spawnedObject, prefabID);
            }
            else
            {
                Debug.LogError($"MultiPrefabPool Not Exists : {prefabID}");
                return null;
            }
        }
    }
}
