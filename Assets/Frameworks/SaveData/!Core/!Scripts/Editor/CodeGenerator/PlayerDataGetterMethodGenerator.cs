using HandyPackage.CodeGeneration;
using System.Collections.Generic;

namespace HandyPackage.Editor
{
    public static class PlayerDataGetterMethodGenerator
    {
        public static MethodGenerationData GenerateGetReactivePropertyMethod(PlayerDataEditorData data)
        {
            return new MethodGenerationData
            {
                m_MethodName = PlayerDataCodeGeneratorConstants.GETTER_GET_REACTIVE_PROPERTY_METHOD_NAME + data.key.ToCamelCase(true),
                m_MethodReturnType = ReactivePropertyEditorUtility.CreateReactivePropertyType(data),
                m_MethodBodyStatements = new List<string>
                {
                    $"return {PlayerDataCodeGeneratorConstants.PLAYER_DATA_REFERENCE_NAME}.{data.key.ToCamelCase(false)};"
                }
            };
        }

        public static MethodGenerationData GenerateGetValueMethod(PlayerDataEditorData data)
        {
            return new MethodGenerationData
            {
                m_MethodName = PlayerDataCodeGeneratorConstants.GETTER_GET_VALUE_METHOD_NAME + data.key.ToCamelCase(true),
                m_MethodReturnType = PlayerDataCodeGeneratorUtility.CreateValueReturnType(data),
                m_MethodBodyStatements = new List<string>
                {
                    $"return {PlayerDataCodeGeneratorConstants.GETTER_GET_REACTIVE_PROPERTY_METHOD_NAME}{data.key.ToCamelCase(true)}().Value;"
                }
            };
        }

        public static List<MethodGenerationData> GenerateListGetterMethods(PlayerDataEditorData data)
        {
            return new List<MethodGenerationData>
            {
                // GenerateFindByPredicateMethod(data),
                GenerateGetAtIndexMethod(data),
                GenerateContainsMethod(data)
            };
        }

        public static List<MethodGenerationData> GenerateDictionaryGetterMethods(PlayerDataEditorData data)
        {
            return new List<MethodGenerationData>
            {
                GenerateGetByKeyMethod(data),
                GenerateContainsKeyMethod(data),
            };
        }

        // for lists only
        private static MethodGenerationData GenerateFindByPredicateMethod(PlayerDataEditorData data)
        {
            // List<string> test = new List<string>();
            // System.Predicate<string> predicate = x => x.Equals("test");
            // test.FindAll(predicate);

            return new MethodGenerationData
            {
                m_MethodName = PlayerDataCodeGeneratorConstants.GETTER_COLLECTION_GET_VALUES_BY_PREDICATE_METHOD_NAME + data.key.ToCamelCase(true),
                m_MethodReturnType = PlayerDataCodeGeneratorUtility.CreateValueReturnType(data),
                m_MethodParams = new List<MethodParameterData>
                {
                    new MethodParameterData($"Predicate<{data.valueDataType}>", "predicate")
                },
                m_MethodBodyStatements = new List<string>
                {
                    $"return {PlayerDataCodeGeneratorConstants.GETTER_GET_REACTIVE_PROPERTY_METHOD_NAME}{data.key.ToCamelCase(true)}().FindAll(predicate);"
                }
            };
        }

        private static MethodGenerationData GenerateGetAtIndexMethod(PlayerDataEditorData data)
        {
            return new MethodGenerationData
            {
                m_MethodName = PlayerDataCodeGeneratorConstants.GETTER_COLLECTION_GET_VALUE_AT_INDEX_METHOD_NAME + data.key.ToCamelCase(true),
                m_MethodReturnType = data.valueDataType,
                m_MethodParams = new List<MethodParameterData>
                {
                    new MethodParameterData("int", "index")
                },
                m_MethodBodyStatements = new List<string>
                {
                    $"if ({PlayerDataCodeGeneratorConstants.GETTER_GET_REACTIVE_PROPERTY_METHOD_NAME}{data.key.ToCamelCase(true)}().Count >= index || index < 0)\n\rreturn default;",
                    $"return {PlayerDataCodeGeneratorConstants.GETTER_GET_REACTIVE_PROPERTY_METHOD_NAME}{data.key.ToCamelCase(true)}()[index];"
                }
            };
        }

        private static MethodGenerationData GenerateContainsMethod(PlayerDataEditorData data)
        {
            return new MethodGenerationData
            {
                m_MethodName = PlayerDataCodeGeneratorConstants.GETTER_COLLECTION_CONTAINS_METHOD_NAME + data.key.ToCamelCase(true),
                m_MethodReturnType = "bool",
                m_MethodParams = new List<MethodParameterData>
                {
                    new MethodParameterData(data.valueDataType, "value")
                },
                m_MethodBodyStatements = new List<string>
                {
                    $"return {PlayerDataCodeGeneratorConstants.GETTER_GET_REACTIVE_PROPERTY_METHOD_NAME}{data.key.ToCamelCase(true)}().Contains(value);"
                }
            };
        }

        // dictionary
        private static MethodGenerationData GenerateGetByKeyMethod(PlayerDataEditorData data)
        {
            return new MethodGenerationData
            {
                m_MethodName = PlayerDataCodeGeneratorConstants.GETTER_DICTIONARY_GET_VALUE_BY_KEY_METHOD_NAME + data.key.ToCamelCase(true),
                m_MethodReturnType = data.valueDataType,
                m_MethodParams = new List<MethodParameterData>
                {
                    new MethodParameterData(data.keyDataType, "key")
                },
                m_MethodBodyStatements = new List<string>
                {
                    $"if (!{PlayerDataCodeGeneratorConstants.GETTER_DICTIONARY_CONTAINS_KEY_METHOD_NAME}{data.key.ToCamelCase(true)}(key))\r\nreturn default;",
                    $"return {PlayerDataCodeGeneratorConstants.GETTER_GET_REACTIVE_PROPERTY_METHOD_NAME}{data.key.ToCamelCase(true)}()[key];"
                }
            };
        }

        private static MethodGenerationData GenerateContainsKeyMethod(PlayerDataEditorData data)
        {
            return new MethodGenerationData
            {
                m_MethodName = PlayerDataCodeGeneratorConstants.GETTER_DICTIONARY_CONTAINS_KEY_METHOD_NAME + data.key.ToCamelCase(true),
                m_MethodReturnType = "bool",
                m_MethodParams = new List<MethodParameterData>
                {
                    new MethodParameterData(data.keyDataType, "key")
                },
                m_MethodBodyStatements = new List<string>
                {
                    $"return {PlayerDataCodeGeneratorConstants.GETTER_GET_REACTIVE_PROPERTY_METHOD_NAME}{data.key.ToCamelCase(true)}().ContainsKey(key);"
                }
            };
        }
    }
}
