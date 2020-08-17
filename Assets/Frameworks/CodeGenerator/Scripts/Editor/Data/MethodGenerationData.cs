using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandyPackage.CodeGeneration
{
    public class MethodGenerationData
    {
        public string m_MethodName;
        public string m_MethodReturnType;
        public FunctionInheritanceKeyword m_InheritanceKeyword = FunctionInheritanceKeyword.NONE;
        public bool m_IsAsync = false;
        // note: unused yet
        public List<AttributeGenerationData> m_Attributes = new List<AttributeGenerationData>();
        public List<MethodParameterData> m_MethodParams = new List<MethodParameterData>();
        /// <summary> The function body statements, such as "return this.Value;". Ensure the statements are lined up IN ORDER!!! </summary>
        public List<string> m_MethodBodyStatements = new List<string>();
        public ProtectionLevel m_ProtectionLevel = ProtectionLevel.Public;
    }

    public class MethodParameterData
    {
        public string m_ParamType;
        public string m_ParamName;

        public MethodParameterData(string paramType, string paramName)
        {
            m_ParamType = paramType;
            m_ParamName = paramName;
        }
    }
}


