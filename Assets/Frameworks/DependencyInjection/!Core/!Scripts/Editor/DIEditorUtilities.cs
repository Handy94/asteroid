#if UNITY_EDITOR
namespace HandyPackage.Editor
{
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public static class DIEditorUtilities
    {
        public static string AddCSharpClassTemplate(
            string friendlyName, string defaultFileName, string templateStr)
        {
            return AddCSharpClassTemplate(
                friendlyName, defaultFileName, templateStr, GetCurrentDirectoryAssetPathFromSelection());
        }

        public static string AddCSharpClassTemplate(
            string friendlyName, string defaultFileName,
            string templateStr, string folderPath)
        {
            var absolutePath = EditorUtility.SaveFilePanel(
                "Choose name for " + friendlyName,
                folderPath,
                defaultFileName + ".cs",
                "cs");

            if (absolutePath == "")
            {
                // Dialog was cancelled
                return null;
            }

            if (!absolutePath.ToLower().EndsWith(".cs"))
            {
                absolutePath += ".cs";
            }

            var className = Path.GetFileNameWithoutExtension(absolutePath);
            File.WriteAllText(absolutePath, templateStr.Replace("CLASS_NAME", className));

            AssetDatabase.Refresh();

            var assetPath = ConvertFullAbsolutePathToAssetPath(absolutePath);

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);

            return assetPath;
        }

        public static string ConvertFullAbsolutePathToAssetPath(string fullPath)
        {
            fullPath = Path.GetFullPath(fullPath);

            var assetFolderFullPath = Path.GetFullPath(Application.dataPath);

            if (fullPath.Length == assetFolderFullPath.Length)
            {
                return "Assets";
            }

            var assetPath = fullPath.Remove(0, assetFolderFullPath.Length + 1).Replace("\\", "/");
            return "Assets/" + assetPath;
        }

        public static string GetCurrentDirectoryAssetPathFromSelection()
        {
            return ConvertFullAbsolutePathToAssetPath(
                GetCurrentDirectoryAbsolutePathFromSelection());
        }

        public static string GetCurrentDirectoryAbsolutePathFromSelection()
        {
            var folderPath = TryGetSelectedFolderPathInProjectsTab();

            if (folderPath != null)
            {
                return folderPath;
            }

            var filePath = TryGetSelectedFilePathInProjectsTab();

            if (filePath != null)
            {
                return Path.GetDirectoryName(filePath);
            }

            return Application.dataPath;
        }

        public static string TryGetSelectedFilePathInProjectsTab()
        {
            return GetSelectedFilePathsInProjectsTab().FirstOrDefault();
        }

        public static List<string> GetSelectedFilePathsInProjectsTab()
        {
            return GetSelectedPathsInProjectsTab()
                .Where(x => File.Exists(x)).ToList();
        }

        public static List<string> GetSelectedAssetPathsInProjectsTab()
        {
            var paths = new List<string>();

            UnityEngine.Object[] selectedAssets = Selection.GetFiltered(
                typeof(UnityEngine.Object), SelectionMode.Assets);

            foreach (var item in selectedAssets)
            {
                var assetPath = AssetDatabase.GetAssetPath(item);

                if (!string.IsNullOrEmpty(assetPath))
                {
                    paths.Add(assetPath);
                }
            }

            return paths;
        }

        public static List<string> GetSelectedPathsInProjectsTab()
        {
            var paths = new List<string>();

            UnityEngine.Object[] selectedAssets = Selection.GetFiltered(
                typeof(UnityEngine.Object), SelectionMode.Assets);

            foreach (var item in selectedAssets)
            {
                var relativePath = AssetDatabase.GetAssetPath(item);

                if (!string.IsNullOrEmpty(relativePath))
                {
                    var fullPath = Path.GetFullPath(Path.Combine(
                        Application.dataPath, Path.Combine("..", relativePath)));

                    paths.Add(fullPath);
                }
            }

            return paths;
        }

        // Note that the path is relative to the Assets folder
        public static List<string> GetSelectedFolderPathsInProjectsTab()
        {
            return GetSelectedPathsInProjectsTab()
                .Where(x => Directory.Exists(x)).ToList();
        }

        // Returns the best guess directory in projects pane
        // Useful when adding to Assets -> Create context menu
        // Returns null if it can't find one
        // Note that the path is relative to the Assets folder for use in AssetDatabase.GenerateUniqueAssetPath etc.
        public static string TryGetSelectedFolderPathInProjectsTab()
        {
            return GetSelectedFolderPathsInProjectsTab().FirstOrDefault();
        }
    }
}
#endif
