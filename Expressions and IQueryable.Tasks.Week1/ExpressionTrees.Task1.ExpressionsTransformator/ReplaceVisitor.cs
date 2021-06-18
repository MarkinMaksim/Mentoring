using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class ReplaceVisitor : ExpressionVisitor
    {
        private IDictionary<string, object> _replaceDictionary;

        public ReplaceVisitor(IDictionary<string, object> replaceDictionary)
        {
            _replaceDictionary = replaceDictionary;
        }

        public Expression Replace(LambdaExpression exp)
        {
            var replaced = Visit(exp.Body);

            return replaced;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_replaceDictionary.ContainsKey(node.Name))
            {
                var obj = _replaceDictionary[node.Name];
                return Expression.Constant(obj);
            }

            return base.VisitParameter(node);
        }
    }
}
