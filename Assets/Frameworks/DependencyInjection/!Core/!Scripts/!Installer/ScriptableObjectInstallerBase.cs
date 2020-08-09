using UnityEngine;
namespace HandyPackage
{
    public abstract class ScriptableObjectInstallerBase : ScriptableObject, IInstaller
    {
        public DIContainer Container;
        [SerializeField] private bool isEnabled = true;
        public virtual bool IsEnabled => isEnabled;

        public abstract void InstallDependencies();
    }
}
