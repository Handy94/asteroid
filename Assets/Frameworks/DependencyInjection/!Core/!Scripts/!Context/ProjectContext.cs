namespace HandyPackage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class ProjectContext : AppContext
    {
        protected override DIContainer Container => StaticContainer.ProjectContainer;

        public void Init()
        {
            DontDestroyOnLoad(gameObject);
            Install();
        }

        private static void InstallAdditionals()
        {
            StaticContainer.ProjectContainer.Install<CoroutineManager>();
            StaticContainer.ProjectContainer.Install<MonoApplicationManager>();
        }

        public static void TryToInitProjectContext(Action actionOnProjectContextInited)
        {
            if (!IsProjectContextInited)
            {
                IsProjectContextInited = true;

                ProjectContext.InstallAdditionals();

                ProjectContextSource source = Resources.Load<ProjectContextSource>(StaticContainer.PROJECT_CONTEXT_RESOURCE_PATH);
                if (source == null) { actionOnProjectContextInited.Invoke(); return; }
                if (source.projectContext == null || source.projectContext.Count == 0) { actionOnProjectContextInited.Invoke(); return; }

                GameObject projectContextContainer = new GameObject(StaticContainer.PROJECT_CONTEXT_CONTAINER_GAME_OBJECT_NAME);
                MonoRunner monoRunner = projectContextContainer.AddComponent<MonoRunner>();
                monoRunner.InitializeCompleteEvent.Listen(() =>
                {
                    actionOnProjectContextInited?.Invoke();
                });
                DontDestroyOnLoad(projectContextContainer);

                GameObject projectContextObject = new GameObject(StaticContainer.PROJECT_CONTEXT_OBJECTS_GAME_OBJECT_NAME);
                projectContextObject.transform.SetParent(projectContextContainer.transform, false);

                List<ProjectContext> instantiatedProjectContexts = new List<ProjectContext>();
                foreach (var projectContext in source.projectContext)
                {
                    var objProjectContext = Instantiate(projectContext);
                    objProjectContext.Init();
                    objProjectContext.transform.SetParent(projectContextObject.transform, false);

                    instantiatedProjectContexts.Add(objProjectContext);
                }

                if (StaticContainer.ProjectContainer.Container == null || StaticContainer.ProjectContainer.Container.Count == 0) return;
                foreach (var installed in StaticContainer.ProjectContainer.Container)
                {
                    if (installed.Value.GetType().IsSubclassOf(typeof(Component)))
                    {
                        Component obj = (Component)installed.Value;
                        obj.transform.SetParent(projectContextContainer.transform, false);
                    }
                }
                monoRunner.InstallContainer(StaticContainer.ProjectContainer);
            }
        }
    }
}
