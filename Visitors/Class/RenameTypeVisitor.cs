﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace LibAdapter.Visitors.Class
{
    public class RenameTypeVisitor : ClassVisitor
    {
        private string FullTypeName { get; }

        private string NewName { get; }

        public RenameTypeVisitor(SyntaxTypeMap map, string fullTypeName, string newName) : base(map)
        {
            FullTypeName = fullTypeName;
            NewName = newName;
        }

        public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node)
        {
            node = (IdentifierNameSyntax) base.VisitIdentifierName(node);
            if (node != null && MatchesClassType(node, FullTypeName))
            {
                node = node.Update(SyntaxFactory.Identifier(NewName)
                    .WithTrailingTrivia(node.Identifier.TrailingTrivia)
                    .WithLeadingTrivia(node.Identifier.LeadingTrivia));

                node = node.CopyAnnotationsTo(node);
            }

            return node;
        }
    }
}
