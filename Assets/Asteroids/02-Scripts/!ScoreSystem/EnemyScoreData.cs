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
    public struct EnemyScoreData
    {
#if UNITY_EDITOR
        [ValueDropdown("GetAllEnemyID")]
#endif
        [SerializeField]
        private string enemyID;
        [SerializeField] private int score;

        public string EnemyID => enemyID;
        public int Score => score;

#if UNITY_EDITOR
        private IEnumerable GetAllEnemyID()
        {
            return EditorUtilities.GetAllAssets<EnemyData>().Select(x => x.EnemyID);
        }
#endif
    }

}
