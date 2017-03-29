using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LinqDynamic = System.Linq.Dynamic;

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

        /// <summary>
        /// Convert string format of Linq.Dynamic to expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="stringFilter"></param>
        /// <returns></returns>
        public static Expression<Func<T, TResult>> Convert<T, TResult>(string stringFilter)
        {
            if (stringFilter == null)
            {
                return null;
            }
            //Transform stringFilter to the string accepted by Dynamic.Linq
            var items = stringFilter.Split(' ');
            List<object> values = new List<object>();
            for (var i = 2; i < items.Length; i = i + 4)
            {
                var key = items[i - 2];
                var type = typeof(T).GetProperty(key).PropertyType;
                values.Add(System.Convert.ChangeType(items[i], type));
                items[i] = $"@{(i - 2) / 4}"; ;
            }
            var newfilter = items.Aggregate((first, second) => {
                return $"{first} {second}";
            });
            return LinqDynamic.DynamicExpression.ParseLambda<T, TResult>(newfilter, values.ToArray());
        }
    }
}
