﻿using System.Collections.Generic;
using System.Linq;
using LibAdapter.Migrations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace LibAdapter.Visitors.MethodSignatureOperations
{
    public class RemoveArgumentsVisitor : MethodInvocationVisitor
    {
        private readonly Method _method;
        private readonly IEnumerable<int> _positions;

        public RemoveArgumentsVisitor(
            MigrationContext context,
            Method method,
            IEnumerable<int> positions) : base(context)
        {
            _method = method;
            _positions = positions;
        }

        public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax invocation)
        {
            invocation = (InvocationExpressionSyntax)base.VisitInvocationExpression(invocation);
            if (InvocationMatches(invocation, _method.Type, _method.Name))
            {
                InvocationExpressionSyntax newInvocation = invocation;

                var argList = new List<ArgumentSyntax>();
                argList.AddRange(invocation.ArgumentList.Arguments);

                var itemsToRemove = _positions
                    .Select(position => argList[position - 1])
                    .ToList();

                foreach (var item in itemsToRemove)
                {
                    argList.Remove(item);
                }
                

                var argListSyntax = ArgumentList(SeparatedList(argList));
                argListSyntax = argListSyntax.WithCloseParenToken(argListSyntax.CloseParenToken
                    .WithTrailingTrivia(invocation.ArgumentList.CloseParenToken.TrailingTrivia)
                    .WithLeadingTrivia(invocation.ArgumentList.CloseParenToken.LeadingTrivia));

                newInvocation = newInvocation.WithArgumentList(argListSyntax);

                invocation = invocation.CopyAnnotationsTo(newInvocation);
            }
            return invocation;
        }
    }
}
