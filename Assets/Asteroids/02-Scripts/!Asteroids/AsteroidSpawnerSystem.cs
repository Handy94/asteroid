namespace Asteroid
{
    using HandyPackage;
    using UniRx;
    using UniRx.Async;
    using UnityEngine;

    public class AsteroidSpawnerSystem : IInitializable, System.IDisposable
    {
        private MultiplePrefabMemoryPool _multiplePrefabMemoryPool;
        private AsteroidGameSettings _asteroidGameSettings;
        private AsteroidAssetSource _asteroidAssetSource;
        private GameSignals _gameSignals;

        private CompositeDisposable disposables = new CompositeDisposable();
        private Vector2 minAdditionalWorldPos;
        private Vector2 maxAdditionalWorldPos;
        private Vector2 minWorldPos;
        private Vector2 maxWorldPos;

        public UniTask Initialize()
        {
            _multiplePrefabMemoryPool = DIResolver.GetObject<MultiplePrefabMemoryPool>();
            _asteroidGameSettings = DIResolver.GetObject<AsteroidGameSettings>();
            _asteroidAssetSource = DIResolver.GetObject<AsteroidAssetSource>();
            _gameSignals = DIResolver.GetObject<GameSignals>();

            Camera mainCamera = Camera.main;
            minWorldPos = mainCamera.ViewportToWorldPoint(Vector2.zero);
            maxWorldPos = mainCamera.ViewportToWorldPoint(Vector2.one);

            minAdditionalWorldPos = mainCamera.ViewportToWorldPoint(_asteroidGameSettings.minAsteroidSpawnAdditionalViewportPosition);
            minAdditionalWorldPos -= minWorldPos;
            maxAdditionalWorldPos = mainCamera.ViewportToWorldPoint(_asteroidGameSettings.maxAsteroidSpawnAdditionalViewportPosition);
            maxAdditionalWorldPos -= minWorldPos;

            _gameSignals.GameEntityDespawnedSignal.Listen(HandleGameEntityDespawned).AddTo(disposables);

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            disposables.Clear();
        }

        private AsteroidData GetRandomAsteroidData()
        {
            int randomIndex = Random.Range(0, _asteroidAssetSource.asteroidSpawnVariants.Count);
            return _asteroidAssetSource.asteroidSpawnVariants[randomIndex];
        }

        public AsteroidComponent GetRandomAsteroidPrefab(AsteroidData asteroidData)
        {
            int randomIndex = Random.Range(0, asteroidData.prefabVariants.Length);
            return asteroidData.prefabVariants[randomIndex];
        }

        private void HandleGameEntityDespawned(GameObject go, GameEntityTag gameEntityTag, GameEntityTag despawner)
        {
            if (gameEntityTag != GameEntityTag.ASTEROID) return;
            DespawnAsteroid(go.GetComponent<AsteroidComponent>(), despawner);
        }

        public async UniTask SpawnAsteroidAtOutOfScreenPosition(AsteroidData asteroidData)
        {
            bool isMinHorizontal = Random.Range(0, 2) == 0;
            bool isMinVertical = Random.Range(0, 2) == 0;

            Vector2 baseSpawnPos = Vector2.zero;
            baseSpawnPos.x = (isMinHorizontal) ? minWorldPos.x : maxWorldPos.x;
            baseSpawnPos.y = (isMinVertical) ? minWorldPos.y : maxWorldPos.y;

            Vector2 additionalPos = Vector2.zero;
            additionalPos.x = Random.Range(minAdditionalWorldPos.x, maxAdditionalWorldPos.x);
            additionalPos.y = Random.Range(minAdditionalWorldPos.y, maxAdditionalWorldPos.y);

            Vector2 spawnPos = baseSpawnPos;
            if (isMinHorizontal) spawnPos.x -= additionalPos.x;
            else spawnPos.x += additionalPos.x;

            if (isMinVertical) spawnPos.y -= additionalPos.y;
            else spawnPos.y += additionalPos.y;

            float speed = Random.Range(1f, 10f);
            Vector2 moveDirection = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
            if (!isMinHorizontal) moveDirection.x *= -1;
            if (!isMinVertical) moveDirection.y *= -1;

            await SpawnAsteroid(asteroidData, moveDirection, spawnPos);
        }

        public async UniTask SpawnAsteroid(AsteroidData asteroidData, Vector2 moveDirection, Vector2 spawnPos)
        {
            GameObject asteroidGO = await _multiplePrefabMemoryPool.SpawnObject(GetRandomAsteroidPrefab(asteroidData).gameObject, spawnPos);
            var asteroid = asteroidGO.GetComponent<AsteroidComponent>();

            asteroid.Init(moveDirection, asteroidData);
            _gameSignals.AsteroidSpawnedSignal.Fire(asteroid);
        }

        private void DespawnAsteroid(AsteroidComponent asteroidComponent, GameEntityTag despawner)
        {
            _gameSignals.AsteroidDespawnedSignal.Fire(asteroidComponent, despawner);
            _multiplePrefabMemoryPool.DespawnObject(asteroidComponent.gameObject);
        }
    }

}
