namespace Asteroid
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

            _gameSignals.GameStartSignal.Listen(HandleGameStartSignal, GameStartPrioritySignal.PRIORITY_SPAWN_WAVE).AddTo(disposables);
            _gameSignals.AsteroidSpawnedSignal.Listen(HandleAsteroidSpawned).AddTo(disposables);
            _gameSignals.AsteroidDespawnedSignal.Listen(HandleAsteroidDespawned).AddTo(disposables);

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            disposables.Clear();
        }

        private bool HandleGameStartSignal()
        {
            _asteroidSpawnerSystem.DespawnAllAsteroid();
            SpawnWave();
            return true;
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
                SpawnWave();
            }
        }

        private void SpawnWave()
        {
            _bookKeepingInGameData.AsteroidCount.Value = 0;

            var currentStageWave = _asteroidAssetSource.GetStageWaveData(_bookKeepingInGameData.CurrentStage.Value);
            if (currentStageWave == null)
                throw new System.NullReferenceException($"Stage Wave Data null on stage {_bookKeepingInGameData.CurrentStage.Value}");

            int asteroidTypeCount = currentStageWave.SpawnAsteroidDataList.Count;
            for (int i = 0; i < asteroidTypeCount; i++)
            {
                int spawnCount = Random.Range(currentStageWave.SpawnAsteroidDataList[i].MinSpawnCount, currentStageWave.SpawnAsteroidDataList[i].MaxSpawnCount + 1);
                AsteroidData asteroidData = _asteroidAssetSource.GetAsteroidData(currentStageWave.SpawnAsteroidDataList[i].AsteroidID);
                for (int j = 0; j < spawnCount; j++)
                {
                    _asteroidSpawnerSystem.SpawnAsteroidAtOutOfScreenPosition(asteroidData);
                }
            }
        }
    }

}
