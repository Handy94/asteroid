using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandyPackage.CodeGeneration
{
    public class MethodModificationData
    {
        public MethodParameterModificationType m_ParamModificationType;
        public MethodBodyModificationType m_BodyModificationType;
        public string m_MethodNameToModify;
        public List<MethodParameterData> m_OriginalMethodParams = new List<MethodParameterData>();
        public List<MethodParameterData> m_NewMethodParams = new List<MethodParameterData>();
        public List<string> m_BodyStatements = new List<string>();
    }

    public enum MethodParameterModificationType
    {
        ADD_PARAMS, REPLACE_PARAMS
    }

    public enum MethodBodyModificationType
    {
        ADD_BODY, REPLACE_BODY, ADD_OR_REPLACE_BODY
    }
}


