namespace HandyPackage
{
    using System.Linq;
    public static class PlayerDataStringExtensions
    {
        public static string ToCamelCase(this string str, char separator = '_', bool toUpperFirstChar = true, bool removeGeneratedStrings = true, bool keepOriginalUpperCases = false)
        {
            if (string.IsNullOrEmpty(str)) return str;
            string result = str;

            var splitString = result.Split(separator)
                .Select(x => char.ToUpper(x[0]) + (x.Length > 1
                ? keepOriginalUpperCases ? x.Substring(1) : x.Substring(1).ToLower()
                : string.Empty)).ToList();

            if (removeGeneratedStrings)
            {
                if (splitString.Contains("List"))
                    splitString.RemoveRange(0, 2);
                else if (splitString.Contains("Dict"))
                    splitString.RemoveRange(0, 3);
                else
                    splitString.RemoveAt(0);
            }

            result = string.Join(string.Empty, splitString);
            if (!toUpperFirstChar)
            {
                result = char.ToLower(result[0]) + (result.Length > 1 ? result.Substring(1) : string.Empty);
            }
            return result;
        }

        public static string ToCamelCase(this string str, bool toUpperFirstChar = true, bool removedGeneratedStrings = true, bool keepOriginalUpperCases = false)
        {
            return str.ToCamelCase('_', toUpperFirstChar, removedGeneratedStrings, keepOriginalUpperCases);
        }

        public static string ToCamelCaseAfterSeparator(this string str, char separator = '_', bool toUpperFirstChar = true)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            var splitString = str.Split(separator).Select(x => char.ToUpper(x[0]) + (x.Length > 1 ? x.Substring(1).ToLower() : string.Empty)).ToList();
            splitString.RemoveAt(0);

            return string.Join(string.Empty, splitString);
        }
    }

}