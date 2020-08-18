using System.Collections.Generic;
using UnityEngine;

namespace Asteroid
{
    [System.Serializable]
    public class AsteroidGameAssetSource
    {
        [Header("Asteroid")]
        public List<AsteroidData> asteroidSpawnVariants;
        public List<AsteroidSplitData> asteroidSplitDataList;

        [Header("Stage")]
        public List<StageWaveData> stageWaveDataList;

        [Header("Enemy")]
        public List<EnemyData> enemyDataList;

        public AsteroidData GetAsteroidData(string asteroidID)
        {
            return asteroidSpawnVariants.Find(x => x.AsteroidID.Equals(asteroidID));
        }

        public AsteroidSplitData GetAsteroidSplitData(string asteroidID)
        {
            return asteroidSplitDataList.Find(x => x.AsteroidIDSource.Equals(asteroidID));
        }

        public StageWaveData GetStageWaveData(int stage)
        {
            if (stage < 1) throw new System.ArgumentException("Stage must be greater than 0");
            if (stage > stageWaveDataList.Count) stage = stageWaveDataList.Count;

            return stageWaveDataList[stage - 1];
        }

        public EnemyData GetEnemyData(string enemyID)
        {
            return enemyDataList.Find(x => x.EnemyID.Equals(enemyID));
        }
    }

}
