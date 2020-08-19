namespace Asteroid
{
    using UnityEngine;

    public class EnemyComponent : MonoBehaviour
    {
        [SerializeField] private EntityHealthComponent _entityHealthComponent;
        public EnemyData EnemyData { get; private set; }

        public void Init()
        {
            _entityHealthComponent.RefillLive(GameEntityTag.UNKNOWN);
        }

        public void SetData(EnemyData enemyData)
        {
            EnemyData = enemyData;
        }
    }

}
