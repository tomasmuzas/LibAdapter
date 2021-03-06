﻿using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Refresh.Components.Visitors.MethodSignatureOperations;

namespace Refresh.Components.Migrations.MethodSignatureOperations
{
    public class ReorderArgumentsMigration : IMigration
    {
        private readonly Method _method;
        private readonly List<int> _order;

        public ReorderArgumentsMigration(Method method, List<int> order)
        {
            _method = method;
            _order = order;
        }

        public SyntaxTree Apply(SyntaxTree initialAST, MigrationContext context)
        {
            var visitor = new ReorderMethodArgumentsVisitor(context, _method.Type, _method.Name, _order.ToArray());
            var ast = visitor.Visit(initialAST.GetRoot());

            return ast.SyntaxTree;
        }
    }
}
