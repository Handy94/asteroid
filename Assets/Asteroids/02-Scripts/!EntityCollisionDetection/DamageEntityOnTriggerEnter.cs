using HandyPackage;

namespace Asteroid
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class DamageEntityOnTriggerEnter : SerializedMonoBehaviour
    {
        [SerializeField] private IDamager<GameEntityTag> damager;

        private GameSignals _gameSignals;

        private void Awake()
        {
            _gameSignals = DIResolver.GetObject<GameSignals>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var damageAble = collision.GetComponent<IDamageable<GameEntityTag>>();
            _gameSignals.GameEntityCollisionTriggeredSignal.Fire(damageAble, damager);
        }
    }

}
