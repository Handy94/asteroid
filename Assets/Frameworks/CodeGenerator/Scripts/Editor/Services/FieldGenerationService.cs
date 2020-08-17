using System.Collections;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UnityEngine;

namespace HandyPackage.CodeGeneration
{
    public static class FieldGenerationService
    {
        /// <summary> Creates the fields in the generated class. Such as "private int testInt = 0" </summary>
        public static FieldDeclarationSyntax[] CreateFieldDeclarationSyntaxes(List<FieldGenerationData> datas)
        {
            var syntaxes = new List<FieldDeclarationSyntax>();

            for (int i = 0; i < datas.Count; i++)
            {
                // Variable Type: bool, Variable Name: Cancelled. Created variable => bool Cancelled
                VariableDeclarationSyntax variableSyntax = SyntaxFactory
                    .VariableDeclaration(SyntaxFactory.ParseTypeName(datas[i].m_VariableType))
                    .AddVariables(CreateVariableDeclaratorSyntax(datas[i]));

                // With Protection Level = bool Cancelled => private bool Cancelled
                syntaxes.Add(SyntaxFactory.FieldDeclaration(variableSyntax)
                    .AddModifiers(CodeGenerationUtility.CreateProtectionLevelToken(datas[i].m_ProtectionLevel))
                    .AddModifiers(CreateStaticFieldModifierTokens(datas[i])));
            }

            return syntaxes.ToArray();
        }

        /// <summary> 
        /// Declares the variables in the field along with their initializers. 
        /// <para>In the example "private int testInt = 0", "testInt" would be the variable. </para>
        /// </summary>
        private static VariableDeclaratorSyntax CreateVariableDeclaratorSyntax(FieldGenerationData data)
        {
            VariableDeclaratorSyntax declaratorSyntax = SyntaxFactory.VariableDeclarator(data.m_VariableName);

            if (!data.m_UseInitializer)
                return declaratorSyntax;

            if (VariableTypeCheckerUtility.IsVariableInitializableWithoutNewKeyword(data.m_VariableType))
                return declaratorSyntax.WithInitializer(CreateFieldInitializer(data));
            else
                return declaratorSyntax.WithInitializer(CreateNewFieldInitializer(data));
        }

        /// <summary> Create a field initializer without the new keyword. Example: int a = 0 </summary>
        private static EqualsValueClauseSyntax CreateFieldInitializer(FieldGenerationData data)
        {
            return SyntaxFactory.EqualsValueClause(LiteralExpressionUtility.CreateLiteralExpression(data.m_VariableType, data.m_InitializerValue));
        }

        /// <summary> Create a field initializer with new keyword. Example: IntReactiveProperty a = new IntReactiveProperty()</summary>
        private static EqualsValueClauseSyntax CreateNewFieldInitializer(FieldGenerationData data)
        {
            var arguments = SyntaxFactory.ArgumentList();

            if (!string.IsNullOrEmpty(data.m_InitializerValue) || VariableTypeCheckerUtility.IsVariableBoolean(data.m_VariableType))
                arguments = arguments.AddArguments(SyntaxFactory.Argument(LiteralExpressionUtility.CreateLiteralExpression(data.m_VariableType, data.m_InitializerValue)));

            return SyntaxFactory.EqualsValueClause(SyntaxFactory.ObjectCreationExpression(
                NameSyntaxUtility.GetNameSyntax(data.m_VariableType), arguments, null));
        }

        private static SyntaxToken[] CreateStaticFieldModifierTokens(FieldGenerationData data)
        {
            var tokens = new List<SyntaxToken>();

            switch (data.m_StaticModifier)
            {
                case StaticFieldType.Static: tokens.Add(SyntaxFactory.Token(SyntaxKind.StaticKeyword)); break;
                case StaticFieldType.Const: tokens.Add(SyntaxFactory.Token(SyntaxKind.ConstKeyword)); break;
                case StaticFieldType.StaticReadOnly:
                    tokens.Add(SyntaxFactory.Token(SyntaxKind.StaticKeyword));
                    tokens.Add(SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword));
                    break;
            }

            return tokens.ToArray();
        }
    }
}


