using System.Collections;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UnityEngine;
using System.Linq;

namespace HandyPackage.CodeGeneration
{
    public static class CodeReaderUtility
    {
        public static CompilationUnitSyntax GetCompilationUnitSyntaxFromCode(string code)
        {
            // Parse the code into a SyntaxTree.
            var tree = CSharpSyntaxTree.ParseText(code);

            // Get the root CompilationUnitSyntax.
            return tree.GetRoot() as CompilationUnitSyntax;
        }

        public static T GetFirstOrDefaultFromRoot<T>(CompilationUnitSyntax root) where T : MemberDeclarationSyntax
        {
            return root.DescendantNodes().FirstOrDefault(x => x is T) as T;
        }

        public static T[] GetArrayFromRoot<T>(CompilationUnitSyntax root) where T : MemberDeclarationSyntax
        {
            return root.DescendantNodes().Where(x => x is T).Cast<T>().ToArray();
        }
    }
}


