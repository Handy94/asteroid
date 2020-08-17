using System.Collections.Generic;

namespace Asteroid
{
    [System.Serializable]
    public class AsteroidAssetSource
    {
        public List<AsteroidData> asteroidSpawnVariants;
        public List<AsteroidSplitData> asteroidSplitDataList;
        public List<StageWaveData> stageWaveDataList;

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
    }

}
