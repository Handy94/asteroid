namespace Asteroid
{
    using HandyPackage;
    using UniRx;
    using UniRx.Async;
    using UnityEngine;

    public class SpawnEnemySaucerOnIntervalSystem : IInitializable, System.IDisposable
    {
        private AsteroidGameSettings _asteroidGameSettings;
        private AsteroidGameAssetSource _asteroidGameAssetSource;
        private GameSignals _gameSignals;
        private EnemySaucerSpawnerSystem _enemySaucerSpawnerSystem;

        private float spawnTimer = 0;
        private int spawnCount = 0;

        private Vector2 minAdditionalWorldPos;
        private Vector2 maxAdditionalWorldPos;
        private Vector2 minWorldPos;
        private Vector2 maxWorldPos;

        private CompositeDisposable disposables = new CompositeDisposable();
        private CompositeDisposable spawnerDisposables = new CompositeDisposable();

        public UniTask Initialize()
        {
            _asteroidGameSettings = DIResolver.GetObject<AsteroidGameSettings>();
            _asteroidGameAssetSource = DIResolver.GetObject<AsteroidGameAssetSource>();
            _gameSignals = DIResolver.GetObject<GameSignals>();
            _enemySaucerSpawnerSystem = DIResolver.GetObject<EnemySaucerSpawnerSystem>();

            _gameSignals.GameStartSignal.Listen(HandleGameStart, GameStartPrioritySignal.PRIORITY_SETUP_SPAWN_ENEMY).AddTo(disposables);
            _gameSignals.GameOverSignal.Listen(HandleGameOver).AddTo(disposables);

            Camera mainCamera = Camera.main;
            minWorldPos = mainCamera.ViewportToWorldPoint(Vector2.zero);
            maxWorldPos = mainCamera.ViewportToWorldPoint(Vector2.one);

            minAdditionalWorldPos = mainCamera.ViewportToWorldPoint(_asteroidGameSettings.minEnemySpawnAdditionalViewportPosition);
            minAdditionalWorldPos -= minWorldPos;
            maxAdditionalWorldPos = mainCamera.ViewportToWorldPoint(_asteroidGameSettings.maxEnemySpawnAdditionalViewportPosition);
            maxAdditionalWorldPos -= minWorldPos;

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            disposables.Clear();
        }

        private bool HandleGameStart()
        {
            spawnerDisposables.Clear();
            Observable.EveryUpdate().Subscribe(x =>
            {
                if (spawnCount >= _asteroidGameSettings.maxEnemyOnScreen) return;

                if (spawnTimer < _asteroidGameSettings.enemySpawnInterval)
                {
                    spawnTimer += Time.deltaTime;
                }
                else
                {
                    spawnTimer = 0f;
                    SpawnRandomEnemy();
                }
            }).AddTo(spawnerDisposables);
            return true;
        }

        private void HandleGameOver()
        {
            spawnerDisposables.Clear();
        }

        private void SpawnRandomEnemy()
        {
            bool isMinHorizontal = Random.Range(0, 2) == 0;
            bool isMinVertical = Random.Range(0, 2) == 0;

            int idx = Random.Range(0, _asteroidGameAssetSource.enemyDataList.Count);
            EnemyData randomEnemyData = _asteroidGameAssetSource.enemyDataList[idx];

            Vector2 baseSpawnPos = Vector2.zero;
            baseSpawnPos.x = (isMinHorizontal) ? minWorldPos.x : maxWorldPos.x;
            baseSpawnPos.y = (isMinVertical) ? minWorldPos.y : maxWorldPos.y;

            Vector2 additionalPos = Vector2.zero;
            additionalPos.x = Random.Range(minAdditionalWorldPos.x, maxAdditionalWorldPos.x);
            additionalPos.y = Random.Range(minAdditionalWorldPos.y, maxAdditionalWorldPos.y);

            Vector2 spawnPos = baseSpawnPos;
            if (isMinHorizontal) spawnPos.x -= additionalPos.x;
            else spawnPos.x += additionalPos.x;

            _enemySaucerSpawnerSystem.SpawnEnemy(randomEnemyData, spawnPos);
        }
    }

}
