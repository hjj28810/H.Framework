using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace H.Framework.Core.Utilities
{
    public class ProjectionExpression<TSource>
    {
        private static readonly Dictionary<string, Expression> ExpressionCache = new Dictionary<string, Expression>();

        private readonly IQueryable<TSource> _source;

        public ProjectionExpression(IQueryable<TSource> source)
        {
            _source = source;
        }

        public IQueryable<TDest> To<TDest>()
        {
            var queryExpression = GetCachedExpression<TDest>() ?? BuildExpression<TDest>();

            return _source.Select(queryExpression);
        }

        private static Expression<Func<TSource, TDest>> GetCachedExpression<TDest>()
        {
            var key = GetCacheKey<TDest>();

            return ExpressionCache.ContainsKey(key) ? ExpressionCache[key] as Expression<Func<TSource, TDest>> : null;
        }

        private static Expression<Func<TSource, TDest>> BuildExpression<TDest>()
        {
            var sourceProperties = typeof(TSource).GetProperties();
            var destinationProperties = typeof(TDest).GetProperties().Where(dest => dest.CanWrite);
            var parameterExpression = Expression.Parameter(typeof(TSource), "src");

            var bindings = destinationProperties
                                .Select(destinationProperty => BuildBinding(parameterExpression, destinationProperty, sourceProperties))
                                .Where(binding => binding != null);

            var expression = Expression.Lambda<Func<TSource, TDest>>(Expression.MemberInit(Expression.New(typeof(TDest)), bindings), parameterExpression);

            var key = GetCacheKey<TDest>();

            ExpressionCache.Add(key, expression);

            return expression;
        }

        private static MemberAssignment BuildBinding(Expression parameterExpression, MemberInfo destinationProperty, IEnumerable<PropertyInfo> sourceProperties)
        {
            var sourceProperty = sourceProperties.FirstOrDefault(src => src.Name == destinationProperty.Name);

            if (sourceProperty != null)
            {
                return Expression.Bind(destinationProperty, Expression.Property(parameterExpression, sourceProperty));
            }
            var propertyNames = SplitTableName(destinationProperty.Name);

            if (propertyNames.Length == 2)
            {
                sourceProperty = sourceProperties.FirstOrDefault(src => src.Name == propertyNames[0]);
                if (sourceProperty != null)
                {
                    var sourceChildProperty = sourceProperty.PropertyType.GetProperties().FirstOrDefault(src => src.Name == propertyNames[1]);

                    if (sourceChildProperty != null)
                    {
                        return Expression.Bind(destinationProperty, Expression.Property(Expression.Property(parameterExpression, sourceProperty), sourceChildProperty));
                    }
                }
            }
            else if (propertyNames.Length == 3)
            {
                var firstSourceProperty = sourceProperties.FirstOrDefault(src => src.Name == propertyNames[0]);
                if (null == firstSourceProperty) return null;
                var secondSourceProperty = firstSourceProperty.PropertyType.GetProperties().FirstOrDefault(src => src.Name == propertyNames[1]);
                if (null == secondSourceProperty) return null;

                var sourceChildProperty = secondSourceProperty.PropertyType.GetProperties().FirstOrDefault(src => src.Name == propertyNames[2]);

                if (sourceChildProperty != null)
                {
                    var secondProp = Expression.Property(parameterExpression, firstSourceProperty);
                    var firstProp = Expression.Property(secondProp, secondSourceProperty);
                    var resultProp = Expression.Property(firstProp, sourceChildProperty);
                    return Expression.Bind(destinationProperty, resultProp);
                }
            }
            else if (propertyNames.Length == 1)
            {
                return null;
            }
            else
            {
                throw new NotSupportedException("do not suports this kind of operation");
            }
            return null;
        }

        private static string GetCacheKey<TDest>()
        {
            return string.Concat(typeof(TSource).FullName, typeof(TDest).FullName);
        }

        private static string[] SplitTableName(string input)
        {
            return input.Trim().Split('_');
        }
    }
}