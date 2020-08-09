using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HandyPackage;

public class DynamicPoolObjectDespawner : MonoBehaviour, IPoolable
{
    private MultiplePrefabMemoryPool _multiplePrefabMemoryPool => DIResolver.GetObject<MultiplePrefabMemoryPool>();

    private float timer = 0f;
    private bool destroyed = false;

    public float destroyDelay = 1f;

    // Use this for initialization
    public void Init()
    {

    }

    void Update()
    {
        if (!destroyed)
        {
            if (timer < destroyDelay)
            {
                timer += Time.deltaTime;
            }
            else
            {
                destroyed = true;
                timer = 0f;
                _multiplePrefabMemoryPool.DespawnObject(gameObject);
            }
        }
    }

    public void OnSpawned()
    {
        destroyed = false;
        timer = 0f;
    }

    public void OnCreated()
    {
    }

    public void OnDespawned()
    {
    }

    public void OnDestroyed()
    {
    }
}
