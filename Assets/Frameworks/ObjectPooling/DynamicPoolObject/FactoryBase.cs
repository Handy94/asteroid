using UnityEngine;

public abstract class FactoryBase<T> where T : Component
{
    protected GameObject _prefab;

    public FactoryBase(
        GameObject prefab)
    {
        _prefab = prefab;
    }

    public virtual T Create()
    {
        T poolObject = GameObject.Instantiate(_prefab).GetComponent<T>();
        OnCreated(poolObject);
        return poolObject;
    }

    public virtual void Despawn(T obj)
    {
        OnDespawned(obj);
        obj.gameObject.SetActive(false);
    }

    public abstract void OnCreated(T obj);
    public abstract void OnDespawned(T obj);
}