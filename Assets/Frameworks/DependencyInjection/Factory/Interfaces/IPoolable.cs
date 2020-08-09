namespace HandyPackage
{
    public interface IPoolableBase
    {
        void OnCreated();
        void OnDespawned();
        void OnDestroyed();
    }

    public interface IPoolable : IPoolableBase
    {
        void OnSpawned();
    }

    public interface IPoolable<in TParam1> : IPoolableBase
    {
        void OnSpawned(TParam1 param);
    }

    public interface IPoolable<in TParam1, in TParam2> : IPoolableBase
    {
        void OnSpawned(TParam1 param1, TParam2 param2);
    }

    public interface IPoolable<in TParam1, in TParam2, in TParam3> : IPoolableBase
    {
        void OnSpawned(TParam1 param1, TParam2 param2, TParam3 param3);
    }

    public interface IPoolable<in TParam1, in TParam2, in TParam3, in TParam4> : IPoolableBase
    {
        void OnSpawned(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4);
    }

    public interface IPoolable<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5> : IPoolableBase
    {
        void OnSpawned(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5);
    }

    public interface IPoolable<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5, in TParam6> : IPoolableBase
    {
        void OnSpawned(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6);
    }

    public interface IPoolable<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5, in TParam6, in TParam7> : IPoolableBase
    {
        void OnSpawned(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7);
    }

    public interface IPoolable<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5, in TParam6, in TParam7, in TParam8> : IPoolableBase
    {
        void OnSpawned(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8);
    }
}