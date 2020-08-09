using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MemoryPool<T> where T : Component
{
    protected FactoryBase<T> _factory;
    protected MemoryPoolSettings _memoryPoolSettings;

    protected List<T> _objectPool;

    public MemoryPool(MemoryPoolSettings _memoryPoolSettings, FactoryBase<T> _factory)
    {
        this._memoryPoolSettings = _memoryPoolSettings;
        this._factory = _factory;
        this._objectPool = new List<T>();

        for (int i = 0; i < _memoryPoolSettings.InitialSize; i++)
        {
            AddObjectToPool();
        }
    }

    private T AddObjectToPool()
    {
        T resultObject = _factory.Create();
        resultObject.gameObject.SetActive(false);
        this.OnCreated(resultObject);
        _objectPool.Add(resultObject);
        return resultObject;
    }

    private void ExpandPool()
    {
        int expandCount = 0;
        int currCount = this._objectPool.Count;
        switch (_memoryPoolSettings.ExpandMethod)
        {
            case PoolExpandMethods.Double: expandCount = (currCount > 0) ? currCount * 2 : 1; break;
            case PoolExpandMethods.OneAtATime: expandCount = 1; break;
            default:
                break;
        }

        for (int i = 0; i < expandCount; i++)
        {
            AddObjectToPool();
        }
    }

    private T GetAvailableObject()
    {
        int count = _objectPool.Count;
        if (count <= 0)
            return null;
        for (int i = 0; i < count; i++)
        {
            if (!_objectPool[i].gameObject.activeSelf)
                return _objectPool[i];
        }
        return null;
    }

    public T Spawn()
    {
        T resultObject = GetAvailableObject();
        if (resultObject == null)
        {
            ExpandPool();
            resultObject = GetAvailableObject();
        }

        if (resultObject != null)
        {
            OnSpawned(resultObject);
        }
        return resultObject;
    }

    public void Despawn(T obj)
    {
        if (_objectPool.Contains(obj))
        {
            this.OnDespawned(obj);
            _factory.Despawn(obj);
        }
    }

    protected virtual void OnCreated(T item)
    {

    }

    protected virtual void OnSpawned(T item)
    {
        item.gameObject.SetActive(true);
    }

    protected virtual void OnDespawned(T item)
    {

    }
}
