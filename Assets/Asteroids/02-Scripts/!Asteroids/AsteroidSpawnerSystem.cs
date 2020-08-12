namespace Asteroid
{
    using HandyPackage;
    using UniRx;
    using UniRx.Async;
    using UnityEngine;

    public class AsteroidSpawnerSystem : IInitializable, ITickable, System.IDisposable
    {
        private MultiplePrefabMemoryPool _multiplePrefabMemoryPool;
        private AsteroidGameSettings _asteroidGameSettings;
        private GameSignals _gameSignals;

        private CompositeDisposable disposables = new CompositeDisposable();
        private Vector2 minAdditionalWorldPos;
        private Vector2 maxAdditionalWorldPos;
        private Vector2 minWorldPos;
        private Vector2 maxWorldPos;

        private float timer = 0f;
        private int currentSpawned = 0;

        public UniTask Initialize()
        {
            _multiplePrefabMemoryPool = DIResolver.GetObject<MultiplePrefabMemoryPool>();
            _asteroidGameSettings = DIResolver.GetObject<AsteroidGameSettings>();
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

        public void Tick()
        {
            if (currentSpawned < _asteroidGameSettings.maxAsteroidCount)
            {
                if (timer < _asteroidGameSettings.asteroidSpawnTime)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    timer = 0f;
                    SpawnAsteroid();
                }
            }
        }

        private void HandleGameEntityDespawned(GameObject go, GameEntityTag gameEntityTag)
        {
            if (gameEntityTag != GameEntityTag.ASTEROID) return;
            DespawnAsteroid(go.GetComponent<AsteroidComponent>());
        }

        private async void SpawnAsteroid()
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

            GameObject asteroidGO = await _multiplePrefabMemoryPool.SpawnObject(_asteroidGameSettings.asteroidPrefab.gameObject, spawnPos);
            var asteroid = asteroidGO.GetComponent<AsteroidComponent>();

            float speed = Random.Range(1f, 10f);
            Vector2 moveDirection = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
            if (!isMinHorizontal) moveDirection.x *= -1;
            if (!isMinVertical) moveDirection.y *= -1;

            asteroid.Init(moveDirection, speed);
            _gameSignals.AsteroidSpawnedSignal.Fire(asteroid);

            currentSpawned++;
        }

        private void DespawnAsteroid(AsteroidComponent asteroidComponent)
        {
            _gameSignals.AsteroidDespawnedSignal.Fire(asteroidComponent);
            _multiplePrefabMemoryPool.DespawnObject(asteroidComponent.gameObject);
            currentSpawned--;
        }
    }

}
