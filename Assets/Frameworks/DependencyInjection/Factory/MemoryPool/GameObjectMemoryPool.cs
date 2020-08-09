using UnityEngine;
using UniRx.Async;

namespace HandyPackage
{
    public class GameObjectMemoryPool<TValue> : MemoryPool<TValue>
        where TValue : Component, IPoolable
    {
        protected override void OnCreated(TValue item)
        {
            item.OnCreated();
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
            item.gameObject.SetActive(false);
        }

        protected override void OnDestroyed(TValue item)
        {
            item.OnDestroyed();
            GameObject.Destroy(item.gameObject);
        }

        public override async UniTask<TValue> Spawn()
        {
            TValue item = await base.Spawn();
            item.gameObject.SetActive(true);
            return item;
        }
    }

    public class GameObjectMemoryPool<TParam1, TValue> : MemoryPool<TParam1, TValue>
        where TValue : Component, IPoolable<TParam1>
    {
        protected override void OnCreated(TValue item)
        {
            item.OnCreated();
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
            item.gameObject.SetActive(false);
        }

        protected override void OnDestroyed(TValue item)
        {
            item.OnDestroyed();
            GameObject.Destroy(item.gameObject);
        }

        public override async UniTask<TValue> Spawn(TParam1 param1)
        {
            TValue item = await base.Spawn(param1);
            item.gameObject.SetActive(true);
            return item;
        }
    }

    public class GameObjectMemoryPool<TParam1, TParam2, TValue> : MemoryPool<TParam1, TParam2, TValue>
        where TValue : Component, IPoolable<TParam1, TParam2>
    {
        protected override void OnCreated(TValue item)
        {
            item.OnCreated();
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
            item.gameObject.SetActive(false);
        }

        protected override void OnDestroyed(TValue item)
        {
            item.OnDestroyed();
            GameObject.Destroy(item.gameObject);
        }

        public override async UniTask<TValue> Spawn(TParam1 param1, TParam2 param2)
        {
            TValue item = await GetInternal();
            item.OnSpawned(param1, param2);
            item.gameObject.SetActive(true);
            return item;
        }
    }

    public class GameObjectMemoryPool<TParam1, TParam2, TParam3, TValue> : MemoryPool<TParam1, TParam2, TParam3, TValue>
        where TValue : Component, IPoolable<TParam1, TParam2, TParam3>
    {
        protected override void OnCreated(TValue item)
        {
            item.OnCreated();
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
            item.gameObject.SetActive(false);
        }

        protected override void OnDestroyed(TValue item)
        {
            item.OnDestroyed();
            GameObject.Destroy(item.gameObject);
        }

        public override async UniTask<TValue> Spawn(TParam1 param1, TParam2 param2, TParam3 param3)
        {
            TValue item = await GetInternal();
            item.OnSpawned(param1, param2, param3);
            item.gameObject.SetActive(true);
            return item;
        }
    }

    public class GameObjectMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue> : MemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>
        where TValue : Component, IPoolable<TParam1, TParam2, TParam3, TParam4>
    {
        protected override void OnCreated(TValue item)
        {
            item.OnCreated();
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
            item.gameObject.SetActive(false);
        }

        protected override void OnDestroyed(TValue item)
        {
            item.OnDestroyed();
            GameObject.Destroy(item.gameObject);
        }

        public override async UniTask<TValue> Spawn(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            TValue item = await GetInternal();
            item.OnSpawned(param1, param2, param3, param4);
            item.gameObject.SetActive(true);
            return item;
        }
    }

    public class GameObjectMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>
        where TValue : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5>
    {
        protected override void OnCreated(TValue item)
        {
            item.OnCreated();
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
            item.gameObject.SetActive(false);
        }

        protected override void OnDestroyed(TValue item)
        {
            item.OnDestroyed();
            GameObject.Destroy(item.gameObject);
        }

        public override async UniTask<TValue> Spawn(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            TValue item = await GetInternal();
            item.OnSpawned(param1, param2, param3, param4, param5);
            item.gameObject.SetActive(true);
            return item;
        }
    }

    public class GameObjectMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>
        where TValue : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>
    {
        protected override void OnCreated(TValue item)
        {
            item.OnCreated();
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
            item.gameObject.SetActive(false);
        }

        protected override void OnDestroyed(TValue item)
        {
            item.OnDestroyed();
            GameObject.Destroy(item.gameObject);
        }

        public override async UniTask<TValue> Spawn(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6)
        {
            TValue item = await GetInternal();
            item.OnSpawned(param1, param2, param3, param4, param5, param6);
            item.gameObject.SetActive(true);
            return item;
        }
    }

    public class GameObjectMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue> : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue>
        where TValue : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7>
    {
        protected override void OnCreated(TValue item)
        {
            item.OnCreated();
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
            item.gameObject.SetActive(false);
        }

        protected override void OnDestroyed(TValue item)
        {
            item.OnDestroyed();
            GameObject.Destroy(item.gameObject);
        }

        public override async UniTask<TValue> Spawn(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7)
        {
            TValue item = await GetInternal();
            item.OnSpawned(param1, param2, param3, param4, param5, param6, param7);
            item.gameObject.SetActive(true);
            return item;
        }
    }

    public class GameObjectMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TValue> : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TValue>
        where TValue : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8>
    {
        protected override void OnCreated(TValue item)
        {
            item.OnCreated();
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
            item.gameObject.SetActive(false);
        }

        protected override void OnDestroyed(TValue item)
        {
            item.OnDestroyed();
            GameObject.Destroy(item.gameObject);
        }

        public override async UniTask<TValue> Spawn(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8)
        {
            TValue item = await GetInternal();
            item.OnSpawned(param1, param2, param3, param4, param5, param6, param7, param8);
            item.gameObject.SetActive(true);
            return item;
        }
    }
}