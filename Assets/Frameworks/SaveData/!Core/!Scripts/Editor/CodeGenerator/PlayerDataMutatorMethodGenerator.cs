using System.Collections;
using System.Collections.Generic;
using HandyPackage.CodeGeneration;
using UnityEngine;
using UniRx;
using System;

namespace HandyPackage.Editor
{
    public static class PlayerDataMutatorMethodGenerator
    {
        public static MethodGenerationData GenerateSetMethod(PlayerDataEditorData data)
        {
            return new MethodGenerationData
            {
                m_MethodName = PlayerDataCodeGeneratorConstants.MUTATOR_SET_METHOD_NAME + data.key.ToCamelCase(true),
                m_MethodReturnType = "void",
                m_MethodParams = new List<MethodParameterData>
                {
                    new MethodParameterData(PlayerDataCodeGeneratorUtility.CreateValueReturnType(data), "value")
                },
                m_MethodBodyStatements = new List<string>
                {
                    VariableTypeCheckerUtility.IsVariableCollection(data.baseDataType)
                    ? $"{PlayerDataCodeGeneratorConstants.PLAYER_DATA_REFERENCE_NAME}.{data.key.ToCamelCase(false)} = value.ToReactiveCollection();"
                    : VariableTypeCheckerUtility.IsVariableDictionary(data.baseDataType)
                    ? $"{PlayerDataCodeGeneratorConstants.PLAYER_DATA_REFERENCE_NAME}.{data.key.ToCamelCase(false)} = value.ToReactiveDictionary();"
                    : $"{PlayerDataCodeGeneratorConstants.PLAYER_DATA_REFERENCE_NAME}.{data.key.ToCamelCase(false)}.Value = value;"
                }
            };
        }

        public static List<MethodGenerationData> GenerateNumericMutatorMethods(PlayerDataEditorData data)
        {
            return new List<MethodGenerationData>
            {
                GenerateAddMethod(data),
                GenerateUseMethod(data)
            };
        }

        public static List<MethodGenerationData> GenerateListMutatorMethods(PlayerDataEditorData data)
        {
            return new List<MethodGenerationData>
            {
                GenerateAddToListMethod(data),
                GenerateAddRangeToListMethod(data),
                GenerateRemoveMethod(data),
                // GenerateRemoveWithPredicateMethod(data)
            };
        }

        public static List<MethodGenerationData> GenerateDictionaryMutatorMethods(PlayerDataEditorData data)
        {
            return new List<MethodGenerationData>
            {
                GenerateSetOrAddKvpMethod(data),
                GenerateAddKvpMethod(data),
                GenerateAddRangeToDictionaryMethod(data),
                GenerateRemoveKeyMethod(data)
            };
        }

        public static List<MethodGenerationData> GenerateDictionaryNumericMutatorMethods(PlayerDataEditorData data)
        {
            return new List<MethodGenerationData>
            {
                GenerateDictionaryNumericAddMethod(data),
                GenerateDictionaryNumericUseMethod(data)
            };
        }

        private static MethodGenerationData GenerateAddMethod(PlayerDataEditorData data)
        {
            return new MethodGenerationData
            {
                m_MethodName = PlayerDataCodeGeneratorConstants.MUTATOR_ADD_METHOD_NAME + data.key.ToCamelCase(true),
                m_MethodReturnType = "void",
                m_MethodParams = new List<MethodParameterData>
                {
                    new MethodParameterData(data.baseDataType, "value")
                },
                m_MethodBodyStatements = new List<string>
                {
                    $"{PlayerDataCodeGeneratorConstants.MUTATOR_SET_METHOD_NAME}{data.key.ToCamelCase(true)}" +
                    $"({PlayerDataCodeGeneratorConstants.PLAYER_DATA_REFERENCE_NAME}.{data.key.ToCamelCase(false)}.Value + value);"
                }
            };
        }

        private static MethodGenerationData GenerateUseMethod(PlayerDataEditorData data)
        {
            return new MethodGenerationData
            {
                m_MethodName = PlayerDataCodeGeneratorConstants.MUTATOR_USE_METHOD_NAME + data.key.ToCamelCase(true),
                m_MethodReturnType = "void",
                m_MethodParams = new List<MethodParameterData>
                {
                    new MethodParameterData(data.baseDataType, "value")
                },
                m_MethodBodyStatements = new List<string>
                {
                    $"{PlayerDataCodeGeneratorConstants.MUTATOR_SET_METHOD_NAME}{data.key.ToCamelCase(true)}" +
                    $"({PlayerDataCodeGeneratorConstants.PLAYER_DATA_REFERENCE_NAME}.{data.key.ToCamelCase(false)}.Value - value);"
                }
            };
        }

        private static MethodGenerationData GenerateAddToListMethod(PlayerDataEditorData data)
        {
            return new MethodGenerationData
            {
                m_MethodName = PlayerDataCodeGeneratorConstants.MUTATOR_COLLECTION_ADD_TO_LIST_METHOD_NAME + data.key.ToCamelCase(true),
                m_MethodReturnType = "void",
                m_MethodParams = new List<MethodParameterData>
                {
                    new MethodParameterData(data.valueDataType, "value")
                },
                m_MethodBodyStatements = new List<string>
                {
                    $"{PlayerDataCodeGeneratorConstants.PLAYER_DATA_REFERENCE_NAME}.{data.key.ToCamelCase(false)}.Add(value);"
                }
            };
        }

        private static MethodGenerationData GenerateAddRangeToListMethod(PlayerDataEditorData data)
        {
            return new MethodGenerationData
            {
                m_MethodName = "AddRange_" + data.key.ToCamelCase(true),
                m_MethodReturnType = "void",
                m_MethodParams = new List<MethodParameterData>
                {
                    new MethodParameterData(PlayerDataCodeGeneratorUtility.CreateValueReturnType(data), "values")
                },

                m_MethodBodyStatements = new List<string>
                {
                    $"foreach (var value in values)\n\r{PlayerDataCodeGeneratorConstants.MUTATOR_COLLECTION_ADD_TO_LIST_METHOD_NAME}{data.key.ToCamelCase(true)}(value);",
                }
            };
        }

        private static MethodGenerationData GenerateRemoveMethod(PlayerDataEditorData data)
        {
            return new MethodGenerationData
            {
                m_MethodName = "Remove_" + data.key.ToCamelCase(true),
                m_MethodReturnType = "void",
                m_MethodParams = new List<MethodParameterData>
                {
                    new MethodParameterData(data.valueDataType, "value")
                },
                m_MethodBodyStatements = new List<string>
                {
                    $"{PlayerDataCodeGeneratorConstants.PLAYER_DATA_REFERENCE_NAME}.{data.key.ToCamelCase(false)}.Remove(value);"
                }
            };
        }

        // dictionary only
        private static MethodGenerationData GenerateSetOrAddKvpMethod(PlayerDataEditorData data)
        {
            return new MethodGenerationData
            {
                m_MethodName = PlayerDataCodeGeneratorConstants.MUTATOR_DICTIONARY_SET_OR_ADD_KVP_METHOD_NAME + data.key.ToCamelCase(true),
                m_MethodReturnType = "void",
                m_MethodParams = new List<MethodParameterData>
                {
                    new MethodParameterData(data.keyDataType, "key"),
                    new MethodParameterData(data.valueDataType, "value")
                },
                m_MethodBodyStatements = new List<string>
                {
                    $"var dictionary = {PlayerDataCodeGeneratorConstants.PLAYER_DATA_REFERENCE_NAME}.{data.key.ToCamelCase(false)};",
                    $"if (!dictionary.ContainsKey(key))\n\r{PlayerDataCodeGeneratorConstants.MUTATOR_DICTIONARY_ADD_KVP_METHOD_NAME + data.key.ToCamelCase(true)}(key, value);",
                    $"else {PlayerDataCodeGeneratorConstants.PLAYER_DATA_REFERENCE_NAME}.{data.key.ToCamelCase(false)}[key] = value;"
                }
            };
        }

        // dictionary only
        private static MethodGenerationData GenerateAddKvpMethod(PlayerDataEditorData data)
        {
            return new MethodGenerationData
            {
                m_MethodName = PlayerDataCodeGeneratorConstants.MUTATOR_DICTIONARY_ADD_KVP_METHOD_NAME + data.key.ToCamelCase(true),
                m_MethodReturnType = "void",
                m_MethodParams = new List<MethodParameterData>
                {
                    new MethodParameterData(data.keyDataType, "key"),
                    new MethodParameterData(data.valueDataType, "value")
                },
                m_MethodBodyStatements = new List<string>
                {
                    $"var dictionary = {PlayerDataCodeGeneratorConstants.PLAYER_DATA_REFERENCE_NAME}.{data.key.ToCamelCase(false)};",
                    "if (dictionary.ContainsKey(key))\n\rreturn;",
                    $"{PlayerDataCodeGeneratorConstants.PLAYER_DATA_REFERENCE_NAME}.{data.key.ToCamelCase(false)}.Add(key, value);"
                }
            };
        }

        private static MethodGenerationData GenerateAddRangeToDictionaryMethod(PlayerDataEditorData data)
        {
            return new MethodGenerationData
            {
                m_MethodName = "AddRange_" + data.key.ToCamelCase(true),
                m_MethodReturnType = "void",
                m_MethodParams = new List<MethodParameterData>
                {
                    new MethodParameterData(PlayerDataCodeGeneratorUtility.CreateValueReturnType(data), "dictionary")
                },

                m_MethodBodyStatements = new List<string>
                {
                    "foreach (var kvp in dictionary)",
                    $"\n\r{PlayerDataCodeGeneratorConstants.MUTATOR_DICTIONARY_ADD_KVP_METHOD_NAME + data.key.ToCamelCase(true)}(kvp.Key, kvp.Value);"
                }
            };
        }

        private static MethodGenerationData GenerateRemoveKeyMethod(PlayerDataEditorData data)
        {
            return new MethodGenerationData
            {
                m_MethodName = "RemoveKey_" + data.key.ToCamelCase(true),
                m_MethodReturnType = "void",
                m_MethodParams = new List<MethodParameterData>
                {
                    new MethodParameterData(data.keyDataType, "key")
                },

                m_MethodBodyStatements = new List<string>
                {
                    $"var dictionary = {PlayerDataCodeGeneratorConstants.PLAYER_DATA_REFERENCE_NAME}.{data.key.ToCamelCase(false)};",
                    "if (!dictionary.ContainsKey(key))\n\rreturn;",
                    "dictionary.Remove(key);"
                }
            };
        }

        private static MethodGenerationData GenerateDictionaryNumericAddMethod(PlayerDataEditorData data)
        {
            return new MethodGenerationData
            {
                m_MethodName = PlayerDataCodeGeneratorConstants.MUTATOR_ADD_DICTIONARY_VALUE_METHOD_NAME + data.key.ToCamelCase(true),
                m_MethodReturnType = "void",
                m_MethodParams = new List<MethodParameterData>
                {
                    new MethodParameterData(data.keyDataType, "key"),
                    new MethodParameterData(data.valueDataType, "value")
                },
                m_MethodBodyStatements = new List<string>
                {
                    string.Format("{0}{1}(key, 0);", PlayerDataCodeGeneratorConstants.MUTATOR_DICTIONARY_ADD_KVP_METHOD_NAME, data.key.ToCamelCase(true)),
                    string.Format("{0}.{1}[key] += value;", PlayerDataCodeGeneratorConstants.PLAYER_DATA_REFERENCE_NAME, data.key.ToCamelCase(false))
                }
            };
        }

        private static MethodGenerationData GenerateDictionaryNumericUseMethod(PlayerDataEditorData data)
        {
            return new MethodGenerationData
            {
                m_MethodName = PlayerDataCodeGeneratorConstants.MUTATOR_USE_DICTIONARY_VALUE_METHOD_NAME + data.key.ToCamelCase(true),
                m_MethodReturnType = "void",
                m_MethodParams = new List<MethodParameterData>
                {
                    new MethodParameterData(data.keyDataType, "key"),
                    new MethodParameterData(data.valueDataType, "value")
                },
                m_MethodBodyStatements = new List<string>
                {
                    string.Format("{0}{1}(key, 0);", PlayerDataCodeGeneratorConstants.MUTATOR_DICTIONARY_ADD_KVP_METHOD_NAME, data.key.ToCamelCase(true)),
                    string.Format("{0}.{1}[key] -= value;", PlayerDataCodeGeneratorConstants.PLAYER_DATA_REFERENCE_NAME, data.key.ToCamelCase(false))
                }
            };
        }
    }
}


