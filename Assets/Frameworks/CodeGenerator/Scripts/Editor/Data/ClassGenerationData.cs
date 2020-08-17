using System.Collections.Generic;

namespace HandyPackage.CodeGeneration
{
    public class ClassGenerationData
    {
        public string m_Namespace;
        public string m_ClassName;
        public string[] m_Usings = new string[0];
        public string[] m_BaseClasses = new string[0];
        public List<AttributeGenerationData> m_ClassAttributes = new List<AttributeGenerationData>();
        public ClassType m_ClassType = ClassType.Standard;
        public List<ConstraintData> m_Constraints = new List<ConstraintData>();
        public List<FieldGenerationData> m_FieldGenerationDatas = new List<FieldGenerationData>();
        public List<MethodGenerationData> m_MethodGenerationDatas = new List<MethodGenerationData>();
    }

    public enum ClassType
    {
        Standard,           // not any of the below
        Static, 
        Partial, 
        StaticPartial
    }
}



