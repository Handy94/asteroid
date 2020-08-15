using System.Collections.Generic;

namespace Asteroid
{
    [System.Serializable]
    public class AsteroidAssetSource
    {
        public List<AsteroidData> asteroidSpawnVariants;
        public List<AsteroidSplitData> asteroidSplitDataList;

        public AsteroidData GetAsteroidData(string asteroidID)
        {
            return asteroidSpawnVariants.Find(x => x.AsteroidID.Equals(asteroidID));
        }

        public AsteroidSplitData GetAsteroidSplitData(string asteroidID)
        {
            return asteroidSplitDataList.Find(x => x.asteroidIDSource.Equals(asteroidID));
        }
    }

}
