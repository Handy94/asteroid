using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HandyPackage.CodeGeneration
{
    public static class LiteralExpressionUtility
    {
        public static LiteralExpressionSyntax CreateLiteralExpression(string variableType, string variableValue)
        {
            if (VariableTypeCheckerUtility.IsVariableBoolean(variableType))
                return SyntaxFactory.LiteralExpression(CreateLiteralExpressionSyntaxKind(variableType, variableValue));

            return SyntaxFactory.LiteralExpression(
                CreateLiteralExpressionSyntaxKind(variableType, variableValue), 
                CreateLiteralExpressionSyntaxToken(variableType, variableValue));
        }

        private static SyntaxKind CreateLiteralExpressionSyntaxKind(string variableType, string initializerValue)
        {
            if (VariableTypeCheckerUtility.IsVariableNumeric(variableType))
                return SyntaxKind.NumericLiteralExpression;
            else if (VariableTypeCheckerUtility.IsVariableString(variableType))
                return SyntaxKind.StringLiteralExpression;
            else if (VariableTypeCheckerUtility.IsVariableBoolean(variableType))
                return initializerValue.Equals("true") ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression;

            return SyntaxKind.DefaultLiteralExpression;
        }

        private static SyntaxToken CreateLiteralExpressionSyntaxToken(string variableType, string initializerValue)
        {
            // doesn't matter for bool or custom types
            if (VariableTypeCheckerUtility.IsVariableInteger(variableType))
                return SyntaxFactory.Literal(int.Parse(initializerValue));
            else if (VariableTypeCheckerUtility.IsVariableFloat(variableType))
                return SyntaxFactory.Literal(float.Parse(initializerValue));
            else if (VariableTypeCheckerUtility.IsVariableDouble(variableType))
                return SyntaxFactory.Literal(double.Parse(initializerValue));

            return SyntaxFactory.Literal(initializerValue);
        }
    }
}


