namespace HandyPackage
{
    public abstract class InstallerBase : IInstaller
    {
        private DIContainer _container;
        protected DIContainer Container
        {
            get { return _container; }
            set { _container = value; }
        }

        public abstract void InstallDependencies();
    }
}
