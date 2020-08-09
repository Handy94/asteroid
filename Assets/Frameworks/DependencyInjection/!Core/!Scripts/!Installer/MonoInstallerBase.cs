namespace HandyPackage
{
    using UnityEngine;

    public abstract class MonoInstallerBase : MonoBehaviour, IInstaller
    {
        public DIContainer Container;
        [SerializeField] private bool isEnabled = true;
        public virtual bool IsEnabled => isEnabled;

        public abstract void InstallDependencies();
    }
}
