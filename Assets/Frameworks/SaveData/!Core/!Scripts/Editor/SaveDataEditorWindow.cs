#if UNITY_EDITOR
namespace Asteroid
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;
    using HandyPackage;
    using UnityEditor;
    using ES3Internal;
    using Newtonsoft.Json;

    public class SaveDataEditorWindow : EditorWindow
    {
        #region Constants
        private const string SAVE_DATA_EXTENSIONS = "es3,bac";
        #endregion

        private Vector2 _windowScrollPos;
        private string _saveDataPath;
        private bool? _isSaveDataPathValid;

        private Vector2 _contentTextAreaScrollPos;

        private ES3Settings _es3Settings;
        private ES3Settings Default_ES3Settings
        {
            get
            {
                if (_es3Settings == null) _es3Settings = ES3Settings.GetDefaultSettings().settings;
                return _es3Settings;
            }
        }

        private string _saveDataContent;
        private string SaveDataContent
        {
            get
            {
                return _saveDataContent;
            }
            set
            {
                _saveDataContent = value;
                GUIUtility.keyboardControl = 0;
                GUIUtility.hotControl = 0;
            }
        }

        #region EditorWindow
        [MenuItem("HandyPackage/SaveDataEditor")]
        static void Init()
        {
            var window = EditorWindow.GetWindow(typeof(SaveDataEditorWindow), false, "Save Data Editor") as SaveDataEditorWindow;
            window.InitValue();
            window.Show();
        }

        private void OnGUI()
        {
            _windowScrollPos = EditorGUILayout.BeginScrollView(_windowScrollPos);
            EditorGUILayout.BeginVertical();

            Draw_LoadSaveData();

            Draw_SaveDataContent();

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
        #endregion

        #region Init
        private void InitValue()
        {
            Action_ChooseEditorSaveDataPath();
            Action_LoadFileFromSaveDataPath();
        }
        #endregion

        #region Load Save Path
        private void Draw_LoadSaveData()
        {
            EditorGUILayout.BeginVertical(EditorStyles.textArea);
            EditorGUILayout.LabelField("Load Path", EditorStyles.centeredGreyMiniLabel);

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            _saveDataPath = EditorUtilities.DrawStringFilePath("Save Data Path", _saveDataPath, "Choose Path...", "Choose .es3 file", SAVE_DATA_EXTENSIONS);
            if (GUILayout.Button("Choose Editor Path"))
            {
                Action_ChooseEditorSaveDataPath();
            }
            EditorGUILayout.EndHorizontal();
            if (EditorGUI.EndChangeCheck())
            {
                Action_LoadFileFromSaveDataPath();
            }

            var defaultColor = GUI.color;
            GUI.color = Color.yellow;
            if (GUILayout.Button("Load File From Save Data Path"))
            {
                Action_LoadFileFromSaveDataPath();
            }
            GUI.color = defaultColor;
            if (_isSaveDataPathValid.HasValue && !_isSaveDataPathValid.Value)
            {
                EditorGUILayout.HelpBox($"File Not Found : {_saveDataPath}", MessageType.Error);
            }
            EditorGUILayout.EndVertical();
        }

        private void Action_ChooseEditorSaveDataPath()
        {
            _saveDataPath = Default_ES3Settings.FullPath;
        }

        private void Action_LoadFileFromSaveDataPath()
        {
            _isSaveDataPathValid = File.Exists(_saveDataPath);
            if (_isSaveDataPathValid.HasValue && _isSaveDataPathValid.Value)
            {
                ES3File file = new ES3File(_saveDataPath, Default_ES3Settings);
                var contentBytes = file.LoadRawBytes();

                using (MemoryStream stream = new MemoryStream(contentBytes))
                {
                    StreamReader reader = new StreamReader(ES3Stream.CreateStream(stream, Default_ES3Settings, ES3FileMode.Read));
                    SaveDataContent = reader.ReadToEnd();
                }
            }

            var obj = JsonConvert.DeserializeObject(SaveDataContent);
            SaveDataContent = JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
        #endregion

        #region Content
        private void Draw_SaveDataContent()
        {
            EditorGUILayout.BeginVertical(EditorStyles.textArea);
            EditorGUILayout.LabelField("Content Editor", EditorStyles.centeredGreyMiniLabel);

            EditorGUILayout.LabelField("File Content", EditorStyles.boldLabel);
            _contentTextAreaScrollPos = EditorGUILayout.BeginScrollView(_contentTextAreaScrollPos, GUILayout.Height(150));
            _saveDataContent = EditorGUILayout.TextArea(SaveDataContent, GUILayout.ExpandHeight(true), GUILayout.MaxWidth(this.position.width / 2));
            EditorGUILayout.EndScrollView();

            var defaultColor = GUI.color;

            if (GUILayout.Button("Copy Content To Clipboard"))
            {
                Action_CopyContentToClipboard();
            }

            if (GUILayout.Button("Save Encrypted Content to Current Path"))
            {
                Action_SaveEncryptedContentToCurrentPath();
            }

            if (GUILayout.Button("Save Encrypted Content to Editor"))
            {
                Action_SaveEncryptedContentToEditor();
            }
            GUI.color = defaultColor;

            EditorGUILayout.EndVertical();
        }

        private void Action_CopyContentToClipboard()
        {
            EditorGUIUtility.systemCopyBuffer = this.SaveDataContent;
        }

        private void Action_SaveEncryptedContentToCurrentPath()
        {
            SaveEncryptedContent(_saveDataPath);
            OpenInFileBrowser.Open(_saveDataPath);
        }

        private void Action_SaveEncryptedContentToEditor()
        {
            string path = Default_ES3Settings.FullPath;
            SaveEncryptedContent(path);
            OpenInFileBrowser.Open(path);
        }

        private void SaveEncryptedContent(string path)
        {
            var saveValue = new ES3Settings().encoding.GetBytes(this.SaveDataContent);

            var _oldSettings = new ES3Settings(ES3.EncryptionType.None, string.Empty);
            _oldSettings.location = ES3.Location.File;

            var file = new ES3File(saveValue, _oldSettings);
            ES3Settings fileSetting = new ES3Settings(path, Default_ES3Settings);
            file.settings = fileSetting;
            file.Sync();
        }
        #endregion
    }
}
#endif
