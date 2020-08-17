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
    public struct AsteroidScoreData
    {
#if UNITY_EDITOR
        [ValueDropdown("GetAllAsteroidID")]
#endif
        [SerializeField]
        private string asteroidID;
        [SerializeField] private int score;

        public string AsteroidID => asteroidID;
        public int Score => score;

#if UNITY_EDITOR
        private IEnumerable GetAllAsteroidID()
        {
            return EditorUtilities.GetAllAssets<AsteroidData>().Select(x => x.name);
        }
#endif
    }

}
