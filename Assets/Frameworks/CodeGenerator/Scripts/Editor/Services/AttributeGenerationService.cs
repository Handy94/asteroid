using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HandyPackage.CodeGeneration
{
    public static class AttributeGenerationService
    {
        /// <summary>
        /// Create all the attributes that are used by the class. Such as [System.Serializable].
        /// A single attribute is considered a single attribute list syntax.
        /// </summary>
        public static AttributeListSyntax[] CreateAttributeListSyntaxes(List<AttributeGenerationData> datas)
        {
            var syntaxes = new List<AttributeListSyntax>();

            for (int i = 0, count = datas.Count; i < count; i++)
                syntaxes.Add(GetAttributeListSyntax(datas[i]));

            return syntaxes.ToArray();
        }

        private static AttributeListSyntax GetAttributeListSyntax(AttributeGenerationData data)
        {
            return SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(GetAttributeSyntax(data)));
        }

        private static AttributeSyntax GetAttributeSyntax(AttributeGenerationData data)
        {
            var syntax = SyntaxFactory.Attribute(NameSyntaxUtility.GetNameSyntax(data.m_AttributeName));

            if (data.m_AttributeArguments != null && data.m_AttributeArguments.Count > 0)
                syntax = syntax.WithArgumentList(GetAttributeArgumentListSyntax(data.m_AttributeArguments));
            
            return syntax;
        }

        private static AttributeArgumentListSyntax GetAttributeArgumentListSyntax(List<AttributeArgumentData> arguments)
        {
            var argumentListSyntax = SyntaxFactory.AttributeArgumentList();

            for (int i = 0, count = arguments.Count; i < count; i++)
                argumentListSyntax = argumentListSyntax.AddArguments(GetAttributeArgumentSyntax(arguments[i]));

            return argumentListSyntax;
        }

        private static AttributeArgumentSyntax GetAttributeArgumentSyntax(AttributeArgumentData data)
        {
            var argumentSyntax = SyntaxFactory.AttributeArgument(LiteralExpressionUtility.CreateLiteralExpression(data.m_ArgumentType, data.m_ArgumentValue));

            if (data.m_IsPartOfConstructor)
                argumentSyntax = argumentSyntax.WithNameEquals(NameSyntaxUtility.GetNameEqualsSyntax(data.m_ArgumentName));
            else
                argumentSyntax = argumentSyntax.WithNameColon(NameSyntaxUtility.GetNameColonSyntax(data.m_ArgumentName));

            return argumentSyntax;
        }
    }
}


