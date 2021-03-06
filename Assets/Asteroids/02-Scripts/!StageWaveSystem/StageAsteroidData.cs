﻿using UnityEngine;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
using System.Collections;
using System.Linq;
using Asteroid.Editor;
#endif

namespace Asteroid
{
    [System.Serializable]
    public struct StageAsteroidData
    {
#if UNITY_EDITOR
        [ValueDropdown("GetAllAsteroidID")]
#endif
        [SerializeField] private string asteroidID;
        [SerializeField] private int minSpawnCount;
        [SerializeField] private int maxSpawnCount;

        public string AsteroidID => asteroidID;
        public int MinSpawnCount => minSpawnCount;
        public int MaxSpawnCount => maxSpawnCount;

#if UNITY_EDITOR
        private IEnumerable GetAllAsteroidID()
        {
            return EditorUtilities.GetAllAssets<AsteroidData>().Select(x => x.name);
        }
#endif
    }

}
