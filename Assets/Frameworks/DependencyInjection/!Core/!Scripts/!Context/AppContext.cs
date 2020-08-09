namespace HandyPackage
{
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class AppContext : MonoBehaviour
    {
        public List<MonoInstaller> monoInstallers;
        public List<ScriptableObjectInstaller> scriptableObjectInstallers;

        protected static bool IsProjectContextInited = false;

        protected abstract DIContainer Container { get; }

        protected void Install()
        {
            int count = 0;

            count = scriptableObjectInstallers.Count;
            for (int i = 0; i < count; i++)
            {
                if (scriptableObjectInstallers[i].IsEnabled)
                {
                    scriptableObjectInstallers[i].Container = Container;
                    scriptableObjectInstallers[i].InstallDependencies();
                }
            }

            count = monoInstallers.Count;
            for (int i = 0; i < count; i++)
            {
                if (monoInstallers[i].IsEnabled)
                {
                    monoInstallers[i].Container = Container;
                    monoInstallers[i].InstallDependencies();
                }
            }
        }
        public void ClearContainer()
        {
            if (Container != null) Container.Clear();
        }
        private void OnDestroy()
        {
            ClearContainer();
        }
    }
}
