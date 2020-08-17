namespace Asteroid
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(menuName = "@Asteroid/Asteroid Score Data Source", fileName = "New AsteroidScoreDataSource")]
    public class AsteroidScoreDataSourceScriptableObject : ScriptableObject, IScoreDataSource<string>
    {
        [SerializeField] private List<AsteroidScoreData> asteroidScoreDataList;

        public int GetScore(string key)
        {
            return asteroidScoreDataList.Find(x => x.AsteroidID.Equals(key)).Score;
        }
    }

}
