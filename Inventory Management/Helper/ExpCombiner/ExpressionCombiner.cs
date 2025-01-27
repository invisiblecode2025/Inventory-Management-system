using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Core.Common.ExpCombiner
{
    public static class ExpressionCombiner
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>>? exp, Expression<Func<T, bool>>? newExp)
        {
            Expression<Func<T, bool>>? expnull = null;

            if (newExp != null && exp != null)
            {

                var visitor = new ParameterUpdateVisitor(newExp.Parameters.First(), exp.Parameters.First());

                newExp = visitor.Visit(newExp) as Expression<Func<T, bool>>;
                var binExp = Expression.And(exp.Body, newExp.Body);
                // and return a new lambda, that will do what you want. NOTE that the binExp has reference only to te newExp.Parameters[0] (there is only 1) parameter, and no other
                return Expression.Lambda<Func<T, bool>>(binExp, newExp.Parameters);

            }
            else
            {
                expnull = exp == null ? newExp : exp;

                if (expnull == null)
                    expnull = newExp == null ? exp : newExp;

                return expnull;
            }
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>>? exp, Expression<Func<T, bool>>? newExp)
        {
            Expression<Func<T, bool>>? expnull = null;

            if (newExp != null && exp != null)
            {

                var visitor = new ParameterUpdateVisitor(newExp.Parameters.First(), exp.Parameters.First());

                newExp = visitor.Visit(newExp) as Expression<Func<T, bool>>;
                var binExp = Expression.Or(exp.Body, newExp.Body);
                // and return a new lambda, that will do what you want. NOTE that the binExp has reference only to te newExp.Parameters[0] (there is only 1) parameter, and no other
                return Expression.Lambda<Func<T, bool>>(binExp, newExp.Parameters);

            }
            else
            {
                expnull = exp == null ? newExp : exp;

                if (expnull == null)
                    expnull = newExp == null ? exp : newExp;

                return expnull;
            }
        }

        class ParameterUpdateVisitor : ExpressionVisitor
        {
            private ParameterExpression _oldParameter;
            private ParameterExpression _newParameter;

            public ParameterUpdateVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
            {
                _oldParameter = oldParameter;
                _newParameter = newParameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (object.ReferenceEquals(node, _oldParameter))
                    return _newParameter;

                return base.VisitParameter(node);
            }
        }
    }
}
