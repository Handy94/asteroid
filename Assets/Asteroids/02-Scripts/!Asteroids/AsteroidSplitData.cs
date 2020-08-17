using UnityEngine;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
using System.Collections;
using System.Linq;
using Asteroid.Editor;
#endif

namespace Asteroid
{
    [CreateAssetMenu(menuName = "@Asteroid/Asteroid Split Data", fileName = "New AsteroidSplitData")]
    public class AsteroidSplitData : ScriptableObject
    {
#if UNITY_EDITOR
        [ValueDropdown("GetAllAsteroidID")]
#endif
        [SerializeField] private string asteroidIDSource;
        [SerializeField] private AsteroidSplitCountData[] splitCountData;

        public string AsteroidIDSource => asteroidIDSource;
        public AsteroidSplitCountData[] SplitCountData => splitCountData;

#if UNITY_EDITOR
        private IEnumerable GetAllAsteroidID()
        {
            return EditorUtilities.GetAllAssets<AsteroidData>().Select(x => x.name);
        }
#endif
    }
}
