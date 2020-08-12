using HandyPackage;
using UnityEngine;

namespace Asteroid
{
    public class BulletComponent : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        public float baseSpeed = 10;
        public float lifeTime = 2f;

        private MultiplePrefabMemoryPool _multiplePrefabMemoryPool;
        private GameSignals _gameSignals;

        private Vector2 _moveDirection;
        private float _currentLife = 0;
        private bool _isAlive = false;

        private void Awake()
        {
            if (rb == null) rb = GetComponent<Rigidbody2D>();
            _multiplePrefabMemoryPool = DIResolver.GetObject<MultiplePrefabMemoryPool>();
            _gameSignals = DIResolver.GetObject<GameSignals>();
        }

        private void Update()
        {
            if (!_isAlive) return;
            if (_currentLife < lifeTime)
            {
                _currentLife += Time.deltaTime;
            }
            else
            {
                Dead();
            }
        }

        private void FixedUpdate()
        {
            if (!_isAlive) return;
            rb.velocity = _moveDirection * baseSpeed;
        }

        public void Init(Vector2 dir)
        {
            _currentLife = 0;
            _isAlive = true;
            _moveDirection = dir;
        }

        private void Dead()
        {
            _isAlive = false;
            _gameSignals.BulletDespawnedSignal.Fire(this);
            _multiplePrefabMemoryPool.DespawnObject(gameObject);
        }
    }

}
