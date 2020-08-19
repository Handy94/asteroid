namespace Asteroid
{
    using HandyPackage;
    using System.Collections.Generic;
    using UniRx;
    using UniRx.Async;
    using UnityEngine;

    public class EnemySaucerSpawnerSystem : IInitializable, System.IDisposable
    {
        private AsteroidGameSettings _asteroidGameSettings;
        private GameSignals _gameSignals;
        private MultiplePrefabMemoryPool _multiplePrefabMemoryPool;

        private float spawnTimer = 0;
        private List<EnemyComponent> spawnedEnemies = new List<EnemyComponent>();
        private CompositeDisposable disposables = new CompositeDisposable();

        public UniTask Initialize()
        {
            _asteroidGameSettings = DIResolver.GetObject<AsteroidGameSettings>();
            _gameSignals = DIResolver.GetObject<GameSignals>();
            _multiplePrefabMemoryPool = DIResolver.GetObject<MultiplePrefabMemoryPool>();

            _gameSignals.GameStartSignal.Listen(HandleGameStart, GameStartPrioritySignal.PRIORITY_SETUP_SPAWN_ENEMY).AddTo(disposables);
            _gameSignals.GameEntityDespawnedSignal.Listen(HandleGameEntityDespawned).AddToDisposables(disposables);

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            disposables.Clear();
        }

        private bool HandleGameStart()
        {
            DespawnAllEnemy();
            return true;
        }

        private void HandleGameEntityDespawned(GameObject go, GameEntityTag gameEntityTag, GameEntityTag despawner)
        {
            if (gameEntityTag != GameEntityTag.ENEMY) return;
            DespawnEnemy(go.GetComponent<EnemyComponent>(), despawner);
        }

        public async UniTask<EnemyComponent> SpawnEnemy(EnemyData enemyData, Vector2 spawnPosition)
        {
            GameObject newEnemyGO = await _multiplePrefabMemoryPool.SpawnObject(enemyData.EnemyPrefab.gameObject, spawnPosition);
            EnemyComponent newEnemy = newEnemyGO.GetComponent<EnemyComponent>();
            newEnemy.Init();
            newEnemy.SetData(enemyData);
            spawnedEnemies.Add(newEnemy);

            _gameSignals.EnemySpawnedSignal.Fire(newEnemy);

            return newEnemy;
        }

        public void DespawnEnemy(EnemyComponent enemyComponent, GameEntityTag despawner)
        {
            spawnedEnemies.Remove(enemyComponent);

            _gameSignals.EnemyDespawnedSignal.Fire(enemyComponent, despawner);
            _multiplePrefabMemoryPool.DespawnObject(enemyComponent.gameObject);
        }

        public void DespawnAllEnemy()
        {
            int enemyCount = spawnedEnemies.Count;
            for (int i = 0; i < enemyCount; i++)
            {
                _multiplePrefabMemoryPool.DespawnObject(spawnedEnemies[i].gameObject);
            }
            spawnedEnemies.Clear();
        }
    }

}
