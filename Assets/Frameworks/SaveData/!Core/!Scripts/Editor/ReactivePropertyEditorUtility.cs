using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HandyPackage.CodeGeneration;

namespace HandyPackage.Editor
{
    public static class ReactivePropertyEditorUtility
    {
        public static string CreateReactivePropertyType(PlayerDataEditorData data)
        {
            return VariableTypeCheckerUtility.IsVariableCollection(data.baseDataType)
                ? CreateReactiveCollectionPropertyType(data.valueDataType) : VariableTypeCheckerUtility.IsVariableDictionary(data.baseDataType)
                ? CreateReactiveDictionaryPropertyType(data.keyDataType, data.valueDataType) 
                : CreateStandardReactivePropertyType(data.baseDataType);
        }

        private static string CreateStandardReactivePropertyType(string variableType) => variableType.FirstCharToUpper() + "ReactiveProperty";
        private static string CreateReactiveCollectionPropertyType(string variableType) => $"ReactiveCollection<{variableType}>";
        private static string CreateReactiveDictionaryPropertyType(string key, string value) => $"ReactiveDictionary<{key}, {value}>";
    }
}

