namespace HandyPackage
{
    public static class StaticContainer
    {
        public static DIContainer ProjectContainer = new DIContainer();
        public static DIContainer SceneContainer = new DIContainer();

        public const string PROJECT_CONTEXT_RESOURCE_PATH = "ProjectContextSource";
        public const string PROJECT_CONTEXT_CONTAINER_GAME_OBJECT_NAME = "ProjectContextContainer";
        public const string PROJECT_CONTEXT_OBJECTS_GAME_OBJECT_NAME = "ProjectContextObjects";
    }
}
