using HandyPackage;
using UnityEngine;

namespace Asteroid
{
    public class EntityHealthComponent : MonoBehaviour, IDamageable<GameEntityTag>
    {
        [SerializeField] private int Health = 1;
        [SerializeField] private int MaxHealth = 1;

        public EventSignal<int, GameEntityTag> OnHealthChanged { get; private set; } = new EventSignal<int, GameEntityTag>();

        public void RefillLive(GameEntityTag changerEntity)
        {
            SetHealth(MaxHealth, changerEntity);
        }

        public void SetHealth(int health, GameEntityTag changerEntity)
        {
            Health = health;
            if (Health > MaxHealth) Health = MaxHealth;
            if (Health < 0) Health = 0;
            OnHealthChanged.Fire(Health, changerEntity);
        }

        public void Damage(int damage, GameEntityTag damager)
        {
            SetHealth(Health - damage, damager);
        }
    }

}
