using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HandyPackage.CodeGeneration;

namespace HandyPackage.Editor
{
    public static class PlayerDataCodeGeneratorUtility
    {
        public static string CreateValueReturnType(PlayerDataEditorData data)
        {
            if (VariableTypeCheckerUtility.IsVariableCollection(data.baseDataType))
                return $"List<{data.valueDataType}>";

            if (VariableTypeCheckerUtility.IsVariableDictionary(data.baseDataType))
                return $"Dictionary<{data.keyDataType}, {data.valueDataType}>";

            return data.baseDataType;
        }

        public static bool UsesReactiveProperty(string varType)
        {
            return VariableTypeCheckerUtility.IsVariableCollection(varType) || VariableTypeCheckerUtility.IsVariableDictionary(varType);
        }

        public static string GetPlayerDataVariableReferenceForSaver(PlayerDataEditorData data)
        {
            if (VariableTypeCheckerUtility.IsVariableCollection(data.baseDataType))
                return $"{PlayerDataCodeGeneratorConstants.PLAYER_DATA_REFERENCE_NAME}.{data.key.ToCamelCase(false)}.ToList()";

            if (VariableTypeCheckerUtility.IsVariableDictionary(data.baseDataType))
                return $"{PlayerDataCodeGeneratorConstants.PLAYER_DATA_REFERENCE_NAME}.{data.key.ToCamelCase(false)}.ToDictionary(t => t.Key, t => t.Value)";

            return $"{PlayerDataCodeGeneratorConstants.PLAYER_DATA_REFERENCE_NAME}.{data.key.ToCamelCase(false)}.Value";
        }
    }
}


