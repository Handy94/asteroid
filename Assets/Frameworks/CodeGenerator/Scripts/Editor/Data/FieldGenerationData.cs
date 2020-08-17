using System.Collections.Generic;

namespace HandyPackage.CodeGeneration
{
    public class FieldGenerationData
    {
        public string m_VariableType; // string, int, ..
        public string m_VariableName; // testInt, m_TestInt
        public ProtectionLevel m_ProtectionLevel = ProtectionLevel.Private;
        public StaticFieldType m_StaticModifier;
        public bool m_UseInitializer = true;
        public string m_InitializerValue = string.Empty;
        public List<AttributeGenerationData> m_Attributes = new List<AttributeGenerationData>();
    }

    public enum StaticFieldType
    {
        None, Static, Const, StaticReadOnly
    }
}

