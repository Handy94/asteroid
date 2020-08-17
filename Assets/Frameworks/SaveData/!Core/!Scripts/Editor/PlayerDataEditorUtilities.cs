#if UNITY_EDITOR
namespace HandyPackage.Editor
{
    using System.Collections.Generic;
    using System.Linq;

    public static class PlayerDataEditorUtilities
    {
        public static string GetKeyPrefix(string type)
        {
            if (string.IsNullOrEmpty(type)) return string.Empty;
            if (PlayerDataEditorStaticData.saveDataKeyPrefix.ContainsKey(type)) return PlayerDataEditorStaticData.saveDataKeyPrefix[type];
            return string.Empty;
        }

        public static string GetRawPlayerDataEditorKey(string key)
        {
            if (string.IsNullOrEmpty(key)) return key;
            string result = key;
            bool isKeyPrefixExist = false;
            do
            {
                isKeyPrefixExist = false;
                foreach (var item in PlayerDataEditorStaticData.saveDataKeyPrefix)
                {
                    string prefix = item.Value + PlayerDataEditorStaticData.KEY_DATA_TYPE_SEPARATOR;
                    if (result.StartsWith(prefix))
                    {
                        result = result.Replace(prefix, string.Empty);
                        isKeyPrefixExist = true;
                        break;
                    }
                }
            } while (isKeyPrefixExist);
            string localPrefix = PlayerDataEditorStaticData.KEY_DATA_LOCAL_PREFIX + PlayerDataEditorStaticData.KEY_DATA_TYPE_SEPARATOR;
            if (result.StartsWith(localPrefix))
            {
                result = result.Replace(localPrefix, string.Empty);
            }
            return result;
        }

        public static string EvaluatePlayerDataEditorKey(PlayerDataEditorData data)
        {
            if (data == null) return string.Empty;

            data.key = GetRawPlayerDataEditorKey(data.key);

            List<string> typeStrings = new List<string>()
            {
                GetKeyPrefix(data.baseDataType),
                GetKeyPrefix(data.keyDataType),
                GetKeyPrefix(data.valueDataType)
            }.FindAll(x => !string.IsNullOrEmpty(x));

            if (typeStrings.Count == 0) return string.Empty;

            string finalType = string.Join(PlayerDataEditorStaticData.KEY_DATA_TYPE_SEPARATOR, typeStrings);
            if (data.isLocalOnly)
            {
                finalType = string.Join(PlayerDataEditorStaticData.KEY_DATA_TYPE_SEPARATOR, finalType, PlayerDataEditorStaticData.KEY_DATA_LOCAL_PREFIX);
            }
            string finalKeyName = string.Join(PlayerDataEditorStaticData.KEY_DATA_TYPE_SEPARATOR, finalType, data.key);

            return finalKeyName;
        }
    }
}
#endif