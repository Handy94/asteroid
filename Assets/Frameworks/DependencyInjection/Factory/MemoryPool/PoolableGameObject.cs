namespace HandyPackage
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PoolableGameObject : MonoBehaviour, IPoolable
    {
        private List<IPoolable> poolables;

        public void OnCreated()
        {
            poolables = transform.GetComponentsInChildren<IPoolable>().ToList().FindAll(x => x != this);
            ForEachPoolable(x => x.OnCreated());
        }

        public void OnDespawned()
        {
            ForEachPoolable(x => x.OnDespawned());
        }

        public void OnDestroyed()
        {
            ForEachPoolable(x => x.OnDestroyed());
        }

        public void OnSpawned()
        {
            ForEachPoolable(x => x.OnSpawned());
        }

        private void ForEachPoolable(Action<IPoolable> action)
        {
            if (poolables.Count > 0)
            {
                poolables.ForEach(poolable =>
                {
                    if (poolable == null) return;
                    if (action != null) action.Invoke(poolable);
                });
            }
        }
    }
}
