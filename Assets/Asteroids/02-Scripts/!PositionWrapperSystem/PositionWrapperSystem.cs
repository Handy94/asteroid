namespace Asteroid
{
    using HandyPackage;
    using System.Collections.Generic;
    using UniRx;
    using UniRx.Async;
    using UnityEngine;

    public class PositionWrapperSystem : IInitializable, ITickable, System.IDisposable
    {
        private AsteroidGameSettings _asteroidGameSettings;
        private GameSignals _gameSignals;

        private Vector2 minWorldPos;
        private Vector2 maxWorldPos;

        private List<Transform> _transformList = new List<Transform>();
        private CompositeDisposable disposables = new CompositeDisposable();

        public UniTask Initialize()
        {
            _asteroidGameSettings = DIResolver.GetObject<AsteroidGameSettings>();
            _gameSignals = DIResolver.GetObject<GameSignals>();

            Camera mainCamera = Camera.main;
            minWorldPos = mainCamera.ViewportToWorldPoint(_asteroidGameSettings.minWrapViewportPosition);
            maxWorldPos = mainCamera.ViewportToWorldPoint(_asteroidGameSettings.maxWrapViewportPosition);

            _gameSignals.PlayerSpawnedSignal.Listen(HandlePlayerSpawned).AddTo(disposables);
            _gameSignals.PlayerDespawnedSignal.Listen(HandlePlayerDespawned, PlayerDespawnedPrioritySignal.Priority.REMOVE_FROM_POSITION_WRAPPER).AddTo(disposables);

            _gameSignals.BulletSpawnedSignal.Listen(HandleBulletSpawned).AddTo(disposables);
            _gameSignals.BulletDespawnedSignal.Listen(HandleBulletDespawned).AddTo(disposables);

            _gameSignals.AsteroidSpawnedSignal.Listen(HandleAsteroidSpawned).AddTo(disposables);
            _gameSignals.AsteroidDespawnedSignal.Listen(HandleAsteroidDespawned).AddTo(disposables);

            return UniTask.CompletedTask;
        }

        public void Tick()
        {
            int count = _transformList.Count;
            for (int i = 0; i < count; i++)
            {
                if (_transformList[i] == null)
                {
                    _transformList.RemoveAt(i);
                    i--;
                }
                else
                {
                    WrapTransform(_transformList[i]);
                }
            }
        }

        public void Dispose()
        {
            disposables.Clear();
        }

        private void HandlePlayerSpawned(PlayerShipController playerShipController)
        {
            RegisterTransform(playerShipController.transform);
        }

        private bool HandlePlayerDespawned(PlayerShipController playerShipController)
        {
            RemoveTransform(playerShipController.transform);
            return true;
        }

        private void HandleBulletSpawned(BulletComponent bulletComponent)
        {
            RegisterTransform(bulletComponent.transform);
        }

        private void HandleBulletDespawned(BulletComponent bulletComponent)
        {
            RemoveTransform(bulletComponent.transform);
        }

        private void HandleAsteroidSpawned(AsteroidComponent asteroidComponent)
        {
            if (!_transformList.Contains(asteroidComponent.transform)) _transformList.Add(asteroidComponent.transform);
        }

        private void HandleAsteroidDespawned(AsteroidComponent asteroidComponent, GameEntityTag despawner)
        {
            if (_transformList.Contains(asteroidComponent.transform)) _transformList.Remove(asteroidComponent.transform);
        }

        public void RegisterTransform(Transform trans)
        {
            if (!_transformList.Contains(trans)) _transformList.Add(trans);
        }

        public void RemoveTransform(Transform trans)
        {
            if (_transformList.Contains(trans)) _transformList.Remove(trans);
        }

        private void WrapTransform(Transform trans)
        {
            Vector2 currPos = trans.position;
            currPos.x = RepeatValue(currPos.x, minWorldPos.x, maxWorldPos.x);
            currPos.y = RepeatValue(currPos.y, minWorldPos.y, maxWorldPos.y);

            trans.position = currPos;
        }

        private float RepeatValue(float value, float min, float max)
        {
            if (value < min)
                return max;
            else if (value > max)
                return min;

            return value;
        }
    }

}
