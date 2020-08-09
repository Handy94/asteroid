using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HandyPackage;
using System.Threading.Tasks;
using UniRx.Async;

namespace HandyPackage.Examples
{
    public class Testclass : MonoBehaviour, IInitializable
    {
        IMemoryPool<TestPoolableGameObject> pool;
        List<TestPoolableGameObject> spawnedObject;

        public UniTask Initialize()
        {
            pool = DIResolver.GetObject<IMemoryPool<TestPoolableGameObject>>();

            spawnedObject = new List<TestPoolableGameObject>();
            Debug.Log("Start");
            TestCoroutine();
            Debug.Log("End");

            return UniTask.CompletedTask;
        }

        async void TestCoroutine()
        {
            await SpawnCoroutine(100);
            await DespawnCoroutine(50);
            await SpawnCoroutine(100);
            await DespawnCoroutine(130);
        }
        async Task SpawnCoroutine(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var obj = await pool.Spawn();
                obj.gameObject.name = obj.gameObject.name + $"({i})";
                spawnedObject.Add(obj);
            }
        }

        Task DespawnCoroutine(int count)
        {
            int length = spawnedObject.Count;

            for (int i = 0; i < count; i++)
            {
                int index = length - 1 - i;
                pool.Despawn(spawnedObject[index]);
                spawnedObject.RemoveAt(index);
            }
            return Task.CompletedTask;
        }
    }
}