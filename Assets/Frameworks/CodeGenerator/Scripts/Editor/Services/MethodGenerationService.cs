using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UnityEngine;
using System.Linq;

namespace HandyPackage.CodeGeneration
{
    public static class MethodGenerationService
    {
        private static string[] s_AcceptedAsyncReturnTypes = new string[]
        {
            "void", "Task", "UniTask"
        };

        public static MethodDeclarationSyntax CreateMethod(MethodGenerationData data)
        {
            Debug.Assert(!string.IsNullOrEmpty(data.m_MethodName), "Trying to generate a method with null or empty method name!");
            Debug.Assert(!string.IsNullOrEmpty(data.m_MethodReturnType), "Trying to generate a method with null or empty return type!");
            Debug.Assert(data.m_MethodBodyStatements != null && data.m_MethodBodyStatements.Count > 0, "Trying to generate a method with no body!");

            MethodDeclarationSyntax syntax = SyntaxFactory
                .MethodDeclaration(SyntaxFactory.ParseTypeName(data.m_MethodReturnType), data.m_MethodName)
                .AddModifiers(CodeGenerationUtility.CreateProtectionLevelToken(data.m_ProtectionLevel));

            switch (data.m_InheritanceKeyword)
            {
                case FunctionInheritanceKeyword.STATIC: syntax = syntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword)); break;
                case FunctionInheritanceKeyword.OVERRIDE: syntax = syntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.OverrideKeyword)); break;
                case FunctionInheritanceKeyword.VIRTUAL: syntax = syntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.VirtualKeyword)); break;
            }

            if (data.m_IsAsync)
            {
                bool canMakeAsync = false;

                for (int i = 0; i < s_AcceptedAsyncReturnTypes.Length; i++)
                {
                    if (!data.m_MethodReturnType.Contains(s_AcceptedAsyncReturnTypes[i]))
                        continue;

                    canMakeAsync = true;
                    break;
                }

                Debug.Assert(canMakeAsync, "Trying to generate async function but the return type is not supported! Please make the return type either void, Task or UniTask");

                if (canMakeAsync)
                    syntax = syntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.AsyncKeyword));
            }

            syntax = syntax.AddAttributeLists(AttributeGenerationService.CreateAttributeListSyntaxes(data.m_Attributes));

            foreach (var param in data.m_MethodParams)
                syntax = syntax.AddParameterListParameters(CodeGenerationUtility.CreateParameterSyntax(param.m_ParamName, param.m_ParamType));

            foreach (var statement in data.m_MethodBodyStatements)
                syntax = syntax.AddBodyStatements((SyntaxFactory.ParseStatement(statement)));

            return syntax;
        }
    }
}

