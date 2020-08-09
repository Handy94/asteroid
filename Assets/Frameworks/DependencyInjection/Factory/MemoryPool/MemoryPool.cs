using UniRx.Async;

namespace HandyPackage
{
    public class MemoryPool<TValue> : MemoryPoolBase<TValue>, IMemoryPool<TValue>
        where TValue : IPoolable
    {
        public virtual async UniTask<TValue> Spawn()
        {
            TValue item = await GetInternal();
            item.OnSpawned();
            return item;
        }
    }

    public class MemoryPool<TParam1, TValue> : MemoryPoolBase<TValue>, IMemoryPool<TParam1, TValue>
        where TValue : IPoolable<TParam1>
    {
        public virtual async UniTask<TValue> Spawn(TParam1 param1)
        {
            TValue item = await GetInternal();
            item.OnSpawned(param1);
            return item;
        }
    }

    public class MemoryPool<TParam1, TParam2, TValue> : MemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TValue>
        where TValue : IPoolable<TParam1, TParam2>
    {
        public virtual async UniTask<TValue> Spawn(TParam1 param1, TParam2 param2)
        {
            TValue item = await GetInternal();
            item.OnSpawned(param1, param2);
            return item;
        }
    }

    public class MemoryPool<TParam1, TParam2, TParam3, TValue> : MemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TValue>
        where TValue : IPoolable<TParam1, TParam2, TParam3>
    {
        public virtual async UniTask<TValue> Spawn(TParam1 param1, TParam2 param2, TParam3 param3)
        {
            TValue item = await GetInternal();
            item.OnSpawned(param1, param2, param3);
            return item;
        }
    }

    public class MemoryPool<TParam1, TParam2, TParam3, TParam4, TValue> : MemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>
        where TValue : IPoolable<TParam1, TParam2, TParam3, TParam4>
    {
        public virtual async UniTask<TValue> Spawn(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            TValue item = await GetInternal();
            item.OnSpawned(param1, param2, param3, param4);
            return item;
        }
    }

    public class MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> : MemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>
        where TValue : IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5>
    {
        public virtual async UniTask<TValue> Spawn(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            TValue item = await GetInternal();
            item.OnSpawned(param1, param2, param3, param4, param5);
            return item;
        }
    }

    public class MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> : MemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>
        where TValue : IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>
    {
        public virtual async UniTask<TValue> Spawn(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6)
        {
            TValue item = await GetInternal();
            item.OnSpawned(param1, param2, param3, param4, param5, param6);
            return item;
        }
    }

    public class MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue> : MemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue>
        where TValue : IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7>
    {
        public virtual async UniTask<TValue> Spawn(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7)
        {
            TValue item = await GetInternal();
            item.OnSpawned(param1, param2, param3, param4, param5, param6, param7);
            return item;
        }
    }

    public class MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TValue> : MemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TValue>
        where TValue : IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8>
    {
        public virtual async UniTask<TValue> Spawn(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8)
        {
            TValue item = await GetInternal();
            item.OnSpawned(param1, param2, param3, param4, param5, param6, param7, param8);
            return item;
        }
    }
}
