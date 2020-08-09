namespace Asteroid
{
    using HandyPackage;
    using System.Collections.Generic;
    using UniRx.Async;
    using UnityEngine;

    public class PositionWrapperSystem : IInitializable, ITickable, System.IDisposable
    {
        private AsteroidGameSettings _asteroidGameSettings;

        private Vector2 minWorldPos;
        private Vector2 maxWorldPos;

        private List<Transform> _transformList = new List<Transform>();

        public UniTask Initialize()
        {
            _asteroidGameSettings = DIResolver.GetObject<AsteroidGameSettings>();

            Camera mainCamera = Camera.main;
            minWorldPos = mainCamera.ViewportToWorldPoint(_asteroidGameSettings.minViewportPosition);
            maxWorldPos = mainCamera.ViewportToWorldPoint(_asteroidGameSettings.maxViewportPosition);

            RegisterPlayerTransform();

            return UniTask.CompletedTask;
        }

        // HACK: To be replaced with on player spawned
        private void RegisterPlayerTransform()
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            RegisterTransform(player);
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
