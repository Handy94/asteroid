namespace Asteroid
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(menuName = "@Asteroid/Enemy Score Data Source", fileName = "New EnemyScoreDataSource")]
    public class EnemyScoreDataSourceScriptableObject : ScriptableObject, IScoreDataSource<string>
    {
        [SerializeField] private List<EnemyScoreData> enemyScoreDataList;

        public int GetScore(string key)
        {
            return enemyScoreDataList.Find(x => x.EnemyID.Equals(key)).Score;
        }
    }

}
