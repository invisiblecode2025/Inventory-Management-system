
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using static Inventory.Shared.Core.Enum.Common;

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

      
        public class ExpressionTreeHelper<T,P>
        {

            public static (object Min, object Max) GetMinMax(IEnumerable<T> collection, Expression<Func<T, object>> propertySelector)
            {
                if (collection == null || !collection.Any())
                {
                    throw new ArgumentException("Collection cannot be null or empty.");
                }
                var selector = propertySelector.Compile();

                var min = collection.Min(selector);
                var max = collection.Max(selector);

                return (min, max);
            }

            public static Expression<Func<T, bool>> BuildPredicate(Expression<Func<T, object>> exp, ExpressionType expresstype, object propertyvalue)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
           
                var binaryExp = exp?.Body as UnaryExpression;

                var propertyName = binaryExp.Operand as MemberExpression;

                var property = Expression.Property(parameter, propertyName?.Member.Name);
                var value = Expression.Constant(propertyvalue);

                BinaryExpression comparison;
            
                switch (expresstype)
                {
                    case ExpressionType.GreaterThan:
                        comparison = Expression.GreaterThan(property, value);
                        break;
                    case ExpressionType.LessThan:
                        comparison = Expression.LessThan(property, value);
                        break;
                    case ExpressionType.GreaterThanOrEqual:
                        comparison = Expression.GreaterThanOrEqual(property, value);
                        break;
                    case ExpressionType.LessThanOrEqual:
                        comparison = Expression.LessThanOrEqual(property, value);
                        break;
                    case ExpressionType.Equal:
                        comparison = Expression.Equal(property, value);
                        break;
                    case ExpressionType.NotEqual:
                        comparison = Expression.NotEqual(property, value);
                        break;
                    
                    default:
                        throw new NotSupportedException($"Operator '{""}' is not supported.");
                       
                }

                return Expression.Lambda<Func<T, bool>>(comparison, parameter);

            }


            public static Expression<Func<T, bool>> BuildPredicate(string condition)
            {
                var parameter = Expression.Parameter(typeof(T), "p");
                var parts = condition.Split(' ');

                if (parts.Length != 3)
                    throw new ArgumentException("Condition must be in the format 'Property Operator Value'.");

                var propertyName = parts[0];
                var operatorString = parts[1];
                var valueString = parts[2];

                var property = Expression.Property(parameter, propertyName);
                var value = Expression.Constant(Convert.ToInt32(valueString), typeof(int)); 

                Expression body;

                switch (operatorString)
                {
                    case ">":
                        body = Expression.GreaterThan(property, value);
                        break;
                    case "<":
                        body = Expression.LessThan(property, value);
                        break;
                    case ">=":
                        body = Expression.GreaterThanOrEqual(property, value);
                        break;
                    case "<=":
                        body = Expression.LessThanOrEqual(property, value);
                        break;
                    case "==":
                        body = Expression.Equal(property, value);
                        break;
                    case "!=":
                        body = Expression.NotEqual(property, value);
                        break;
                    default:
                        throw new NotSupportedException($"Operator '{operatorString}' is not supported.");
                }

                return Expression.Lambda<Func<T, bool>>(body, parameter);
            }
            public static Expression<Func<T, bool>> GetCondition(Expression<Func<T, bool>> exp)
            {
                var parameter = Expression.Parameter(typeof(T), "p");

                var binaryExp = exp?.Body as BinaryExpression;

                var propertyvalue = binaryExp?.Right as ConstantExpression;

                var propertyName = binaryExp?.Left as MemberExpression;

                var property = Expression.Property(parameter, propertyName?.Member.Name);
                var value = Expression.Constant(Convert.ToInt32(propertyvalue?.Value), typeof(int));

                Expression body;

                switch (binaryExp?.NodeType)
                {
                    case ExpressionType.GreaterThan:
                        body = Expression.GreaterThan(property, value);
                        break;
                    case ExpressionType.LessThan:
                        body = Expression.LessThan(property, value);
                        break;
                    case ExpressionType.GreaterThanOrEqual:
                        body = Expression.GreaterThanOrEqual(property, value);
                        break;
                    case ExpressionType.LessThanOrEqual:
                        body = Expression.LessThanOrEqual(property, value);
                        break;
                    case ExpressionType.Equal:
                        body = Expression.Equal(property, value);
                        break;
                    case ExpressionType.NotEqual:
                        body = Expression.NotEqual(property, value);
                        break;
                    default:
                        throw new NotSupportedException($"Operator '{""}' is not supported.");
                }

                return Expression.Lambda<Func<T, bool>>(body, parameter);
            }
        }


    }
}
