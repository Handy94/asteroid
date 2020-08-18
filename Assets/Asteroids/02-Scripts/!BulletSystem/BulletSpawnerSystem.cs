namespace Asteroid
{
    using HandyPackage;
    using System.Collections.Generic;
    using UniRx.Async;
    using UnityEngine;

    public class BulletSpawnerSystem : IInitializable, System.IDisposable
    {
        private MultiplePrefabMemoryPool _multiplePrefabMemoryPool;
        private GameSignals _gameSignals;

        private DisposableCollection disposables = new DisposableCollection();

        public UniTask Initialize()
        {
            _gameSignals = DIResolver.GetObject<GameSignals>();
            _multiplePrefabMemoryPool = DIResolver.GetObject<MultiplePrefabMemoryPool>();

            _gameSignals.GameEntityDespawnedSignal.Listen(HandleGameEntityDespawned).AddToDisposables(disposables);

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            disposables.Clear();
        }

        private async void HandlePlayerShoot(GameObject prefab, Transform[] spawnPoints)
        {
            await SpawnBullet(prefab, spawnPoints);
        }

        private void HandleGameEntityDespawned(GameObject go, GameEntityTag gameEntityTag, GameEntityTag despawner)
        {
            if (gameEntityTag != GameEntityTag.BULLET && gameEntityTag != GameEntityTag.ENEMY_BULLET) return;
            DespawnBullet(go.GetComponent<BulletComponent>());
        }

        public async UniTask<List<BulletComponent>> SpawnBullet(GameObject prefab, Transform[] spawnPoints)
        {
            List<BulletComponent> bullets = new List<BulletComponent>();

            if (prefab == null) return bullets;
            int spawnPointCount = spawnPoints.Length;

            for (int i = 0; i < spawnPointCount; i++)
            {
                if (spawnPoints[i] == null) continue;
                var newBullet = await SpawnBullet(prefab, spawnPoints[i]);
                bullets.Add(newBullet);
            }
            return bullets;
        }

        public async UniTask<BulletComponent> SpawnBullet(GameObject prefab, Transform spawnPoint)
        {
            GameObject bulletGO = await _multiplePrefabMemoryPool.SpawnObject(prefab);
            bulletGO.transform.position = spawnPoint.position;
            bulletGO.transform.rotation = spawnPoint.rotation;

            BulletComponent bullet = bulletGO.GetComponent<BulletComponent>();

            _gameSignals.BulletSpawnedSignal.Fire(bullet);
            return bullet;
        }

        private void DespawnBullet(BulletComponent bullet)
        {
            _gameSignals.BulletDespawnedSignal.Fire(bullet);
            _multiplePrefabMemoryPool.DespawnObject(bullet.gameObject);
        }
    }

}
