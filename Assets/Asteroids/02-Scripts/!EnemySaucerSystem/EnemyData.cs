namespace Asteroid
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "@Asteroid/Enemy Data", fileName = "New EnemyData")]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private EnemyComponent enemyPrefab;

        public EnemyComponent EnemyPrefab => enemyPrefab;

        public string EnemyID => this.name;
    }

}
