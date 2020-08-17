#if UNITY_EDITOR
namespace HandyPackage
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEditor;
    public static partial class EditorUtilities
    {
        private static Dictionary<object, bool> foldoutCacheContainer = new Dictionary<object, bool>();

        public static string DrawStringPopup(string value, IEnumerable<string> options, params GUILayoutOption[] layoutOptions)
        {
            return DrawStringPopup(string.Empty, value, options, EditorStyles.popup, default(Rect), layoutOptions);
        }

        public static string DrawStringPopup(string label, string value, IEnumerable<string> options, params GUILayoutOption[] layoutOptions)
        {
            return DrawStringPopup(label, value, options, EditorStyles.popup, default(Rect), layoutOptions);
        }

        public static string DrawStringPopup(string label, string value, IEnumerable<string> options, Rect rect, params GUILayoutOption[] layoutOptions)
        {
            return DrawStringPopup(label, value, options, EditorStyles.popup, rect, layoutOptions);
        }

        public static string DrawStringPopup(string label, string value, IEnumerable<string> options, GUIStyle guiStyle, Rect rect = default(Rect), params GUILayoutOption[] layoutOptions)
        {
            List<string> optList = options.ToList();
            int currIdx = optList.IndexOf(value);

            bool isValueExist = true;
            string result = value;
            int notExistIndex = 0;

            if (currIdx == -1)
            {
                isValueExist = false;
                currIdx = notExistIndex;
                optList.Insert(notExistIndex, $"{value} (Current Value)");
            }
            int selectedIndex = 0;
            if (rect == default(Rect))
            {
                selectedIndex = EditorGUILayout.Popup(label, currIdx, optList.ToArray(), guiStyle, layoutOptions);
            }
            else
            {
                selectedIndex = EditorGUI.Popup(rect, label, currIdx, optList.ToArray(), guiStyle);
            }
            if (!isValueExist && selectedIndex == notExistIndex)
            {
                return value;
            }
            return optList[selectedIndex];
        }

        public static bool DrawFoldout(string label, object obj)
        {
            return DrawFoldout(label, obj, foldoutCacheContainer, EditorStyles.foldout);
        }

        public static bool DrawFoldout(string label, object obj, IDictionary<object, bool> foldoutCaches)
        {
            return DrawFoldout(label, obj, foldoutCaches, EditorStyles.foldout);
        }

        public static bool DrawFoldout(string label, object obj, IDictionary<object, bool> foldoutCaches, GUIStyle guiStyle)
        {
            if (obj == null) return false;
            if (!foldoutCaches.ContainsKey(obj))
                foldoutCaches.Add(obj, false);
            foldoutCaches[obj] = EditorGUILayout.Foldout(foldoutCaches[obj], label, true, guiStyle);
            return foldoutCaches[obj];
        }

        public static List<T> DrawObjectFields<T>(string label, List<T> objectList, bool allowSceneObject, params GUILayoutOption[] layoutOptions)
        where T : Object
        {
            string removeButtonLabel = "X";
            string addButtonLabel = "Add New Element";
            EditorGUILayout.BeginVertical();
            if (DrawFoldout(label, objectList, foldoutCacheContainer))
            {
                int count = objectList.Count;
                for (int i = 0; i < count; i++)
                {
                    bool isRemoved = false;
                    EditorGUILayout.BeginHorizontal();
                    objectList[i] = (T)EditorGUILayout.ObjectField(objectList[i], typeof(T), allowSceneObject, layoutOptions);
                    if (GUILayout.Button(removeButtonLabel))
                    {
                        isRemoved = true;
                    }
                    EditorGUILayout.EndHorizontal();

                    if (isRemoved)
                    {
                        objectList.RemoveAt(i);
                        i--;
                        count--;
                    }
                }
                if (GUILayout.Button(addButtonLabel))
                {
                    objectList.Add(default(T));
                }
            }
            EditorGUILayout.EndVertical();
            return objectList;
        }

        public static string DrawStringFolderPath(string fieldLabel, string path, string buttonLabel,
                                          string windowTitle, bool allowEmpty = false)
        {
            string result = path;
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField(fieldLabel, path);
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button(buttonLabel))
            {
                string tempPath = path;
                if (string.IsNullOrEmpty(tempPath))
                {
                    tempPath = Application.dataPath;
                }

                string filePath = EditorUtility.OpenFolderPanel(windowTitle, tempPath, string.Empty);
                if (string.IsNullOrEmpty(filePath) && allowEmpty)
                {
                    return string.Empty;
                }
                result = filePath;
            }
            EditorGUILayout.EndHorizontal();
            return result;
        }

        public static string DrawStringFilePath(string fieldLabel, string path, string buttonLabel,
                                          string windowTitle, string extension = "", bool allowEmpty = false)
        {
            string result = path;
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField(fieldLabel, path);
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button(buttonLabel))
            {
                string tempPath = path;
                if (string.IsNullOrEmpty(tempPath))
                {
                    tempPath = Application.dataPath;
                }
                string filePath = EditorUtility.OpenFilePanel(windowTitle, tempPath, extension);
                if (string.IsNullOrEmpty(filePath) && allowEmpty)
                {
                    return string.Empty;
                }
                result = filePath;
            }
            EditorGUILayout.EndHorizontal();
            return result;
        }
    }
}
#endif
