using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Collections.Generic;

namespace HandyPackage.CodeGeneration
{
    public class AttributeParser
    {
        public static void ParseAttributes<TAttribute>(string code, 
            Action<FieldDeclarationSyntax> fieldParser, 
            Action<MethodDeclarationSyntax> methodParser, 
            Action<PropertyDeclarationSyntax> propertyParser) where TAttribute : Attribute
        {
            var root = GetCompilationUnitSyntax(code);

            var membersWithAttribute = root.DescendantNodes().OfType<AttributeListSyntax>()
                .Where(x => IsAttributeInsideAttributeList<TAttribute>(x));

            var fieldsWithAttribute = GetClassMembersWithAttribute<FieldDeclarationSyntax>(membersWithAttribute);
            var propertiesWithAttribute = GetClassMembersWithAttribute<PropertyDeclarationSyntax>(membersWithAttribute);
            var methodsWithAttribute = GetClassMembersWithAttribute<MethodDeclarationSyntax>(membersWithAttribute);

            for (int i = 0, count = fieldsWithAttribute.Count; i < count; i++)
                fieldParser?.Invoke(fieldsWithAttribute[i]);

            for (int i = 0, count = methodsWithAttribute.Count; i < count; i++)
                methodParser?.Invoke(methodsWithAttribute[i]);

            for (int i = 0, count = propertiesWithAttribute.Count; i < count; i++)
                propertyParser?.Invoke(propertiesWithAttribute[i]);
        }

        private static List<TSyntax> GetClassMembersWithAttribute<TSyntax>(IEnumerable<AttributeListSyntax> membersWithAttribute) 
            where TSyntax : MemberDeclarationSyntax
        {
            return membersWithAttribute
                .Where(x => x.Parent is TSyntax)
                .Select(x => x.Parent as TSyntax)
                .ToList();
        }

        private static bool IsAttributeInsideAttributeList<T>(AttributeListSyntax attributeList) where T : Attribute
        {
            var requiredName = typeof(T).Name;
            foreach (var attribute in attributeList.Attributes)
            {
                string attributeName = string.Concat(attribute.Name.ToFullString(), "Attribute");

                if (attributeName.Equals(requiredName))
                    return true;
            }
            
            return false;
        }

        private static void ParseFieldWithAttribute(FieldDeclarationSyntax fieldc)
        {
            // var variable = field.Declaration;
            
            // // string test;
            // string varType = variable.Type.ToFullString();
            // string varName = variable.Variables.First().ToFullString();

            // UnityEngine.Debug.Log(variable.Variables.First().Initializer.ToFullString());

            // parserFunc(varType, varName);
        }

        private static void ParseMethodWithAttribute(MethodDeclarationSyntax method)
        {

        }

        private static CompilationUnitSyntax GetCompilationUnitSyntax(string code)
        {
            // Parse the code into a SyntaxTree.
            var tree = CSharpSyntaxTree.ParseText(code);

            // Get the root CompilationUnitSyntax.
            return tree.GetRoot() as CompilationUnitSyntax;
        }
    }
}

