﻿namespace Asteroid
{
    using UnityEngine;

    public class AsteroidComponent : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private HealthComponent healthComponent;

        public AsteroidData AsteroidData { get; private set; }

        private Vector2 _moveDirection;
        private float _moveSpeed;
        private Vector2 _currentVelocity;

        private void Awake()
        {
            if (rb == null) rb = GetComponent<Rigidbody2D>();
            if (healthComponent == null) healthComponent = GetComponent<HealthComponent>();
        }

        private void FixedUpdate()
        {
            rb.velocity = _currentVelocity;
        }

        public void Init(Vector2 dir, AsteroidData asteroidData)
        {
            this.AsteroidData = asteroidData;
            _moveDirection = dir;
            _moveSpeed = Random.Range(asteroidData.minSpeed, asteroidData.maxSpeed);
            healthComponent.SetHealth(1);

            _currentVelocity = _moveDirection * _moveSpeed;
        }
    }

}
