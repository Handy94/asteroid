#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Asteroid.Editor
{
    public static class EditorUtilities
    {
        public static List<T> GetAllAssets<T>() where T : Object
        {
            return AssetDatabase.FindAssets($"t:{typeof(T).Name}")
                                .Select(x => AssetDatabase.GUIDToAssetPath(x))
                                .Select(x => AssetDatabase.LoadAssetAtPath(x, typeof(T)))
                                .Cast<T>()
                                .ToList<T>();
        }
    }
}

#endif
