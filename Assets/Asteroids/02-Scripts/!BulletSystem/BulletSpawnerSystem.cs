namespace Asteroid
{
    using HandyPackage;
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

            _gameSignals.PlayerShootSignal.Listen(HandlePlayerShoot).AddToDisposables(disposables);
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
            if (gameEntityTag != GameEntityTag.BULLET) return;
            DespawnBullet(go.GetComponent<BulletComponent>());
        }

        private async UniTask SpawnBullet(GameObject prefab, Transform[] spawnPoints)
        {
            if (prefab == null) return;
            int spawnPointCount = spawnPoints.Length;

            for (int i = 0; i < spawnPointCount; i++)
            {
                if (spawnPoints[i] == null) continue;
                await SpawnBullet(prefab, spawnPoints[i]);
            }
        }

        private async UniTask SpawnBullet(GameObject prefab, Transform spawnPoint)
        {
            GameObject bulletGO = await _multiplePrefabMemoryPool.SpawnObject(prefab);
            bulletGO.transform.position = spawnPoint.position;
            bulletGO.transform.rotation = spawnPoint.rotation;

            BulletComponent bullet = bulletGO.GetComponent<BulletComponent>();
            bullet.Init(spawnPoint.up);

            _gameSignals.BulletSpawnedSignal.Fire(bullet);
        }

        private void DespawnBullet(BulletComponent bullet)
        {
            _gameSignals.BulletDespawnedSignal.Fire(bullet);
            _multiplePrefabMemoryPool.DespawnObject(bullet.gameObject);
        }
    }

}
