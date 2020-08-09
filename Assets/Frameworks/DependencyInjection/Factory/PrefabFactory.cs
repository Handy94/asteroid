using System.Threading.Tasks;
using UniRx.Async;
using UnityEngine;

namespace HandyPackage
{
    public class PrefabFactory<TValue> : IFactory<TValue> where TValue : Component
    {
        public GameObject prefab;

        public PrefabFactory(GameObject prefab)
        {
            this.prefab = prefab;
        }

        UniTask<TValue> IFactory<TValue>.Create()
        {
            GameObject go = GameObject.Instantiate(prefab);
            TValue comp = go.GetComponent<TValue>();
            if (comp == null)
            {
                comp = go.AddComponent<TValue>();
            }
            return UniTask.FromResult(comp);
        }
    }
}