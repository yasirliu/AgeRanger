using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AgeRanger.Dtos.QueryExpressionMappers
{
    public class DtoExpressionMapper
    {
        class ConversionVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression newParameter;
            private readonly ParameterExpression oldParameter;

            public ConversionVisitor(ParameterExpression newParameter, ParameterExpression oldParameter)
            {
                this.newParameter = newParameter;
                this.oldParameter = oldParameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return newParameter; // replace all old param references with new ones
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                if (node.Expression != oldParameter) // if instance is not old parameter - do nothing
                {
                    return base.VisitMember(node);
                }

                var newObj = Visit(node.Expression);
                var newMember = newParameter.Type.GetMember(node.Member.Name).FirstOrDefault();
                if (newMember == null)
                {
                    return base.VisitMember(node);
                }
                return Expression.MakeMemberAccess(newObj, newMember);
            }
        }

        /// <summary>
        /// Convert expression from type [TFrom] to type [TTo]
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static Expression<Func<TTo, TResult>> Convert<TFrom, TTo, TResult>(Expression<Func<TFrom, TResult>> e)
        {
            if (e == null)
            {
                return null ;
            }
            var oldParameter = e.Parameters[0];
            var newParameter = Expression.Parameter(typeof(TTo), oldParameter.Name);
            var converter = new ConversionVisitor(newParameter, oldParameter);
            try
            {
                var newBody = converter.Visit(e.Body);
                return Expression.Lambda<Func<TTo, TResult>>(newBody, newParameter);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }


        public static Expression<Func<T, TResult>> Convert<T, TResult>(string jsonString)
        {
            return null;
        }
    }
}
