﻿using System.Collections.Generic;
using LibAdapter.Visitors.ReplaceOperations;
using Microsoft.CodeAnalysis;

namespace LibAdapter.Migrations.ReplaceOperations
{
    public class ReplaceClassMigration : IMigration
    {
        private readonly string _type;
        private readonly string _newType;
        private readonly IEnumerable<Argument> _constructorArgument;

        public ReplaceClassMigration(string type, string newType, IEnumerable<Argument> constructorArgument)
        {
            _type = type;
            _newType = newType;
            _constructorArgument = constructorArgument;
        }


        public SyntaxTree Apply(SyntaxTree initialAST, MigrationContext context)
        {
            var visitor = new ReplaceClassVisitor(context, _type, _newType, _constructorArgument);
            var ast = visitor.Visit(initialAST.GetRoot());

            context.ReplaceType(_type, _newType);

            return ast.SyntaxTree;
        }
    }
}
