#if UNITY_EDITOR
using HandyPackage;

namespace HandyPackage.Editor
{
    [System.Serializable]
    public class PlayerDataEditorData : System.ICloneable
    {
        public string key;
        public string baseDataType;
        public string keyDataType;
        public string valueDataType;


        public bool isLocalOnly = false;
        public bool shouldGenerateGetter = true;
        public bool shouldGenerateMutator = true;
        public bool shouldGenerateForSubscribe = true;

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
#endif
