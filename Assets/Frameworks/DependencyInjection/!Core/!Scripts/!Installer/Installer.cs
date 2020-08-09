namespace HandyPackage
{
    public abstract class Installer : InstallerBase
    {

    }

    public abstract class Installer<TDerived> : InstallerBase where TDerived : Installer<TDerived>
    {
        public static void Install(DIContainer Container)
        {
            var installer = Container.Install<TDerived>();
            installer.Container = Container;
            installer.InstallDependencies();
        }
    }
}
