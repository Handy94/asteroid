using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UnityEngine;

namespace HandyPackage.CodeGeneration
{
    public static class ClassScriptModifier
    {
        public static string ModifyNamespaceInClass(string code, string newNamespace)
        {
            // Parse the code into a SyntaxTree.
            var root = CodeReaderUtility.GetCompilationUnitSyntaxFromCode(code);

            // this code should be compatible with cases of a class inside another class
            var namespaceSyntax = root.DescendantNodes().FirstOrDefault(x => x is NamespaceDeclarationSyntax) as NamespaceDeclarationSyntax;

            if (namespaceSyntax == null)
            {
                Debug.LogWarning("No namespace found in script to modify");
                return code;
            }

            var newNamespaceSyntax = namespaceSyntax;
            newNamespaceSyntax = newNamespaceSyntax.WithName(SyntaxFactory.ParseName(newNamespace)).NormalizeWhitespace();

            root = root.ReplaceNode(namespaceSyntax, newNamespaceSyntax).NormalizeWhitespace();
            return root.ToFullString();
        }

        /// <summary> Replaces the functions based on data's m_MethodNameToModify that has the same amount of params in the original function.
        /// <para /> This does NOT support making an entirely new function if the function to replace is not found.
        /// <para /> The methodology is to replace the method nodes in the class to edit.
        /// Because replacing a node would recreate the syntax tree, we have to replace the nodes one at a time.
        /// </summary>
        public static string ModifyFunctionsInClass(string code, List<MethodModificationData> datas)
        {
            for (int i = 0; i < datas.Count; i++)
                code = ModifyFunctionInClass(code, datas[i]);

            return code;
        }

        private static string ModifyFunctionInClass(string code, MethodModificationData data)
        {
            var root = CodeReaderUtility.GetCompilationUnitSyntaxFromCode(code);

            // this code should be compatible with cases of a class inside another class
            var classes = root.DescendantNodes().Where(x => x is ClassDeclarationSyntax)
                .Cast<ClassDeclarationSyntax>().ToList();

            for (int i = 0, classCount = classes.Count; i < classCount; i++)
            {
                var newClassSyntax = classes[i];
                var methods = classes[i].DescendantNodes().Where(x => x is MethodDeclarationSyntax)
                    .Cast<MethodDeclarationSyntax>().ToList();

                Debug.Assert(methods != null, "No method to modify in class " + classes[i].Identifier.Value);

                var method = GetOriginalMethodDeclarationSyntax(methods, data);
                if (method == null)
                {
                    Debug.LogWarningFormat("No method of name {0} found in class {1}",
                        data.m_MethodNameToModify, classes[i].Identifier.ValueText);
                    continue;
                }

                var newMethod = GetModifiedMethodDeclarationSyntax(method, data);
                newClassSyntax = newClassSyntax.ReplaceNode(method, newMethod).NormalizeWhitespace();

                root = root.ReplaceNode(classes[i], newClassSyntax).NormalizeWhitespace();
            }

            return root.ToFullString();
        }


        /// <summary> Find the method whose name is equal to method modification data's m_MethodNameToModify. 
        /// After that, check whether those methods have the exact same param count, and then their check for their types and names.</summary>
        private static MethodDeclarationSyntax GetOriginalMethodDeclarationSyntax(IEnumerable<MethodDeclarationSyntax> methods, MethodModificationData modificationData)
        {
            methods = methods.Where(x => x.ParameterList.Parameters.Count == modificationData.m_OriginalMethodParams.Count);

            // progressively filter based on params in the function
            foreach (var param in modificationData.m_OriginalMethodParams)
            {
                methods = methods.Where(x =>
                    x.ParameterList.Parameters.ToFullString().Contains(param.m_ParamType) &&
                    x.ParameterList.Parameters.ToFullString().Contains(param.m_ParamName));
            }

            return methods.SingleOrDefault(x => x.Identifier.ValueText.Equals(modificationData.m_MethodNameToModify));
        }

        /// <summary> Modify the method found in GetOriginalMethodDeclarationSyntax(). </summary>
        private static MethodDeclarationSyntax GetModifiedMethodDeclarationSyntax(MethodDeclarationSyntax method, MethodModificationData modificationData)
        {
            var nodes = method.DescendantNodes().ToList();

            ParameterListSyntax paramListSyntax = SyntaxFactory.ParameterList();

            foreach (var param in modificationData.m_NewMethodParams)
                paramListSyntax = paramListSyntax.AddParameters(CodeGenerationUtility.CreateParameterSyntax(param.m_ParamName, param.m_ParamType));

            method = modificationData.m_ParamModificationType == MethodParameterModificationType.REPLACE_PARAMS
                ? method.WithParameterList(paramListSyntax)
                : method.AddParameterListParameters(paramListSyntax.Parameters.ToArray());

            BlockSyntax blockSyntax = SyntaxFactory.Block();
            var oldStatements = method.Body.Statements.ToList();

            foreach (var statement in modificationData.m_BodyStatements)
            {
                if (modificationData.m_BodyModificationType == MethodBodyModificationType.ADD_OR_REPLACE_BODY)
                {
                    if (oldStatements.Find(x => x.ToFullString().Contains(statement)) != null)
                        continue;
                }

                blockSyntax = blockSyntax.AddStatements(SyntaxFactory.ParseStatement(statement));
            }

            // if replacing the body, the statement in the old function with be completely replaced with the new statement
            method = modificationData.m_BodyModificationType == MethodBodyModificationType.REPLACE_BODY
                ? method.WithBody(blockSyntax)
                : method.AddBodyStatements(blockSyntax.Statements.ToArray());

            return method.NormalizeWhitespace();
        }
    }
}


