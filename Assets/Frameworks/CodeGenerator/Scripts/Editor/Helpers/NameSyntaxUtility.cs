using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HandyPackage.CodeGeneration
{
    public static class NameSyntaxUtility
    {
        /// <summary> A generic name syntax that can be an IdentifierNameSyntax or QualifiedNameSyntax.
        /// In this CodeGenerator, it is used in making Attribute and Field Variable names. </summary>
        public static NameSyntax GetNameSyntax(string str)
        {
            if (string.IsNullOrEmpty(str))
                throw new System.Exception("String to make into NameSyntax is null or empty!");

            var splitString = str.Split('.');

            if (splitString.Length == 1)
                return GetIdentifierNameSyntax(str);
            else
                return GetQualifiedNameSyntax(splitString);
        }

        /// <summary> For attribute arguments because they do not accept a generic NameSyntax. 
        /// <para>This is used for a constructor argument. For example: [Attribute(Test: true)]</para></summary>
        public static NameEqualsSyntax GetNameEqualsSyntax(string str)
        {
            return SyntaxFactory.NameEquals(GetIdentifierNameSyntax(str));
        }

        /// <summary> For attribute arguments because they do not accept a generic NameSyntax. 
        /// <para>This is used for a NON-constructor argument. For example: [Attribute(test = true)]</para></summary> 
        public static NameColonSyntax GetNameColonSyntax(string str)
        {
            return SyntaxFactory.NameColon(GetIdentifierNameSyntax(str));
        }

        /// <summary>
        /// A QualifiedNameSyntax is a syntax created using a Qualified/IdentifierName Syntax + an IdentifierNameSyntax.
        /// The first two separated words are created using 2 IdentifierNameSyntaxes, while the third and so on (if needed)
        /// are added onto the created QualifiedNameSyntax.
        /// <para />For Example: "System.Serializable" is a QualifiedNameSyntax created from 2 IdentifierNameSyntaxes "System" and "Serializable".
        /// <para />Meanwhile, "System.Serializable.ABCD" is created from QualifiedNameSyntax "System.Serializable" + "IdentifierNameSyntax "ABCD".
        /// </summary>
        private static QualifiedNameSyntax GetQualifiedNameSyntax(string[] splitString)
        {
            var qualifiedName = SyntaxFactory.QualifiedName(
                GetIdentifierNameSyntax(splitString[0]), 
                GetIdentifierNameSyntax(splitString[1]));

            if (splitString.Length == 2)
                return qualifiedName;

            for (int i = 2; i < splitString.Length; i++)
                qualifiedName = SyntaxFactory.QualifiedName(qualifiedName, GetIdentifierNameSyntax(splitString[i]));

            return qualifiedName;
        }

        /// <summary> 
        /// Creates a name syntax that takes in only one (1) string parameter
        /// Use this function if the string is not split (Eg: Just "Serializable" instead of "System.Serializable")
        /// </summary>
        private static IdentifierNameSyntax GetIdentifierNameSyntax(string str)
        {
            return SyntaxFactory.IdentifierName(str);
        }
    }
}


