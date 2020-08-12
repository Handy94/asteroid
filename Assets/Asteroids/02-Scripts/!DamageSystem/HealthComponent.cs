using HandyPackage;
using UnityEngine;

namespace Asteroid
{
    public class HealthComponent : MonoBehaviour, IDamageable<GameEntityTag>
    {
        [SerializeField] private int Health;

        public EventSignal<int, GameEntityTag> OnHealthChanged { get; private set; } = new EventSignal<int, GameEntityTag>();

        public void Damage(int damage, GameEntityTag damager)
        {
            Health -= damage;
            if (Health < 0) Health = 0;

            OnHealthChanged.Fire(Health, damager);
        }
    }

}
