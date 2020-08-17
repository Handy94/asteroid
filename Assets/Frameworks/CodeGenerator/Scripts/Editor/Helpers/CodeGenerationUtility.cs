using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HandyPackage.CodeGeneration
{
    public static class CodeGenerationUtility
    {
        public static SyntaxToken CreateProtectionLevelToken(ProtectionLevel protectionLevel) => SyntaxFactory.Token(
                protectionLevel == ProtectionLevel.Public
                ? SyntaxKind.PublicKeyword : protectionLevel == ProtectionLevel.Protected
                ? SyntaxKind.ProtectedKeyword : SyntaxKind.PrivateKeyword);

        public static TypeParameterConstraintClauseSyntax CreateConstraintClause(ConstraintData data)
        {
            var constraints = SyntaxFactory.SeparatedList<TypeParameterConstraintSyntax>();
            constraints = constraints.Add(SyntaxFactory.TypeConstraint(SyntaxFactory.ParseTypeName(data.m_ConstraintBaseType)));
            return SyntaxFactory.TypeParameterConstraintClause(SyntaxFactory.IdentifierName(data.m_ConstraintIdentifier), constraints);
        }

        public static ParameterSyntax CreateParameterSyntax(string identifier, string typeName) => 
            SyntaxFactory.Parameter(SyntaxFactory.Identifier(identifier)).WithType(SyntaxFactory.ParseTypeName(typeName));
    }
}

