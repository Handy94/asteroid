using UnityEngine;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
using System.Collections;
using System.Linq;
using Asteroid.Editor;
#endif

namespace Asteroid
{
    [System.Serializable]
    public struct AsteroidSplitCountData
    {
#if UNITY_EDITOR
        [ValueDropdown("GetAllAsteroidID")]
#endif
        [SerializeField] private string asteroidID;
        [SerializeField] private int minCount;
        [SerializeField] private int maxCount;

        public string AsteroidID => asteroidID;
        public int MinCount => minCount;
        public int MaxCount => maxCount;

#if UNITY_EDITOR
        private IEnumerable GetAllAsteroidID()
        {
            return EditorUtilities.GetAllAssets<AsteroidData>().Select(x => x.name);
        }
#endif
    }

}
