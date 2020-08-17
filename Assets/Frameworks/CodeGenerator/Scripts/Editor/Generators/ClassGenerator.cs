using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;
using System.Collections.Generic;

namespace HandyPackage.CodeGeneration
{
    public class ClassGenerator
    {
        public static string CreateClass(ClassGenerationData classGenerationData, string codeWithPreservedData = null)
        {
            var preservedData = new PreservedClassData(codeWithPreservedData);
            return CreateClass(classGenerationData, preservedData);
        }

        private static string CreateClass(ClassGenerationData classGenerationData, PreservedClassData preservedClassData)
        {
            //  Create a class: (class TestClass)
            var classDeclaration = CreateClassUsingClassGenerationData(classGenerationData);
            var @namespace = CreateNamespaceDeclarationSyntax(classGenerationData.m_Namespace, classGenerationData.m_Usings);

            if (preservedClassData != null)
                classDeclaration = AppendClassWithPreservedData(classDeclaration, preservedClassData);

            // Add the class to the namespace.
            @namespace = @namespace.AddMembers(classDeclaration);

            // Normalize and get code as string.
            var code = @namespace
                .NormalizeWhitespace()
                .ToFullString();

            return code;
        }

        private static ClassDeclarationSyntax CreateClassUsingClassGenerationData(ClassGenerationData data)
        {
            var classDeclaration = CreateClassDeclarationSyntax(data.m_ClassName, data.m_BaseClasses, data.m_ClassType, data.m_Constraints);
            classDeclaration = classDeclaration.AddAttributeLists(AttributeGenerationService.CreateAttributeListSyntaxes(data.m_ClassAttributes));
            classDeclaration = classDeclaration.AddMembers(FieldGenerationService.CreateFieldDeclarationSyntaxes(data.m_FieldGenerationDatas));

            // a more freeform type of method generation
            for (int i = 0; i < data.m_MethodGenerationDatas.Count; i++)
                classDeclaration = classDeclaration.AddMembers(MethodGenerationService.CreateMethod(data.m_MethodGenerationDatas[i]));

            return classDeclaration;
        }

        private static ClassDeclarationSyntax AppendClassWithPreservedData(ClassDeclarationSyntax classDeclaration, PreservedClassData data)
        {
            if (data.m_PreservedFields.Count > 0)
                classDeclaration = classDeclaration.AddMembers(data.m_PreservedFields.ToArray());

            if (data.m_PreservedMethods.Count > 0)
                classDeclaration = classDeclaration.AddMembers(data.m_PreservedMethods.ToArray());

            if (data.m_PreservedProperties.Count > 0)
                classDeclaration = classDeclaration.AddMembers(data.m_PreservedProperties.ToArray());

            return classDeclaration;
        }

        /// <summary> Create the namespace that contains the class(es). In this function, the usings are also created. </summary>
        private static NamespaceDeclarationSyntax CreateNamespaceDeclarationSyntax(string namespaceName, string[] usings)
        {
            // Create a namespace: (namespace CodeGenerationSample)
            NamespaceDeclarationSyntax @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(namespaceName)).NormalizeWhitespace();

            // Add System using statement: (using System)
            List<UsingDirectiveSyntax> usingDirectiveSyntaxes = CreateUsingDirectiveSyntaxes(usings);

            for (int i = 0; i < usingDirectiveSyntaxes.Count; i++)
                @namespace = @namespace.AddUsings(usingDirectiveSyntaxes[i]);

            return @namespace;
        }

        private static List<UsingDirectiveSyntax> CreateUsingDirectiveSyntaxes(string[] usings)
        {
            var syntaxes = new List<UsingDirectiveSyntax>();

            for (int i = 0; i < usings.Length; i++)
                syntaxes.Add(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(usings[i])));

            return syntaxes;
        }

        private static ClassDeclarationSyntax CreateClassDeclarationSyntax(string className, string[] baseClasses, ClassType classType, List<ConstraintData> constraints)
        {
            ClassDeclarationSyntax syntax = SyntaxFactory.ClassDeclaration(className);

            List<SyntaxToken> tokens = new List<SyntaxToken>();
            tokens.Add(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            switch (classType)
            {
                case ClassType.Standard: break;
                case ClassType.Partial: tokens.Add(SyntaxFactory.Token(SyntaxKind.PartialKeyword)); break;
                case ClassType.Static: tokens.Add(SyntaxFactory.Token(SyntaxKind.StaticKeyword)); break;
                case ClassType.StaticPartial:
                    tokens.Add(SyntaxFactory.Token(SyntaxKind.StaticKeyword));
                    tokens.Add(SyntaxFactory.Token(SyntaxKind.PartialKeyword));
                    break;
            }

            // Add the public modifier: (public class Order)
            syntax = syntax.AddModifiers(tokens.ToArray());

            // Inherit BaseEntity<T> and implement IHaveIdentity: (public class Order : BaseEntity<T>, IHaveIdentity)
            for (int i = 0; i < baseClasses.Length; i++)
                syntax = syntax.AddBaseListTypes(SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(baseClasses[i])));

            for (int i = 0; i < constraints.Count; i++)
                syntax = syntax.AddConstraintClauses(CodeGenerationUtility.CreateConstraintClause(constraints[i]));

            return syntax;
        }
    }
}