using System;
using System.Linq.Expressions;

namespace H.Framework.Core.Mapping
{
    public class Mapping<TSource, TResult>
    {
        public static implicit operator TResult(Mapping<TSource, TResult> m)
        {
            return m.Result();
        }

        public Mapping(Expression<Func<TSource>> source, Expression<Func<TSource, TResult>> target)
        {
            Source = source;
        }

        public Expression<Func<TSource>> Source { get; }

        public Mapping<TSource, TResult> Property<TPropertySource, TPropertyResult>(Expression<Func<TSource, TPropertySource>> property, Expression<Func<TPropertySource, TPropertyResult>> target)
        {
            var x = property.Compile();
            var source = Expression.Lambda<Func<TPropertySource>>(property, null);
            var mapping = new Mapping<TPropertySource, TPropertyResult>(source, target);
            return this;
        }

        public Expression<Func<TResult>> Build()
        {
            //LambdaExpression<Expression<Func<int>>> e;
            return Expression.Lambda<Func<TResult>>(null, null);
        }

        public TResult Result()
        {
            return Build().Compile().Invoke();
        }
    }
}