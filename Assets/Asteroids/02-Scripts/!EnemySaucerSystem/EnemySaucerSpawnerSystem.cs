namespace Asteroid
{
    using HandyPackage;
    using UniRx;
    using UniRx.Async;
    using UnityEngine;

    public class EnemySaucerSpawnerSystem : IInitializable, ITickable, System.IDisposable
    {
        private AsteroidGameSettings _asteroidGameSettings;
        private GameSignals _gameSignals;
        private MultiplePrefabMemoryPool _multiplePrefabMemoryPool;

        private float spawnTimer = 0;
        private CompositeDisposable disposables = new CompositeDisposable();

        public UniTask Initialize()
        {
            _asteroidGameSettings = DIResolver.GetObject<AsteroidGameSettings>();
            _gameSignals = DIResolver.GetObject<GameSignals>();
            _multiplePrefabMemoryPool = DIResolver.GetObject<MultiplePrefabMemoryPool>();

            _gameSignals.GameEntityDespawnedSignal.Listen(HandleGameEntityDespawned).AddToDisposables(disposables);

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            disposables.Clear();
        }

        public void Tick()
        {
            if (spawnTimer < _asteroidGameSettings.enemySpawnInterval)
            {
                spawnTimer += Time.deltaTime;
            }
            else
            {
                spawnTimer = 0f;
                SpawnSaucer();
            }

        }

        private void SpawnSaucer()
        {
            _multiplePrefabMemoryPool.SpawnObject(_asteroidGameSettings.enemySaucerPrefab, Vector2.one * 3);
        }

        private void HandleGameEntityDespawned(GameObject go, GameEntityTag gameEntityTag, GameEntityTag despawner)
        {
            if (gameEntityTag != GameEntityTag.ENEMY) return;
            _multiplePrefabMemoryPool.DespawnObject(go);
        }
    }

}
