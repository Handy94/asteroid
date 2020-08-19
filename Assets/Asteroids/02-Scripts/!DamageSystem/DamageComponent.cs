namespace Asteroid
{
    using UnityEngine;

    public class DamageComponent : MonoBehaviour, IDamager<GameEntityTag>
    {
        [SerializeField] private GameEntityTagComponent _gameEntityTagComponent;
        [SerializeField] private int damage = 1;

        public GameEntityTag DamagerType => _gameEntityTagComponent.GameEntityTag;

        private void Reset()
        {
            if (_gameEntityTagComponent == null) _gameEntityTagComponent = GetComponent<GameEntityTagComponent>();
        }

        private void Awake()
        {
            if (_gameEntityTagComponent == null) _gameEntityTagComponent = GetComponent<GameEntityTagComponent>();
        }

        public int GetDamage()
        {
            return damage;
        }
    }

}
