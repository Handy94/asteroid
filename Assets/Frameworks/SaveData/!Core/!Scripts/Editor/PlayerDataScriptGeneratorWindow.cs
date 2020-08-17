#if UNITY_EDITOR
namespace HandyPackage.Editor
{
    using System.Linq;
    using System.IO;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using Sirenix.Utilities.Editor;
    using Newtonsoft.Json;
    using Asteroid;

    public class PlayerDataScriptGeneratorWindow : EditorWindow
    {
        private PlayerDataEditorData addData = new PlayerDataEditorData();
        private List<PlayerDataEditorData> cachedPlayerEditorData = new List<PlayerDataEditorData>();

        private TextAsset playerEditorDataJson;
        private GUITable guiTablePlayerEditorData;
        private Vector2 scrollPos;

        private Paging<PlayerDataEditorData> _pagingTable;
        private Paging<PlayerDataEditorData> PagingTable
        {
            get
            {
                if (_pagingTable == null)
                    _pagingTable = new Paging<PlayerDataEditorData>(10, cachedPlayerEditorData);
                return _pagingTable;
            }
        }

        private bool useFolderPath = true;
        private string generatedScriptFolderTarget;
        private TextAsset generatedScriptPlayerData;
        private TextAsset generatedScriptPlayerDataGetter;
        private TextAsset generatedScriptPlayerDataKey;
        private TextAsset generatedScriptPlayerDataKeyValueGetter;
        private TextAsset generatedScriptPlayerDataLoader;
        private TextAsset generatedScriptPlayerDataMutator;
        private TextAsset generatedScriptPlayerDataSubscribe;
        private TextAsset generatedScriptPlayerDataTrMapper;
        private TextAsset generatedScriptPlayerDataSaver;

        private string playerDataLoaderPath;
        private string playerDataSubscribeManagerPath;
        private string playerTransactionResolverMapPath;

        private const int COLUMN_REMOVE_WIDTH = 30;
        private const int COLUMN_TOGGLE_WIDTH = 70;

        #region EditorWindow
        [MenuItem("HandyPackage/PlayerDataEditor")]
        static void OpenWindow()
        {
            var window = EditorWindow.GetWindow(typeof(PlayerDataScriptGeneratorWindow), false, "PlayerDataScriptGenerator") as PlayerDataScriptGeneratorWindow;
            window.Show();
        }

        void OnGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            EditorGUILayout.BeginVertical();

            DrawHeader();
            DrawAddData();
            DrawCachedPlayerEditorData();
            DrawGenerateScript();

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
        #endregion

        #region Editor Specific Methods
        private void DrawHeader()
        {
            EditorGUILayout.LabelField("Player Data Editor", EditorStyles.boldLabel);

            playerEditorDataJson = (TextAsset)EditorGUILayout.ObjectField("Player Data Config", playerEditorDataJson, typeof(TextAsset), false);
            if (GUILayout.Button("Save Config"))
            {
                SaveConfig();
            }
            if (GUILayout.Button("Load Config"))
            {
                if (playerEditorDataJson != null)
                {
                    cachedPlayerEditorData = JsonConvert.DeserializeObject<List<PlayerDataEditorData>>(playerEditorDataJson.text);
                    RefreshDataTable();
                }
            }
        }

        private void SaveConfig()
        {
            string path = string.Empty;
            if (playerEditorDataJson == null)
            {
                path = EditorUtility.SaveFilePanelInProject("Player Editor Data JSON", "playerDataConfig", "json", string.Empty);
            }
            else
            {
                path = AssetDatabase.GetAssetPath(playerEditorDataJson);
            }
            if (!string.IsNullOrEmpty(path))
            {
                string jsonContent = JsonConvert.SerializeObject(cachedPlayerEditorData);
                File.WriteAllText(path, jsonContent);
                AssetDatabase.Refresh();
                playerEditorDataJson = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
            }
        }

        private void DrawAddData()
        {
            if (EditorUtilities.DrawFoldout("Add New Save Key", addData))
            {
                EditorGUILayout.BeginVertical(EditorStyles.textArea);
                DrawPlayerEditorData(addData, true);
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextField("Final Key Result", PlayerDataEditorUtilities.EvaluatePlayerDataEditorKey(addData));
                EditorGUI.EndDisabledGroup();
                if (GUILayout.Button("Add New Save Key"))
                {
                    addData.key = PlayerDataEditorUtilities.EvaluatePlayerDataEditorKey(addData);
                    cachedPlayerEditorData.Add((PlayerDataEditorData)addData.Clone());
                    RefreshDataTable(false);
                }
                EditorGUILayout.EndVertical();
            }
        }

        private void DrawCachedPlayerEditorData()
        {
            if (guiTablePlayerEditorData == null)
                RefreshDataTable();


            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            PagingTable.AmountToShowPerPage = EditorGUILayout.IntField("Show Count (Pagination)", PagingTable.AmountToShowPerPage);
            if (EditorGUI.EndChangeCheck())
            {
                RefreshDataTable(false);
            }

            if (GUILayout.Button("<"))
            {
                PagingTable.PageIndex -= 1;
                RefreshDataTable(false);
            }

            EditorGUILayout.LabelField($"{PagingTable.PageIndex + 1}/{PagingTable.MaxPage()}");

            if (GUILayout.Button(">"))
            {
                PagingTable.PageIndex += 1;
                RefreshDataTable(false);
            }
            EditorGUILayout.EndHorizontal();

            guiTablePlayerEditorData.DrawTable();

            if (GUILayout.Button("Refresh Table"))
            {
                RefreshDataTable();
            }
        }

        private void RefreshDataTable(bool resetPaging = true)
        {
            if (resetPaging) PagingTable.Collections = this.cachedPlayerEditorData;
            var data = PagingTable.GetItems().ToList();
            this.guiTablePlayerEditorData = GUITable.Create(data, string.Empty,
                new GUITableColumn()
                {
                    ColumnTitle = "Save Key",
                    OnGUI = (rect, i) => { DrawPlayerEditorDataKey(data[i], false, rect); },
                },
                new GUITableColumn()
                {
                    ColumnTitle = "Save Data Type",
                    OnGUI = (rect, i) =>
                    {
                        DrawPlayerEditorDataBaseType(data[i], false, rect);
                    },
                },
                new GUITableColumn()
                {
                    ColumnTitle = "Key Type",
                    OnGUI = (rect, i) =>
                    {
                        DrawPlayerEditorDataKeyType(data[i], false, rect);
                    },
                },
                new GUITableColumn()
                {
                    ColumnTitle = "Value Type",
                    OnGUI = (rect, i) =>
                    {
                        DrawPlayerEditorDataValueType(data[i], false, rect);
                    },
                },
                new GUITableColumn()
                {
                    ColumnTitle = "Is Local Only",
                    OnGUI = (rect, i) =>
                    {
                        DrawPlayerEditorIsLocalOnly(data[i], false, EditorStyles.miniButtonMid, rect);
                    },
                    Width = COLUMN_TOGGLE_WIDTH
                },
                new GUITableColumn()
                {
                    ColumnTitle = "Getter",
                    OnGUI = (rect, i) =>
                    {
                        DrawPlayerEditorShouldGenerateGetter(data[i], false, EditorStyles.miniButtonMid, rect);
                    },
                    Width = COLUMN_TOGGLE_WIDTH
                },
                new GUITableColumn()
                {
                    ColumnTitle = "Mutator",
                    OnGUI = (rect, i) =>
                    {
                        DrawPlayerEditorShouldGenerateMutator(data[i], false, EditorStyles.miniButtonMid, rect);
                    },
                    Width = COLUMN_TOGGLE_WIDTH
                },
                new GUITableColumn()
                {
                    ColumnTitle = "Subscribe",
                    OnGUI = (rect, i) =>
                    {
                        DrawPlayerEditorShouldGenerateSubscribe(data[i], false, EditorStyles.miniButtonMid, rect);
                    },
                    Width = COLUMN_TOGGLE_WIDTH
                },
                new GUITableColumn()
                {
                    ColumnTitle = string.Empty,
                    OnGUI = (rect, i) =>
                    {
                        if (GUI.Button(rect, "X"))
                        {
                            cachedPlayerEditorData.RemoveAt(i);
                            RefreshDataTable(false);
                        }
                    },
                    Width = COLUMN_REMOVE_WIDTH
                }
            );
        }

        private void DrawGenerateScript()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Code Generation");

            useFolderPath = EditorGUILayout.Toggle("Use Folder Path", useFolderPath);

            if (!useFolderPath)
            {
                generatedScriptPlayerData = (TextAsset)EditorGUILayout.ObjectField("PlayerData Script", generatedScriptPlayerData, typeof(TextAsset), false);
                generatedScriptPlayerDataGetter = (TextAsset)EditorGUILayout.ObjectField("PlayerDataGetter Script", generatedScriptPlayerDataGetter, typeof(TextAsset), false);
                generatedScriptPlayerDataKey = (TextAsset)EditorGUILayout.ObjectField("PlayerDataKey Script", generatedScriptPlayerDataKey, typeof(TextAsset), false);
                generatedScriptPlayerDataKeyValueGetter = (TextAsset)EditorGUILayout.ObjectField("PlayerDataKeyValueGetter Script", generatedScriptPlayerDataKeyValueGetter, typeof(TextAsset), false);
                generatedScriptPlayerDataLoader = (TextAsset)EditorGUILayout.ObjectField("PlayerDataLoader Script", generatedScriptPlayerDataLoader, typeof(TextAsset), false);
                generatedScriptPlayerDataMutator = (TextAsset)EditorGUILayout.ObjectField("PlayerDataMutator Script", generatedScriptPlayerDataMutator, typeof(TextAsset), false);
                generatedScriptPlayerDataSubscribe = (TextAsset)EditorGUILayout.ObjectField("PlayerDataSubscribeManager Script", generatedScriptPlayerDataSubscribe, typeof(TextAsset), false);
                generatedScriptPlayerDataTrMapper = (TextAsset)EditorGUILayout.ObjectField("PlayerTransactionResolverMap Script", generatedScriptPlayerDataTrMapper, typeof(TextAsset), false);
                generatedScriptPlayerDataSaver = (TextAsset)EditorGUILayout.ObjectField("PlayerDataSaver Script", generatedScriptPlayerDataSaver, typeof(TextAsset), false);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextField("Generated Script Folder Path", generatedScriptFolderTarget);
                EditorGUI.EndDisabledGroup();
                if (GUILayout.Button("Choose Folder Path"))
                {
                    generatedScriptFolderTarget = EditorUtility.OpenFolderPanel("Generated Script Folder Path", generatedScriptFolderTarget, Application.dataPath);
                }
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Generate Scripts"))
            {
                SaveConfig();

                string pathPlayerData = GetGeneratedScriptPath(generatedScriptPlayerData, typeof(PlayerData));
                string pathPlayerDataGetter = GetGeneratedScriptPath(generatedScriptPlayerDataGetter, typeof(PlayerDataGetter));
                string pathPlayerDataKey = GetGeneratedScriptPath(generatedScriptPlayerDataKey, typeof(PlayerDataKeys));
                string pathPlayerDataKeyValueGetter = GetGeneratedScriptPath(generatedScriptPlayerDataKeyValueGetter, typeof(PlayerDataKeyValueGetter));
                string pathPlayerDataLoader = GetGeneratedScriptPath(generatedScriptPlayerDataLoader, typeof(PlayerDataLoader));
                string pathPlayerDataMutator = GetGeneratedScriptPath(generatedScriptPlayerDataMutator, typeof(PlayerDataMutator));
                string pathPlayerDataSubscribe = GetGeneratedScriptPath(generatedScriptPlayerDataSubscribe, typeof(PlayerDataSubscribeManager));
                string pathPlayerDataSaver = GetGeneratedScriptPath(generatedScriptPlayerDataSaver, typeof(PlayerDataSaver));

                string findFilePath = Path.Combine(Application.dataPath, "Frameworks");
                List<string> allFiles = Directory.GetFiles(findFilePath, "*.cs", SearchOption.AllDirectories).ToList();

                string coreFileContent = string.Empty;
                playerDataLoaderPath = allFiles.Find(x => Path.GetFileName(x).Equals(typeof(PlayerDataLoader).Name + ".cs"));
                playerDataSubscribeManagerPath = allFiles.Find(x => Path.GetFileName(x).Equals(typeof(PlayerDataSubscribeManager).Name + ".cs"));

                bool isValid = true;
                if (string.IsNullOrEmpty(playerDataLoaderPath))
                {
                    Debug.LogError($"{typeof(PlayerDataLoader).Name} core file not found");
                    isValid = false;
                }

                if (string.IsNullOrEmpty(playerDataSubscribeManagerPath))
                {
                    Debug.LogError($"{typeof(PlayerDataSubscribeManager).Name} core file not found");
                    isValid = false;
                }

                // data loader
                coreFileContent = File.ReadAllText(playerDataLoaderPath);
                File.WriteAllText(playerDataLoaderPath, PlayerDataCodeModifier.ModifyPlayerDataLoader(
                    coreFileContent, PlayerDataCodeGeneratorConstants.LOADER_METHOD_NAME + GetCategoryNameFromFile(pathPlayerDataLoader) + "();"));

                // subscribe manager
                coreFileContent = File.ReadAllText(playerDataSubscribeManagerPath);
                File.WriteAllText(playerDataSubscribeManagerPath, PlayerDataCodeModifier.ModifyPlayerDataSubscribeManager(
                    coreFileContent, PlayerDataCodeGeneratorConstants.SUBSCRIBE_METHOD_NAME + GetCategoryNameFromFile(pathPlayerDataSubscribe) + "();"));

                if (!string.IsNullOrEmpty(pathPlayerData)) File.WriteAllText(pathPlayerData, PlayerDataCodeGenerator.GeneratePlayerDataScript(cachedPlayerEditorData));
                if (!string.IsNullOrEmpty(pathPlayerDataGetter)) File.WriteAllText(pathPlayerDataGetter, PlayerDataCodeGenerator.GeneratePlayerDataGetterScript(cachedPlayerEditorData));
                if (!string.IsNullOrEmpty(pathPlayerDataKey)) File.WriteAllText(pathPlayerDataKey, PlayerDataCodeGenerator.GeneratePlayerDataKeysScript(cachedPlayerEditorData));
                if (!string.IsNullOrEmpty(pathPlayerDataKeyValueGetter)) File.WriteAllText(pathPlayerDataKeyValueGetter, PlayerDataCodeGenerator.GeneratePlayerDataKeyValueGetterScript(cachedPlayerEditorData, GetCategoryNameFromFile(pathPlayerDataKeyValueGetter)));
                if (!string.IsNullOrEmpty(pathPlayerDataLoader)) File.WriteAllText(pathPlayerDataLoader, PlayerDataCodeGenerator.GeneratePlayerDataLoaderScript(cachedPlayerEditorData, GetCategoryNameFromFile(pathPlayerDataLoader)));
                if (!string.IsNullOrEmpty(pathPlayerDataMutator)) File.WriteAllText(pathPlayerDataMutator, PlayerDataCodeGenerator.GeneratePlayerDataMutatorScript(cachedPlayerEditorData));
                if (!string.IsNullOrEmpty(pathPlayerDataSubscribe)) File.WriteAllText(pathPlayerDataSubscribe, PlayerDataCodeGenerator.GeneratePlayerDataSubscribeManagerScript(cachedPlayerEditorData, GetCategoryNameFromFile(pathPlayerDataSubscribe)));
                if (!string.IsNullOrEmpty(pathPlayerDataSaver)) File.WriteAllText(pathPlayerDataSaver, PlayerDataCodeGenerator.GeneratePlayerDataSaverScript(cachedPlayerEditorData, GetCategoryNameFromFile(pathPlayerDataSaver)));

                AssetDatabase.Refresh();
            }
            EditorGUILayout.EndVertical();
        }

        private string GetGeneratedScriptPath(TextAsset overrideTextAsset, System.Type type)
        {
            string pathGeneratedScript = useFolderPath && generatedScriptFolderTarget != null ? Path.Combine(generatedScriptFolderTarget, $"{type.Name}.Generated.{new DirectoryInfo(generatedScriptFolderTarget).Name}.cs")
                                                                                                    : (overrideTextAsset != null) ? AssetDatabase.GetAssetPath(overrideTextAsset) : string.Empty;
            return pathGeneratedScript;
        }

        private string GetCategoryNameFromFile(string filename)
        {
            string fileNameWithoutExt = new FileInfo(filename).Name.Replace(".cs", string.Empty);
            int lastIndexOfDot = fileNameWithoutExt.LastIndexOf('.');
            string categoryName = fileNameWithoutExt.Substring(lastIndexOfDot + 1);
            return categoryName;
        }
        #endregion

        #region Reusable Methods
        private void DrawPlayerEditorData(PlayerDataEditorData data, bool useLabel)
        {
            if (data == null) data = new PlayerDataEditorData();

            DrawPlayerEditorDataKey(data, useLabel);
            DrawPlayerEditorDataBaseType(data, useLabel);

            EditorGUI.indentLevel++;
            DrawPlayerEditorDataKeyType(data, useLabel);
            DrawPlayerEditorDataValueType(data, useLabel);
            EditorGUI.indentLevel--;

            DrawPlayerEditorIsLocalOnly(data, useLabel, EditorStyles.toggle);
            DrawPlayerEditorShouldGenerateGetter(data, useLabel, EditorStyles.toggle);
            DrawPlayerEditorShouldGenerateMutator(data, useLabel, EditorStyles.toggle);
            DrawPlayerEditorShouldGenerateSubscribe(data, useLabel, EditorStyles.toggle);
        }

        private void DrawPlayerEditorDataKey(PlayerDataEditorData data, bool useLabel, Rect rect = default(Rect))
        {
            string labelKey = (useLabel) ? "Save Key" : string.Empty;
            if (rect == default(Rect))
            {
                data.key = EditorGUILayout.TextField(labelKey, data.key);
            }
            else
            {
                data.key = EditorGUI.TextField(rect, labelKey, data.key);
            }
            if (!string.IsNullOrEmpty(data.key)) data.key = data.key.ToUpper();
        }

        private void DrawPlayerEditorDataBaseType(PlayerDataEditorData data, bool useLabel, Rect rect = default(Rect))
        {
            string labelBaseDataType = (useLabel) ? "Data Type" : string.Empty;

            EditorGUI.BeginChangeCheck();
            data.baseDataType = EditorUtilities.DrawStringPopup(labelBaseDataType, data.baseDataType, PlayerDataEditorStaticData.supportedDataType, rect);
            if (EditorGUI.EndChangeCheck())
            {
                if (!PlayerDataEditorStaticData.DICTIONARY_DATA_TYPE.Equals(data.baseDataType))
                {
                    data.keyDataType = string.Empty;
                    if (!PlayerDataEditorStaticData.LIST_DATA_TYPE.Equals(data.baseDataType))
                    {
                        data.valueDataType = string.Empty;
                    }
                }
                data.key = PlayerDataEditorUtilities.EvaluatePlayerDataEditorKey(data);
            }
        }

        private void DrawPlayerEditorDataKeyType(PlayerDataEditorData data, bool useLabel, Rect rect = default(Rect))
        {
            string labelKeyDataType = (useLabel) ? "Key Data Type" : string.Empty;

            if (PlayerDataEditorStaticData.DICTIONARY_DATA_TYPE.Equals(data.baseDataType))
            {
                EditorGUI.BeginChangeCheck();
                data.keyDataType = EditorUtilities.DrawStringPopup(labelKeyDataType, data.keyDataType, PlayerDataEditorStaticData.keySupportedDataType, rect);
                if (EditorGUI.EndChangeCheck())
                {
                    data.key = PlayerDataEditorUtilities.EvaluatePlayerDataEditorKey(data);
                }
            }
        }

        private void DrawPlayerEditorDataValueType(PlayerDataEditorData data, bool useLabel, Rect rect = default(Rect))
        {
            string labelValueDataType = (useLabel) ? "Value Data Type" : string.Empty;

            if (PlayerDataEditorStaticData.LIST_DATA_TYPE.Equals(data.baseDataType)
            || PlayerDataEditorStaticData.DICTIONARY_DATA_TYPE.Equals(data.baseDataType))
            {
                EditorGUI.BeginChangeCheck();
                data.valueDataType = EditorUtilities.DrawStringPopup(labelValueDataType, data.valueDataType, PlayerDataEditorStaticData.valueSupportedDataType, rect);
                {
                    data.key = PlayerDataEditorUtilities.EvaluatePlayerDataEditorKey(data);
                }
            }
        }

        private void DrawPlayerEditorBoolValue(string label, ref bool value, Rect rect = default(Rect))
        {
            DrawPlayerEditorBoolValue(label, ref value, EditorStyles.toggle, rect);
        }

        private void DrawPlayerEditorBoolValue(string label, ref bool value, GUIStyle guiStyle, Rect rect = default(Rect))
        {
            if (rect == default(Rect))
            {
                value = EditorGUILayout.Toggle(label, value, guiStyle);
            }
            else
            {
                var originBgColor = GUI.backgroundColor;
                if (value)
                {
                    GUI.backgroundColor = Color.green;
                }
                else
                {
                    GUI.backgroundColor = Color.red;
                }
                value = EditorGUI.Toggle(rect, label, value, guiStyle);
                GUI.backgroundColor = originBgColor;
            }
        }

        private void DrawPlayerEditorIsLocalOnly(PlayerDataEditorData data, bool useLabel, GUIStyle guiStyle, Rect rect = default(Rect))
        {
            string label = (useLabel) ? "Is Local Only" : string.Empty;
            DrawPlayerEditorBoolValue(label, ref data.isLocalOnly, guiStyle, rect);
        }

        private void DrawPlayerEditorShouldGenerateGetter(PlayerDataEditorData data, bool useLabel, GUIStyle guiStyle, Rect rect = default(Rect))
        {
            string label = (useLabel) ? "Generate Getter" : string.Empty;
            DrawPlayerEditorBoolValue(label, ref data.shouldGenerateGetter, guiStyle, rect);
        }

        private void DrawPlayerEditorShouldGenerateMutator(PlayerDataEditorData data, bool useLabel, GUIStyle guiStyle, Rect rect = default(Rect))
        {
            string label = (useLabel) ? "Generate Mutator" : string.Empty;
            DrawPlayerEditorBoolValue(label, ref data.shouldGenerateMutator, guiStyle, rect);
        }

        private void DrawPlayerEditorShouldGenerateSubscribe(PlayerDataEditorData data, bool useLabel, GUIStyle guiStyle, Rect rect = default(Rect))
        {
            string label = (useLabel) ? "Generate Subscribe" : string.Empty;
            DrawPlayerEditorBoolValue(label, ref data.shouldGenerateForSubscribe, guiStyle, rect);
        }
        #endregion
    }

}
#endif
