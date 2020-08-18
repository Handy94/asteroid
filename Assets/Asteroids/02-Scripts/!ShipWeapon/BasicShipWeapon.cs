using HandyPackage;
using UnityEngine;

namespace Asteroid
{
    public class BasicShipWeapon : MonoBehaviour, IWeapon
    {
        public GameObject bulletPrefab;
        public Transform[] bulletSpawnPoint;
        public float fireRate = 1;

        private BulletSpawnerSystem _bulletSpawnerSystem;

        private float fireTime = 0f;

        private void Awake()
        {
            _bulletSpawnerSystem = DIResolver.GetObject<BulletSpawnerSystem>();
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

        public async void Shoot()
        {
            if (CanShoot())
            {
                fireTime = 0f;
                var bullets = await _bulletSpawnerSystem.SpawnBullet(bulletPrefab, bulletSpawnPoint);
                for (int i = 0; i < bullets.Count; i++)
                {
                    bullets[i].Init(bulletSpawnPoint[i].right);
                }
            }
        }

        public void StopShoot()
        {

        }
    }

}
