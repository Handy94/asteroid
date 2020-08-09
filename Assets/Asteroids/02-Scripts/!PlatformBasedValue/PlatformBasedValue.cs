using UnityEngine;

namespace HandyPackage
{
    [System.Serializable]
    public class PlatformBasedValue<T>
    {
        [SerializeField] private T EditorValue;
        [SerializeField] private T WindowsValue;
        [SerializeField] private T OSXValue;
        [SerializeField] private T AndroidValue;
        [SerializeField] private T iOSValue;

        public T Value
        {
            get
            {
#if UNITY_EDITOR
                return EditorValue;
#elif UNITY_STANDALONE_WIN
                return WindowsValue;
#elif UNITY_STANDALONE_OSX
                return OSXValue;
#elif UNITY_ANDROID
                return AndroidValue;
#elif UNITY_IOS
                return iOSValue;
#else
                return EditorValue;
#endif
            }
        }
    }

}
