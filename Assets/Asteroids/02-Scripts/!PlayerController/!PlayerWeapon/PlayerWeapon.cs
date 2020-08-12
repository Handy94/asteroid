using HandyPackage;
using UnityEngine;

namespace Asteroid
{
    public class PlayerWeapon : MonoBehaviour, IWeapon
    {
        public GameObject bulletPrefab;
        public Transform[] bulletSpawnPoint;
        public float fireRate = 1;

        private GameSignals _gameSignals;

        private float fireTime = 0f;

        private void Awake()
        {
            _gameSignals = DIResolver.GetObject<GameSignals>();
            fireTime = 1f;
        }

        private void Update()
        {
            if (!CanShoot())
            {
                fireTime += Time.deltaTime;
            }
        }

        private bool CanShoot()
        {
            return fireTime >= (1 / fireRate);
        }

        public void Shoot()
        {
            if (CanShoot())
            {
                fireTime = 0f;
                _gameSignals.PlayerShootSignal.Fire(bulletPrefab, bulletSpawnPoint);
            }
        }

        public void StopShoot()
        {

        }
    }

}
