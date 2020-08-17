using System.Collections;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UnityEngine;

namespace HandyPackage.CodeGeneration
{
    public class PreservedClassData
    {
        public List<FieldDeclarationSyntax> m_PreservedFields = new List<FieldDeclarationSyntax>();
        public List<MethodDeclarationSyntax> m_PreservedMethods = new List<MethodDeclarationSyntax>();
        public List<PropertyDeclarationSyntax> m_PreservedProperties = new List<PropertyDeclarationSyntax>();

        public PreservedClassData(string code)
        {
            if (string.IsNullOrEmpty(code))
                return;

            AttributeParser.ParseAttributes<PreserveDataAttribute>(code, 
                OnFieldWithAttributeParsed, OnMethodWithAttributeParsed, OnPropertyWithAttributeParsed);
        }

        private void OnFieldWithAttributeParsed(FieldDeclarationSyntax field) => m_PreservedFields.Add(field);
        private void OnMethodWithAttributeParsed(MethodDeclarationSyntax method) => m_PreservedMethods.Add(method);
        private void OnPropertyWithAttributeParsed(PropertyDeclarationSyntax property) => m_PreservedProperties.Add(property);
    }
}


