using HandyPackage;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using UnityEngine;

namespace Asteroid
{
    [Category("@@-Handy Package/Enemy")]
    [Name("Enemy Follow Player State")]
    public class EnemyFollowPlayerState : FSMState
    {
        public BBParameter<EnemySaucerMovement> enemySaucerMovement;
        public BBParameter<BasicShipWeapon> enemyWeapon;
        public BBParameter<WeaponAimer> weaponAimer;

        public float shootWeaponInterval = 5f;

        private BookKeepingInGameData _bookKeepingInGameData;

        private Transform playerTransform;
        private float _shootTimer = 0f;

        protected override void OnInit()
        {
            base.OnInit();
            _bookKeepingInGameData = DIResolver.GetObject<BookKeepingInGameData>();
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            _shootTimer = 0f;

            if (_bookKeepingInGameData.PlayerShipComponent != null) playerTransform = _bookKeepingInGameData.PlayerShipComponent.transform;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (playerTransform != null)
            {
                enemySaucerMovement.value.SetTargetPosition(playerTransform.position);
            }

            if (_shootTimer < shootWeaponInterval)
            {
                _shootTimer += Time.deltaTime;
            }
            else
            {
                _shootTimer = 0f;
                weaponAimer.value.AimToTarget(playerTransform.position);
                enemyWeapon.value.Shoot();
            }
        }

        protected override void OnExit()
        {
            base.OnExit();
        }
    }

}
