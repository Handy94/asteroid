#if UNITY_EDITOR
using Sirenix.OdinInspector;
using System.Collections;
using System.Linq;
using Asteroid.Editor;
#endif

namespace Asteroid
{
    [System.Serializable]
    public class StageAsteroidData
    {
#if UNITY_EDITOR
        [ValueDropdown("GetAllAsteroidID")]
#endif
        public string asteroidID;
        public int minSpawnCount;
        public int maxSpawnCount;

#if UNITY_EDITOR
        private IEnumerable GetAllAsteroidID()
        {
            return EditorUtilities.GetAllAssets<AsteroidData>().Select(x => x.name);
        }
#endif
    }

}
