using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UnityEngine;

namespace HandyPackage.CodeGeneration
{
    public static class ClassMemberExtractor
    {
        public static ClassGenerationData ExtractClassGenerationData(string code)
        {
            var data = new ClassGenerationData();
            var root = CodeReaderUtility.GetCompilationUnitSyntaxFromCode(code);

            var @namespace = root.DescendantNodes().FirstOrDefault(x => x is NamespaceDeclarationSyntax) as NamespaceDeclarationSyntax;
            if (@namespace != null)
                data.m_Namespace = @namespace.Name.ToString();

            var @class = CodeReaderUtility.GetFirstOrDefaultFromRoot<ClassDeclarationSyntax>(root);
            data.m_ClassName = @class.Identifier.ToString();
            data.m_ClassAttributes = ExtractAttributes(@class.AttributeLists.ToArray());

            // TODO: Extract constraints

            data.m_BaseClasses = @class.BaseList.Types.Select(x => x.ToString()).ToArray();

            // this takes in specifically EITHER the namespace's OR the root's usings.
            data.m_Usings = ExtractUsings(@namespace != null ? @namespace.Usings.ToArray() : root.Usings.ToArray()).ToArray();

            var fields = CodeReaderUtility.GetArrayFromRoot<FieldDeclarationSyntax>(root);
            data.m_FieldGenerationDatas = ExtractFields(fields);

            var methods = CodeReaderUtility.GetArrayFromRoot<MethodDeclarationSyntax>(root);
            data.m_MethodGenerationDatas = ExtractMethods(methods);

            return data;
        }

        private static List<string> ExtractUsings(UsingDirectiveSyntax[] usings)
        {
            return usings.Select(x => x.Name.ToString()).ToList();
        }

        private static List<FieldGenerationData> ExtractFields(FieldDeclarationSyntax[] fields)
        {
            var dataList = new List<FieldGenerationData>();

            for (int i = 0; i < fields.Length; i++)
            {
                var declaration = fields[i].Declaration;
                var protectionLevel = ExtractProtectionLevel(fields[i].Modifiers.ToArray());

                bool isConst = fields[i].Modifiers.Any(x => x.Kind() == SyntaxKind.ConstKeyword);
                bool isStatic = fields[i].Modifiers.Any(x => x.Kind() == SyntaxKind.StaticKeyword);
                bool isReadOnly = fields[i].Modifiers.Any(x => x.Kind() == SyntaxKind.ReadOnlyKeyword);

                var staticModifier = isConst ? StaticFieldType.Const
                    : isStatic && isReadOnly ? StaticFieldType.StaticReadOnly
                    : isStatic ? StaticFieldType.Static
                    : StaticFieldType.None;

                foreach (var variable in declaration.Variables)
                {
                    var fieldGenerationData = new FieldGenerationData();
                    fieldGenerationData.m_VariableType = declaration.Type.ToString();
                    fieldGenerationData.m_VariableName = variable.Identifier.ToString();
                    fieldGenerationData.m_ProtectionLevel = protectionLevel;
                    fieldGenerationData.m_StaticModifier = staticModifier;

                    // maybe a bit off for string initializers
                    fieldGenerationData.m_InitializerValue = variable.Initializer?.Value.ToString();
                    fieldGenerationData.m_Attributes = ExtractAttributes(fields[i].AttributeLists.ToArray());

                    dataList.Add(fieldGenerationData);
                }
            }

            return dataList;
        }

        private static List<MethodGenerationData> ExtractMethods(MethodDeclarationSyntax[] methods)
        {
            var dataList = new List<MethodGenerationData>();

            for (int i = 0; i < methods.Length; i++)
            {
                var methodGenerationData = new MethodGenerationData();
                methodGenerationData.m_MethodName = methods[i].Identifier.ToString();
                methodGenerationData.m_MethodReturnType = methods[i].ReturnType.ToString();

                methodGenerationData.m_Attributes = ExtractAttributes(methods[i].AttributeLists.ToArray());
                var modifiers = methods[i].Modifiers.ToArray();

                methodGenerationData.m_IsAsync = modifiers.Any(x => x.Kind() == SyntaxKind.AsyncKeyword);
                methodGenerationData.m_ProtectionLevel = ExtractProtectionLevel(modifiers);
                methodGenerationData.m_InheritanceKeyword = ExtractInheritanceKeyword(modifiers);

                // functions that use arrows like GetID() => ID do not have body statements
                // TODO: Parse such functions correctly
                if (methods[i].Body == null)
                { }
                else
                {
                    var bodyStatements = methods[i].Body.Statements.ToArray();
                    for (int j = 0; j < bodyStatements.Length; j++)
                        methodGenerationData.m_MethodBodyStatements.Add(bodyStatements[j].ToFullString());
                }

                var parameters = methods[i].ParameterList.Parameters.ToArray();
                for (int j = 0; j < parameters.Length; j++)
                    methodGenerationData.m_MethodParams.Add(new MethodParameterData(parameters[j].Type.ToString(), parameters[j].Identifier.ToString()));

                dataList.Add(methodGenerationData);
            }

            return dataList;
        }

        // TODO: Extract Attribute Arguments
        private static List<AttributeGenerationData> ExtractAttributes(AttributeListSyntax[] attributeLists)
        {
            var dataList = new List<AttributeGenerationData>();
            for (int i = 0; i < attributeLists.Length; i++)
            {
                var attributes = attributeLists[i].Attributes.ToArray();

                for (int j = 0; j < attributes.Length; j++)
                {
                    var attributeGenerationData = new AttributeGenerationData();
                    attributeGenerationData.m_AttributeName = attributes[j].Name.ToString();

                    dataList.Add(attributeGenerationData);
                }
            }

            return dataList;
        }

        private static ProtectionLevel ExtractProtectionLevel(SyntaxToken[] tokens)
        {
            for (int i = 0; i < tokens.Length; i++)
            {
                switch (tokens[i].Kind())
                {
                    case SyntaxKind.PublicKeyword: return ProtectionLevel.Public;
                    case SyntaxKind.ProtectedKeyword: return ProtectionLevel.Protected;
                    case SyntaxKind.PrivateKeyword: return ProtectionLevel.Private;
                }
            }

            return ProtectionLevel.Public;
        }

        private static FunctionInheritanceKeyword ExtractInheritanceKeyword(SyntaxToken[] tokens)
        {
            for (int i = 0; i < tokens.Length; i++)
            {
                switch (tokens[i].Kind())
                {
                    case SyntaxKind.StaticKeyword: return FunctionInheritanceKeyword.STATIC;
                    case SyntaxKind.OverrideKeyword: return FunctionInheritanceKeyword.OVERRIDE;
                    case SyntaxKind.VirtualKeyword: return FunctionInheritanceKeyword.VIRTUAL;
                }
            }

            return FunctionInheritanceKeyword.NONE;
        }
    }
}


