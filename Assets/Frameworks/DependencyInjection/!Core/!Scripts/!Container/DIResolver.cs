namespace HandyPackage
{
    public static class DIResolver
    {
        public static T GetObject<T>(bool silent = false)
        {
            if (StaticContainer.ProjectContainer.IsTypeExist<T>())
            {
                return StaticContainer.ProjectContainer.GetObject<T>();
            }
            if (StaticContainer.SceneContainer.IsTypeExist<T>())
            {
                return StaticContainer.SceneContainer.GetObject<T>();
            }
            else
            {
                if (!silent)
                    UnityEngine.Debug.LogError($"DI Error: No object with type \"{typeof(T).ToString()}\" installed");
                return default;
            }
        }
    }
}
