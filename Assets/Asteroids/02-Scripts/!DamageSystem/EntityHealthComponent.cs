using HandyPackage;
using UnityEngine;

namespace Asteroid
{
    public class EntityHealthComponent : MonoBehaviour, IDamageable<GameEntityTag>
    {
        [SerializeField] private int Health;

        public EventSignal<int, GameEntityTag> OnHealthChanged { get; private set; } = new EventSignal<int, GameEntityTag>();

        public void SetHealth(int health, GameEntityTag changerEntity)
        {
            Health = health;
            if (Health < 0) Health = 0;
            OnHealthChanged.Fire(Health, changerEntity);
        }

        public void Damage(int damage, GameEntityTag damager)
        {
            SetHealth(Health - damage, damager);
        }
    }

}
