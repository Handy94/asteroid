using HandyPackage;
using UnityEngine;

namespace Asteroid
{
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        [SerializeField] private int Health;

        public EventSignal<int> OnHealthChanged { get; private set; } = new EventSignal<int>();

        public void Damage(int damage)
        {
            Health -= damage;
            if (Health < 0) Health = 0;

            OnHealthChanged.Fire(Health);
        }
    }

}
