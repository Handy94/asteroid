namespace Asteroid
{
    using UnityEngine;

    public class EnemyComponent : MonoBehaviour
    {
        private EnemyData _enemyData;

        public void SetData(EnemyData enemyData)
        {
            _enemyData = enemyData;
        }
    }

}
