namespace HandyPackage
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class SceneContext : AppContext
    {
        protected override DIContainer Container => StaticContainer.SceneContainer;

        private MonoRunner _monoRunner;
        protected MonoRunner MonoRunner
        {
            get
            {
                if (_monoRunner == null)
                {
                    _monoRunner = gameObject.AddComponent<MonoRunner>();
                }
                return _monoRunner;
            }
        }
        public bool initOnAwake = false;

        public bool Initialized { get; private set; }

        private void Awake()
        {
            if (initOnAwake) Init();
        }

        public void Init()
        {
            if (!IsProjectContextInited)
            {
                ProjectContext.TryToInitProjectContext(InitSceneContext);
            }
            else
            {
                InitSceneContext();
            }
        }
        void InitSceneContext()
        {
            Initialized = false;

            List<string> prevContainer = Container.Container.Select(x => x.Key).ToList();
            Install();

            GameObject sceneContextContainer = new GameObject("SceneContextContainer");
            sceneContextContainer.transform.SetParent(transform, false);

            foreach (var installed in Container.Container)
            {
                if (installed.Value == null) continue;
                if (prevContainer.Contains(installed.Key)) continue;

                if (installed.Value.GetType().IsSubclassOf(typeof(Component)))
                {
                    Component obj = (Component)installed.Value;
                    obj.transform.SetParent(sceneContextContainer.transform, false);
                }
                MonoRunner.RegisterObject(installed.Value);
            }

            MonoRunner.InitializeCompleteEvent.Listen(OnInitializationCompleted);
        }

        void OnInitializationCompleted()
        {
            MonoRunner.InitializeCompleteEvent.Unlisten(OnInitializationCompleted);
            Initialized = true;
        }
    }
}
