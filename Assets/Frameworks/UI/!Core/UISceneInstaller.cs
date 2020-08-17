namespace HandyPackage
{
    using UnityEngine;

    public class UISceneInstaller : MonoInstaller
    {
        public UIManager uiManager;

        public override void InstallDependencies()
        {
            if (uiManager == null) uiManager = GameObject.FindObjectOfType<UIManager>();

            Container.Install(uiManager);
        }
    }
}
