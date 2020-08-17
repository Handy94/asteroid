using HandyPackage.CodeGeneration;
using System.Collections.Generic;

namespace HandyPackage.Editor
{
    public static class PlayerDataMethodGenerator
    {
        public static List<MethodGenerationData> GenerateGetterMethods(PlayerDataEditorData data)
        {
            var methods = new List<MethodGenerationData>();

            if (!data.shouldGenerateGetter)
                return methods;

            methods.Add(PlayerDataGetterMethodGenerator.GenerateGetReactivePropertyMethod(data));

            bool isVariableCollection = VariableTypeCheckerUtility.IsVariableCollection(data.baseDataType);
            bool IsVariableDictionary = VariableTypeCheckerUtility.IsVariableDictionary(data.baseDataType);
            bool isVariableNumeric = VariableTypeCheckerUtility.IsVariableNumeric(data.baseDataType);

            if (!isVariableCollection && !IsVariableDictionary)
                methods.Add(PlayerDataGetterMethodGenerator.GenerateGetValueMethod(data));

            if (isVariableCollection)
                methods.AddRange(PlayerDataGetterMethodGenerator.GenerateListGetterMethods(data));

            if (IsVariableDictionary)
                methods.AddRange(PlayerDataGetterMethodGenerator.GenerateDictionaryGetterMethods(data));

            return methods;
        }

        public static List<MethodGenerationData> GenerateMutatorMethods(PlayerDataEditorData data)
        {
            var methods = new List<MethodGenerationData>();

            if (!data.shouldGenerateMutator)
                return methods;

            methods.Add(PlayerDataMutatorMethodGenerator.GenerateSetMethod(data));

            bool isVariableCollection = VariableTypeCheckerUtility.IsVariableCollection(data.baseDataType);
            bool IsVariableDictionary = VariableTypeCheckerUtility.IsVariableDictionary(data.baseDataType);
            bool isVariableNumeric = VariableTypeCheckerUtility.IsVariableNumeric(data.baseDataType);

            if (isVariableNumeric)
                methods.AddRange(PlayerDataMutatorMethodGenerator.GenerateNumericMutatorMethods(data));

            if (isVariableCollection)
                methods.AddRange(PlayerDataMutatorMethodGenerator.GenerateListMutatorMethods(data));

            if (IsVariableDictionary)
            {
                methods.AddRange(PlayerDataMutatorMethodGenerator.GenerateDictionaryMutatorMethods(data));

                if (VariableTypeCheckerUtility.IsVariableNumeric(data.valueDataType))
                    methods.AddRange(PlayerDataMutatorMethodGenerator.GenerateDictionaryNumericMutatorMethods(data));
            }


            return methods;
        }

        public static MethodGenerationData GeneratePlayerDataKeyValueGetterMethod(List<PlayerDataEditorData> datas, string category)
        {
            List<string> statements = new List<string>();

            foreach (var data in datas)
            {
                bool useReactiveProperty = PlayerDataCodeGeneratorUtility.UsesReactiveProperty(data.baseDataType);
                // dictionary[PlayerDataKeys.INT_TEST_INT] = () => _playerDataGetter.GetValue_TestInt();
                statements.Add(string.Format("getterMap[{0}.{1}] = () => {2}.{3}{4}();",
                    PlayerDataCodeGeneratorConstants.PLAYER_DATA_KEYS_CLASS_NAME,
                    data.key,
                    PlayerDataCodeGeneratorConstants.PLAYER_DATA_GETTER_REFERENCE_NAME,
                    useReactiveProperty ? PlayerDataCodeGeneratorConstants.GETTER_GET_REACTIVE_PROPERTY_METHOD_NAME : PlayerDataCodeGeneratorConstants.GETTER_GET_VALUE_METHOD_NAME,
                    data.key.ToCamelCase(true)));
            }

            return new MethodGenerationData
            {
                m_MethodName = PlayerDataCodeGeneratorConstants.KEY_VALUE_GETTER_MAP_METHOD_NAME + category,
                m_MethodReturnType = "void",
                m_MethodBodyStatements = statements,
                m_ProtectionLevel = ProtectionLevel.Private
            };
        }

        public static MethodGenerationData GenerateLoadMethod(List<PlayerDataEditorData> datas, string category)
        {

            List<string> bodyStatement = new List<string>();
            for (int i = 0; i < datas.Count; i++)
            {
                bool usesReactiveProperty = PlayerDataCodeGeneratorUtility.UsesReactiveProperty(datas[i].baseDataType);
                // Example: _playerData.Set_TestInt(_playerDataManager.TryLoad<int>(PlayerDataKeys.INT_TEST_INT))
                string defaultValue = "default";
                bool isVariableCollection = VariableTypeCheckerUtility.IsVariableCollection(datas[i].baseDataType);
                bool IsVariableDictionary = VariableTypeCheckerUtility.IsVariableDictionary(datas[i].baseDataType);

                string baseFormat = "{0}.{1} = {2}.TryLoad<{3}>({4}.{5}, {6}, {7});";
                string loadType = datas[i].baseDataType;

                if (isVariableCollection)
                {
                    defaultValue = $"new {datas[i].baseDataType}<{datas[i].valueDataType}>()";
                    baseFormat = "{0}.{1} = new " + ReactivePropertyEditorUtility.CreateReactivePropertyType(datas[i]) + "({2}.TryLoad<{3}>({4}.{5}, {6}, {7}));";
                    loadType = $"{datas[i].baseDataType}<{datas[i].valueDataType}>";
                }
                else if (IsVariableDictionary)
                {
                    defaultValue = $"new {datas[i].baseDataType}<{datas[i].keyDataType}, {datas[i].valueDataType}>()";
                    baseFormat = "{0}.{1} = new " + ReactivePropertyEditorUtility.CreateReactivePropertyType(datas[i]) + "({2}.TryLoad<{3}>({4}.{5}, {6}, {7}));";
                    loadType = $"{datas[i].baseDataType}<{datas[i].keyDataType},{datas[i].valueDataType}>";
                }

                bodyStatement.Add(string.Format(baseFormat,
                        PlayerDataCodeGeneratorConstants.PLAYER_DATA_REFERENCE_NAME,
                        usesReactiveProperty ? datas[i].key.ToCamelCase(false) : datas[i].key.ToCamelCase(false) + ".Value",
                        PlayerDataCodeGeneratorConstants.PLAYER_DATA_MANAGER_REFERENCE_NAME,
                                                loadType,
                        PlayerDataCodeGeneratorConstants.PLAYER_DATA_KEYS_CLASS_NAME,
                                                datas[i].key, defaultValue, datas[i].isLocalOnly.ToString().ToLower()));
            }

            return new MethodGenerationData
            {
                m_MethodName = PlayerDataCodeGeneratorConstants.LOADER_METHOD_NAME + category,
                m_MethodReturnType = "void",
                m_MethodBodyStatements = bodyStatement
            };
        }

        public static MethodGenerationData GenerateSubscribeMethod(List<PlayerDataEditorData> datas, string category)
        {
            List<string> bodyStatement = new List<string>();

            for (int i = 0; i < datas.Count; i++)
            {
                if (!datas[i].shouldGenerateForSubscribe) continue;

                string code = string.Format("{0}.{1}.SubscribeToPersistence({2}.{3}, disposables,{4});",
                        PlayerDataCodeGeneratorConstants.PLAYER_DATA_REFERENCE_NAME,
                        datas[i].key.ToCamelCase(false),
                        PlayerDataCodeGeneratorConstants.PLAYER_DATA_KEYS_CLASS_NAME,
                        datas[i].key,
                        datas[i].isLocalOnly.ToString().ToLower());
                bodyStatement.Add(code);
            }

            // to give the function an empty body if there's nothing to subscribe
            if (bodyStatement.Count == 0)
                bodyStatement.Add(string.Empty);

            return new MethodGenerationData
            {
                m_MethodName = PlayerDataCodeGeneratorConstants.SUBSCRIBE_METHOD_NAME + category,
                m_MethodReturnType = "void",
                m_MethodBodyStatements = bodyStatement
            };
        }

        public static List<MethodGenerationData> GenerateSaverMethods(List<PlayerDataEditorData> datas, string category)
        {
            var methods = new List<MethodGenerationData>();
            List<string> bodyStatement = new List<string>();

            // Example: _playerDataManager.TrySave<int>(PlayerDataKeys.INT_TEST_INT, _playerData.matchThreeLevel.Value);
            for (int i = 0, count = datas.Count; i < count; i++)
            {
                methods.Add(new MethodGenerationData
                {
                    m_MethodName = PlayerDataCodeGeneratorConstants.SAVER_METHOD_NAME + datas[i].key.ToCamelCase(true),
                    m_MethodReturnType = "void",
                    m_MethodBodyStatements = new List<string>
                    {
                        string.Format("{0}.TrySave<{1}>({2}.{3}, {4});",
                            PlayerDataCodeGeneratorConstants.PLAYER_DATA_MANAGER_REFERENCE_NAME,
                            PlayerDataCodeGeneratorUtility.CreateValueReturnType(datas[i]),
                            PlayerDataCodeGeneratorConstants.PLAYER_DATA_KEYS_CLASS_NAME,
                            datas[i].key,
                            PlayerDataCodeGeneratorUtility.GetPlayerDataVariableReferenceForSaver(datas[i]))
                    }
                });
            }

            return methods;
        }
    }
}
