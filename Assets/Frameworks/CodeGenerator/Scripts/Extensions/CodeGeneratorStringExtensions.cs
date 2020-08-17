using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HandyPackage.CodeGeneration
{
    public static class CodeGeneratorStringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

        public static string SeparateString(this string str, char wordSeparator = '_')
        {
            if (string.IsNullOrEmpty(str)) 
                return str;

            string result = str;
            var splitString = result.Split(wordSeparator).Select(x => 
                char.ToUpper(x[0]) + (x.Length > 1 ? x.Substring(1).ToLower() : string.Empty)).ToList();

            splitString.RemoveAll(x => x.Contains("List"));
            splitString.RemoveAll(x => x.Contains("Dict"));

            result = string.Join(string.Empty, splitString);
            // if (!toUpperFirstChar)
            // {
            //     result = char.ToLower(result[0]) + (result.Length > 1 ? result.Substring(1) : string.Empty);
            // }
            return result;
        }

        public static string RemoveHungarianNotation(this string str)
        {
            if (string.IsNullOrEmpty(str)) 
                return str;

            var splitString = Regex.Split(str, @"(?<!^)(?=[A-Z])").ToList();
            splitString.RemoveAt(0);

            return string.Join(string.Empty, splitString);
        }
    }
}

