using System.Collections.Generic;
using UnityEngine;

namespace Asteroid
{
    [CreateAssetMenu(menuName = "@Asteroid/Stage Wave Data", fileName = "New StageWaveData")]
    public class StageWaveData : ScriptableObject
    {
        public List<StageAsteroidData> spawnAsteroidDataList;
    }

}
