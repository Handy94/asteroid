using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HandyPackage.CodeGeneration
{
    public class VariableTypeCheckerUtility
    {
        public const string VARIABLE_TYPE_INT = "int";
        public const string VARIABLE_TYPE_FLOAT = "float";
        public const string VARIABLE_TYPE_DOUBLE = "double";
        public const string VARIABLE_TYPE_STRING = "string";
        public const string VARIABLE_TYPE_BOOL = "bool";

        public static bool IsVariableNumeric(string variableType) => IsVariableInteger(variableType) || IsVariableFloat(variableType) || IsVariableDouble(variableType);
        public static bool IsVariableCollection(string variableType) => variableType.Contains("[]") || variableType.Contains("List") || variableType.Contains("Collection");
        public static bool IsVariableDictionary(string variableType) => variableType.Contains("Dictionary");

        public static bool IsVariableBoolean(string variableType) => variableType.Contains(VARIABLE_TYPE_BOOL) || variableType.Contains(VARIABLE_TYPE_BOOL.FirstCharToUpper());
        public static bool IsVariableInteger(string variableType) => variableType.Contains(VARIABLE_TYPE_INT) || variableType.Contains(VARIABLE_TYPE_INT.FirstCharToUpper());
        public static bool IsVariableFloat(string variableType) => variableType.Contains(VARIABLE_TYPE_FLOAT) || variableType.Contains(VARIABLE_TYPE_FLOAT.FirstCharToUpper());
        public static bool IsVariableDouble(string variableType) => variableType.Contains(VARIABLE_TYPE_DOUBLE) || variableType.Contains(VARIABLE_TYPE_DOUBLE.FirstCharToUpper());
        public static bool IsVariableString(string variableType) => variableType.Contains(VARIABLE_TYPE_STRING) || variableType.Contains(VARIABLE_TYPE_STRING.FirstCharToUpper());
        public static bool IsVariableCustomType(string variableType) => 
            !IsVariableNumeric(variableType) && !IsVariableString(variableType) && !IsVariableBoolean(variableType);

        public static string FilterDictionaryVariableKeyType(string variableType)
        {
            return variableType.Split('<')[1].Split(',')[0];
        }

        // List<string> => "string"
        // Dictionary<string, int> => "int"
        public static string FilterVariableType(string variableType)
        {
            if (IsVariableCollection(variableType))
            {
                var splitString = variableType.Split('<')[1];
                return splitString.Remove(splitString.Count() - 2);
            }

            if (IsVariableDictionary(variableType))
            {
                var splitString = variableType.Split(',')[1];
                return splitString.Trim(' ').Remove(splitString.Count() - 2);
            }

            return variableType;
        }

        public static bool IsVariableInitializableWithoutNewKeyword(string variableType)
        {
            return variableType.Equals(VARIABLE_TYPE_INT) 
                || variableType.Equals(VARIABLE_TYPE_BOOL) 
                || variableType.Equals(VARIABLE_TYPE_FLOAT)
                || variableType.Equals(VARIABLE_TYPE_STRING);
        }
    }
}


