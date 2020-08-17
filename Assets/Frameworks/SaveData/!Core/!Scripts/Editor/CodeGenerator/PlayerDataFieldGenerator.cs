using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HandyPackage.CodeGeneration;

namespace HandyPackage.Editor
{
    public static class PlayerDataFieldGenerator
    {
        public static FieldGenerationData GeneratePlayerDataField(PlayerDataEditorData data)
        {
            return new FieldGenerationData
            {
                m_VariableName = data.key.ToCamelCase(false),
                m_VariableType = ReactivePropertyEditorUtility.CreateReactivePropertyType(data),
                m_ProtectionLevel = ProtectionLevel.Public
            };
        }

        public static FieldGenerationData GeneratePlayerDataKeyField(PlayerDataEditorData data)
        {
            return new FieldGenerationData
            {
                m_VariableName = data.key,
                m_VariableType = "string",
                m_InitializerValue = data.key,
                m_ProtectionLevel = ProtectionLevel.Public,
                m_StaticModifier = StaticFieldType.Const
            };
        }
    }
}


