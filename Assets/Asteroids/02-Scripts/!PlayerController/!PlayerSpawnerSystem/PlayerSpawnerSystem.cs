namespace Asteroid
{
    using HandyPackage;
    using UniRx;
    using UniRx.Async;
    using UnityEngine;

    public class PlayerSpawnerSystem : IInitializable, System.IDisposable
    {
        private MultiplePrefabMemoryPool _multiplePrefabMemoryPool;
        private GameSignals _gameSignals;
        private AsteroidGameSettings _asteroidGameSettings;

        private CompositeDisposable disposables = new CompositeDisposable();

        public async UniTask Initialize()
        {
            _multiplePrefabMemoryPool = DIResolver.GetObject<MultiplePrefabMemoryPool>();
            _gameSignals = DIResolver.GetObject<GameSignals>();
            _asteroidGameSettings = DIResolver.GetObject<AsteroidGameSettings>();

            _gameSignals.GameStartSignal.Listen(HandleGameStart, GameStartPrioritySignal.PRIORITY_SPAWN_PLAYER).AddToDisposables(disposables);
            _gameSignals.GameEntityDespawnedSignal.Listen(HandleGameEntityDespawned).AddToDisposables(disposables);

            await UniTask.CompletedTask;
        }

        public void Dispose()
        {
            disposables.Clear();
        }

        private bool HandleGameStart()
        {
            SpawnPlayer(_asteroidGameSettings.playerPrefab);
            return true;
        }

        private void HandleGameEntityDespawned(GameObject go, GameEntityTag gameEntityTag, GameEntityTag despawner)
        {
            if (gameEntityTag != GameEntityTag.PLAYER) return;

            DespawnPlayer(go.GetComponent<PlayerShipComponent>());
        }

        public async UniTask SpawnPlayer()
        {
            await SpawnPlayer(_asteroidGameSettings.playerPrefab);
        }

        public async UniTask SpawnPlayer(PlayerShipComponent playerPrefab)
        {
            GameObject go = await _multiplePrefabMemoryPool.SpawnObject(playerPrefab.gameObject);
            go.transform.position = Vector3.zero;

            _gameSignals.PlayerSpawnedSignal.Fire(go.GetComponent<PlayerShipComponent>());
        }

        public void DespawnPlayer(PlayerShipComponent player)
        {
            _multiplePrefabMemoryPool.DespawnObject(player.gameObject);

            _gameSignals.PlayerDespawnedSignal.Fire(player);
        }
    }

}
