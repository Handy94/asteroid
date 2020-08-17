using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HandyPackage.CodeGeneration;

namespace HandyPackage.Editor
{
    public static class PlayerDataCodeModifier
    {
        public static string ModifyPlayerDataLoader(string code, string newStatement)
        {
            List<MethodModificationData> modificationDatas = new List<MethodModificationData>
            {
                new MethodModificationData
                {
                    m_MethodNameToModify = "LoadData",
                    m_BodyModificationType = MethodBodyModificationType.ADD_OR_REPLACE_BODY,
                    m_BodyStatements = new List<string>{newStatement}
                }
            };

            return ClassScriptModifier.ModifyFunctionsInClass(code, modificationDatas);
        }

        public static string ModifyPlayerDataSubscribeManager(string code, string newStatement)
        {
            List<MethodModificationData> modificationDatas = new List<MethodModificationData>
            {
                new MethodModificationData
                {
                    m_MethodNameToModify = "SubscribeForSave",
                    m_BodyModificationType = MethodBodyModificationType.ADD_OR_REPLACE_BODY,
                    m_BodyStatements = new List<string>{newStatement}
                }
            };

            return ClassScriptModifier.ModifyFunctionsInClass(code, modificationDatas);
        }

        public static string ModifyPlayerTransactionResolverMap(string code, string newStatement)
        {
            List<MethodModificationData> modificationDatas = new List<MethodModificationData>
            {
                new MethodModificationData
                {
                    m_MethodNameToModify = "InitMaps",
                    m_BodyModificationType = MethodBodyModificationType.ADD_OR_REPLACE_BODY,
                    m_BodyStatements = new List<string>{newStatement}
                }
            };

            return ClassScriptModifier.ModifyFunctionsInClass(code, modificationDatas);
        }
    }
}


