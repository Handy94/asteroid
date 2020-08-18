namespace Asteroid
{
    using UnityEngine;

    public class EnemyComponent : MonoBehaviour
    {
        public EnemyData EnemyData { get; private set; }

        public void SetData(EnemyData enemyData)
        {
            EnemyData = enemyData;
        }
    }

}
