namespace HandyPackage
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(menuName = "DI/ProjectContextSource", fileName = StaticContainer.PROJECT_CONTEXT_RESOURCE_PATH)]
    public class ProjectContextSource : ScriptableObject
    {
        public List<ProjectContext> projectContext;
    }
}
