using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using UnityEngine;

namespace Asteroid
{
    [Category("@@-Handy Package/Enemy")]
    [Name("Enemy Wandering State")]
    public class EnemyWanderingState : FSMState
    {
        public BBParameter<EnemySaucerMovement> enemySaucerMovement;
        public BBParameter<BasicShipWeapon> enemyWeapon;
        public BBParameter<WeaponAimer> weaponAimer;

        public float shootWeaponInterval = 5f;
        public Vector2 minRandomViewportPosition;
        public Vector2 maxRandomViewportPosition;

        private Camera _mainCamera;
        private float _shootTimer = 0f;

        protected override void OnInit()
        {
            base.OnInit();
            _mainCamera = Camera.main;
        }

        protected override void OnEnter()
        {
            base.OnEnter();

            SetupRandomPosition();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (enemySaucerMovement.value.IsArriveAtTarget())
            {
                SetupRandomPosition();
            }
            if (_shootTimer < shootWeaponInterval)
            {
                _shootTimer += Time.deltaTime;
            }
            else
            {
                _shootTimer = 0f;
                ShootToRandomPoint();
            }
        }

        private void SetupRandomPosition()
        {
            if (_mainCamera == null) _mainCamera = Camera.main;

            Vector2 randomViewportPosition = Vector2.zero;
            randomViewportPosition.x = Random.Range(minRandomViewportPosition.x, maxRandomViewportPosition.x);
            randomViewportPosition.y = Random.Range(minRandomViewportPosition.y, maxRandomViewportPosition.y);

            Vector2 randomWorldPos = _mainCamera.ViewportToWorldPoint(randomViewportPosition);
            enemySaucerMovement.value.SetTargetPosition(randomWorldPos);
        }

        private void ShootToRandomPoint()
        {
            Vector2 additionalRandomDirection = Vector2.zero;
            additionalRandomDirection.x = Random.Range(-1f, 1f);
            additionalRandomDirection.y = Random.Range(-1f, 1f);

            Vector2 shootDirection = (Vector2)weaponAimer.value.transform.position + additionalRandomDirection;
            weaponAimer.value.AimToTarget(shootDirection);

            enemyWeapon.value.Shoot();
        }
    }

}
