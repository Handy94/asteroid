namespace HandyPackage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class DIContainer
    {
        private const string SINGLETON_GAMEOBJECT_NAME_PREFIX = "Singleton - ";

        private Dictionary<string, object> _container;
        public Dictionary<string, object> Container => _container;

        private List<string> excludedInterface = new List<string>()
        {
            typeof(IInitializable).ToString(),
            typeof(ILateInitializable).ToString(),
            typeof(IFixedTickable).ToString(),
            typeof(ITickable).ToString(),
            typeof(ILateTickable).ToString(),
            typeof(IDisposable).ToString(),
            typeof(ILateDisposable).ToString(),
            typeof(IMonoApplicationFocus).ToString(),
            typeof(IMonoApplicationPause).ToString(),
            typeof(IMonoApplicationQuit).ToString(),
            typeof(IFactory).ToString(),
        };

        public DIContainer()
        {
            _container = new Dictionary<string, object>();
        }

        public bool IsTypeExist<T>()
        {
            return IsTypeExist(typeof(T));
        }

        private bool IsTypeExist(Type type)
        {
            return IsTypeExist(type.ToString());
        }

        private bool IsTypeExist(string typeString)
        {
            return _container.ContainsKey(typeString);
        }

        private object CreateObject(Type type)
        {
            object tempObj = null;
            if (type.IsSubclassOf(typeof(ScriptableObject)))
            {
                tempObj = ScriptableObject.CreateInstance(type);
            }
            else if (type.IsSubclassOf(typeof(Component)))
            {
                GameObject go = new GameObject($"{SINGLETON_GAMEOBJECT_NAME_PREFIX}{type.ToString()}");
                tempObj = go.AddComponent(type);
            }
            else
            {
                tempObj = Activator.CreateInstance(type);
            }
            return tempObj;
        }

        public T Install<T>()
        {
            return Install<T>((T)CreateObject(typeof(T)));
        }

        public T Install<T>(T obj)
        {
            Type type = typeof(T);
            return (T)Install(type, obj);
        }

        public T InstallFromPrefab<T>(T prefab) where T : UnityEngine.Object
        {
            T instantiated = GameObject.Instantiate<T>(prefab);
            return (T)Install<T>(instantiated);
        }

        public object Install(Type type, object obj)
        {
            string typeString = type.ToString();
            if (IsTypeExist(typeString))
            {
                throw new Exception($"Container already have object with type of \"{typeString}\"");
            }
            _container.Add(typeString, obj);
            return obj;
        }

        #region Interface
        public T InstallInterface<T>()
        {
            return (T)InstallInterface(typeof(T));
        }
        public T InstallInterfaceAndSelf<T>()
        {
            return (T)InstallInterfaceAndSelf(typeof(T));
        }
        public T InstallInterface<T>(T obj)
        {
            return (T)InstallInterface(obj, typeof(T));
        }
        public T InstallInterfaceAndSelf<T>(T obj)
        {
            return (T)InstallInterfaceAndSelf(obj, typeof(T));
        }
        public System.Object InstallInterfaceAndSelf(Type type)
        {
            return InstallInterfaceAndSelf(CreateObject(type), type);
        }
        public System.Object InstallInterface(Type type)
        {
            return InstallInterface(CreateObject(type), type);
        }
        public System.Object InstallInterfaceAndSelf(System.Object obj, Type type)
        {
            return InstallInterfaceInternal(obj, type, true);
        }
        public System.Object InstallInterface(System.Object obj, Type type)
        {
            return InstallInterfaceInternal(obj, type, false);
        }

        bool CanRegisterType(Type type)
        {
            return !excludedInterface.Contains(type.ToString());
        }

        System.Object InstallInterfaceInternal(System.Object obj, Type type, bool installSelf)
        {
            if (installSelf)
                Install(type, obj);

            Type[] interfaces = type.GetInterfaces();
            foreach (var item in interfaces)
            {
                if (CanRegisterType(item))
                    Install(item, obj);
            }
            return obj;
        }
        #endregion


        public T GetObject<T>()
        {
            string typeString = typeof(T).ToString();
            if (!IsTypeExist<T>())
            {
                Debug.LogError($"Container does't have an object with type of \"{typeString}\"");
                return default(T);
            }
            return (T)_container[typeString];
        }

        public void Clear()
        {
            this._container.Clear();
        }
    }
}
