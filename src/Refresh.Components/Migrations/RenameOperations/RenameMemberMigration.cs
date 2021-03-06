﻿using Microsoft.CodeAnalysis;
using Refresh.Components.Visitors.RenameOperations;

namespace Refresh.Components.Migrations.RenameOperations
{
    public class RenameMemberMigration : IMigration
    {
        private readonly FullType _type;
        private readonly string _memberName;
        private readonly string _newMemberName;

        public RenameMemberMigration(FullType type, string memberName, string newMemberName)
        {
            _type = type;
            _memberName = memberName;
            _newMemberName = newMemberName;
        }

        public SyntaxTree Apply(SyntaxTree initialAST, MigrationContext context)
        {
            var visitor = new RenameMemberVisitor(context, _type, _memberName, _newMemberName);
            var ast = visitor.Visit(initialAST.GetRoot());

            return ast.SyntaxTree;
        }
    }
}
