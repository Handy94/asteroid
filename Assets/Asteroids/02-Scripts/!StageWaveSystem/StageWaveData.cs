using System.Collections.Generic;
using UnityEngine;

namespace Asteroid
{
    [CreateAssetMenu(menuName = "@Asteroid/Stage Wave Data", fileName = "New StageWaveData")]
    public class StageWaveData : ScriptableObject
    {
        [SerializeField] private List<StageAsteroidData> spawnAsteroidDataList;

        public List<StageAsteroidData> SpawnAsteroidDataList => spawnAsteroidDataList;
    }

}
