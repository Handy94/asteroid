﻿namespace Asteroid
{
    using HandyPackage;
    using UniRx;
    using UniRx.Async;
    using UnityEngine;

    public class StageWaveSystem : IInitializable, System.IDisposable
    {
        private GameSignals _gameSignals;
        private AsteroidSpawnerSystem _asteroidSpawnerSystem;
        private AsteroidAssetSource _asteroidAssetSource;
        private BookKeepingInGameData _bookKeepingInGameData;

        private CompositeDisposable disposables = new CompositeDisposable();

        public UniTask Initialize()
        {
            _gameSignals = DIResolver.GetObject<GameSignals>();
            _asteroidSpawnerSystem = DIResolver.GetObject<AsteroidSpawnerSystem>();
            _asteroidAssetSource = DIResolver.GetObject<AsteroidAssetSource>();
            _bookKeepingInGameData = DIResolver.GetObject<BookKeepingInGameData>();

            _gameSignals.AsteroidSpawnedSignal.Listen(HandleAsteroidSpawned).AddTo(disposables);
            _gameSignals.AsteroidDespawnedSignal.Listen(HandleAsteroidDespawned).AddTo(disposables);

            SpawnNextWave();

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            disposables.Clear();
        }

        private void HandleAsteroidSpawned(AsteroidComponent asteroidComponent)
        {
            _bookKeepingInGameData.AsteroidCount.Value++;
        }

        private void HandleAsteroidDespawned(AsteroidComponent asteroidComponent, GameEntityTag gameEntityTag)
        {
            _bookKeepingInGameData.AsteroidCount.Value--;

            if (_bookKeepingInGameData.AsteroidCount.Value < 0)
                throw new System.ArgumentException("Asteroid count value cannot be negative");

            if (_bookKeepingInGameData.AsteroidCount.Value == 0)
            {
                _bookKeepingInGameData.CurrentStage.Value++;
                SpawnNextWave();
            }
        }

        private void SpawnNextWave()
        {
            _bookKeepingInGameData.AsteroidCount.Value = 0;

            var currentStageWave = _asteroidAssetSource.GetStageWaveData(_bookKeepingInGameData.CurrentStage.Value);
            if (currentStageWave == null)
                throw new System.NullReferenceException($"Stage Wave Data null on stage {_bookKeepingInGameData.CurrentStage.Value}");

            int asteroidTypeCount = currentStageWave.spawnAsteroidDataList.Count;
            for (int i = 0; i < asteroidTypeCount; i++)
            {
                int spawnCount = Random.Range(currentStageWave.spawnAsteroidDataList[i].minSpawnCount, currentStageWave.spawnAsteroidDataList[i].maxSpawnCount + 1);
                AsteroidData asteroidData = _asteroidAssetSource.GetAsteroidData(currentStageWave.spawnAsteroidDataList[i].asteroidID);
                for (int j = 0; j < spawnCount; j++)
                {
                    _asteroidSpawnerSystem.SpawnAsteroidAtOutOfScreenPosition(asteroidData);
                }
            }
        }
    }

}