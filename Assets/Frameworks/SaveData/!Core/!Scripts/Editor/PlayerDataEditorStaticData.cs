namespace HandyPackage.Editor
{
    using System.Collections.Generic;
    public static class PlayerDataEditorStaticData
    {
        public const string KEY_DATA_TYPE_SEPARATOR = "_";
        public const string BOOL_DATA_TYPE = "bool";
        public const string INT_DATA_TYPE = "int";
        public const string FLOAT_DATA_TYPE = "float";
        public const string DOUBLE_DATA_TYPE = "double";
        public const string STRING_DATA_TYPE = "string";
        public const string LIST_DATA_TYPE = "List";
        public const string DICTIONARY_DATA_TYPE = "Dictionary";

        public const string KEY_DATA_LOCAL_PREFIX = "LOCAL";

        public static List<string> numericType = new List<string>()
        {
            INT_DATA_TYPE,
            FLOAT_DATA_TYPE,
            DOUBLE_DATA_TYPE
        };

        public static List<string> supportedDataType = new List<string>()
        {
            INT_DATA_TYPE,
            FLOAT_DATA_TYPE,
            DOUBLE_DATA_TYPE,
            BOOL_DATA_TYPE,
            STRING_DATA_TYPE,
            LIST_DATA_TYPE,
            DICTIONARY_DATA_TYPE
        };

        public static List<string> valueSupportedDataType = new List<string>()
        {
            INT_DATA_TYPE,
            FLOAT_DATA_TYPE,
            DOUBLE_DATA_TYPE,
            BOOL_DATA_TYPE,
            STRING_DATA_TYPE
        };

        public static List<string> keySupportedDataType = new List<string>()
        {
            STRING_DATA_TYPE
        };

        public static Dictionary<string, string> saveDataKeyPrefix = new Dictionary<string, string>()
        {
            {INT_DATA_TYPE, "INT"},
            {FLOAT_DATA_TYPE, "FLOAT"},
            {DOUBLE_DATA_TYPE, "DOUBLE"},
            {BOOL_DATA_TYPE, "IS"},
            {STRING_DATA_TYPE, "STRING"},
            {LIST_DATA_TYPE, "LIST"},
            {DICTIONARY_DATA_TYPE, "DICT"},
        };
    }
}