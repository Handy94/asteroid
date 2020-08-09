#if UNITY_EDITOR
namespace HandyPackage.Editor
{
    using System.IO;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;

    public static class DIMenuItems
    {
        [MenuItem("GameObject/Scene Context", false, 10)]
        public static void CreateSceneContext(MenuCommand menuCommand)
        {
            var root = new GameObject("SceneContext").AddComponent<SceneContext>();
            Selection.activeGameObject = root.gameObject;

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        [MenuItem("Assets/Create/DI/Scriptable Object Installer")]
        public static void CreateScriptableObjectInstaller()
        {
            DIEditorUtilities.AddCSharpClassTemplate("Scriptable Object Installer", "NewScriptableObjectInstaller",
                  "using UnityEngine;"
                + "\nusing HandyPackage;"
                + "\n"
                + "\n[CreateAssetMenu(fileName = \"CLASS_NAME\", menuName = \"Installers/CLASS_NAME\")]"
                + "\npublic class CLASS_NAME : ScriptableObjectInstaller"
                + "\n{"
                + "\n    public override void InstallDependencies()"
                + "\n    {"
                + "\n    }"
                + "\n}");
        }

        [MenuItem("Assets/Create/DI/Mono Installer")]
        public static void CreateMonoInstaller()
        {
            DIEditorUtilities.AddCSharpClassTemplate("Mono Installer", "NewMonoInstaller",
                  "using UnityEngine;"
                + "\nusing HandyPackage;"
                + "\n"
                + "\npublic class CLASS_NAME : MonoInstaller"
                + "\n{"
                + "\n    public override void InstallDependencies()"
                + "\n    {"
                + "\n    }"
                + "\n}");
        }

        [MenuItem("Assets/Create/DI/Installer")]
        public static void CreateInstaller()
        {
            DIEditorUtilities.AddCSharpClassTemplate("Installer", "NewInstaller",
                  "using UnityEngine;"
                + "\nusing HandyPackage;"
                + "\n"
                + "\npublic class CLASS_NAME : Installer<CLASS_NAME>"
                + "\n{"
                + "\n    public override void InstallDependencies()"
                + "\n    {"
                + "\n    }"
                + "\n}");
        }

        [MenuItem("Assets/Create/DI/Project Context")]
        public static void CreateProjectContext()
        {
            var absoluteDir = DIEditorUtilities.GetCurrentDirectoryAssetPathFromSelection();

            CreateProjectContextInternal(absoluteDir);
        }

        static void CreateProjectContextInternal(string absoluteDir)
        {
            var assetPath = DIEditorUtilities.ConvertFullAbsolutePathToAssetPath(absoluteDir);
            var prefabPath = Path.Combine(assetPath, "NewProjectContext.prefab").Replace("\\", "/");

            var gameObject = new GameObject();

            try
            {
                gameObject.AddComponent<ProjectContext>();

#if UNITY_2018_3_OR_NEWER
                var prefabObj = PrefabUtility.SaveAsPrefabAsset(gameObject, prefabPath);
#else
                var prefabObj = PrefabUtility.ReplacePrefab(gameObject, PrefabUtility.CreateEmptyPrefab(prefabPath));
#endif

                Selection.activeObject = prefabObj;
            }
            finally
            {
                GameObject.DestroyImmediate(gameObject);
            }
        }
    }
}
#endif
