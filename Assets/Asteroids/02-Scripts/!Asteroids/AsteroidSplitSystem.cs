﻿namespace Asteroid
{
    using HandyPackage;
    using UniRx;
    using UniRx.Async;
    using UnityEngine;

    public class AsteroidSplitSystem : IInitializable, System.IDisposable
    {
        private GameSignals _gameSignals;
        private AsteroidSpawnerSystem _asteroidSpawnerSystem;
        private AsteroidGameAssetSource _asteroidAssetSource;

        private CompositeDisposable disposables = new CompositeDisposable();

        public UniTask Initialize()
        {
            _gameSignals = DIResolver.GetObject<GameSignals>();
            _asteroidSpawnerSystem = DIResolver.GetObject<AsteroidSpawnerSystem>();
            _asteroidAssetSource = DIResolver.GetObject<AsteroidGameAssetSource>();

            _gameSignals.AsteroidDespawnedSignal.Listen(HandleAsteroidDespawned).AddTo(disposables);
            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            disposables.Clear();
        }

        private async void HandleAsteroidDespawned(AsteroidComponent asteroid, GameEntityTag despawner)
        {
            await TryToSpawnAsteroidSplit(asteroid);
        }

        private async UniTask TryToSpawnAsteroidSplit(AsteroidComponent asteroid)
        {
            var splitData = _asteroidAssetSource.GetAsteroidSplitData(asteroid.AsteroidData.AsteroidID);
            if (splitData != null)
            {
                Vector2 spawnPos = asteroid.transform.position;

                int count = splitData.SplitCountData.Length;
                for (int i = 0; i < count; i++)
                {
                    int spawnCount = Random.Range(splitData.SplitCountData[i].MinCount, splitData.SplitCountData[i].MaxCount + 1);
                    var asteroidData = _asteroidAssetSource.GetAsteroidData(splitData.SplitCountData[i].AsteroidID);

                    Vector2 baseDirection = new Vector2(Random.Range(0.1f, 1f), Random.Range(0.1f, 1f));

                    for (int j = 0; j < spawnCount; j++)
                    {
                        float randomRotate = Random.Range(360 * j / spawnCount, 360 * (j + 1) / spawnCount);
                        Vector2 moveDirection = (Quaternion.Euler(0, 0, randomRotate) * baseDirection);
                        await _asteroidSpawnerSystem.SpawnAsteroid(asteroidData, moveDirection, spawnPos);
                    }
                }
            }
        }
    }

}
