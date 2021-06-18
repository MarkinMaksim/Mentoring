using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class IncDecExpressionVisitor : ExpressionVisitor
    {
        public Expression Replace(Expression exp, Dictionary<string, object> replaceDictionary)
        {
            var replaceVisitor = new ReplaceVisitor(replaceDictionary);

            var replacedIncDec = (LambdaExpression)VisitAndConvert(exp, "IncDec");
            var result = replaceVisitor.Replace(replacedIncDec);

            var parametrs = ((LambdaExpression)exp).Parameters.Where(x => !replaceDictionary.ContainsKey(x.Name));

            return Expression.Lambda(result, parametrs) ;
        }


        protected override Expression VisitBinary(BinaryExpression node)
        {

            if (node.Left.NodeType == ExpressionType.Parameter && node.Right.NodeType == ExpressionType.Constant && (int)((ConstantExpression)node.Right).Value == 1)
            {
                return node.NodeType == ExpressionType.Subtract ? Expression.Decrement(node.Left) : Expression.Increment(node.Left);
            }

            return base.VisitBinary(node);
        }
    }
}
