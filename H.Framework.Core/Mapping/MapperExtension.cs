using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace H.Framework.Core.Mapping
{
    public static class MapperExtension
    {
        private static object locker;
        private static IDictionary<Tuple<Type, Type>, object> mappingExpressions;
        private static MethodInfo ConvertMethod;

        static MapperExtension()
        {
            locker = new object();
            mappingExpressions = new Dictionary<Tuple<Type, Type>, object>();
            ConvertMethod = typeof(MapperExtension).GetMethod("ConvertValue",
                BindingFlags.Static | BindingFlags.Public);
        }

        private static string GetMappingName(Type sourceType, PropertyInfo prop)
        {
            var list = prop.GetCustomAttributes<MappingSourceAttribute>();

            var attr = list.FirstOrDefault(x => x.SourceType == sourceType);
            if (attr != null)
                return attr.Name;

            attr = list.FirstOrDefault(x => x.SourceType == null);
            if (attr != null)
                return attr.Name;

            return prop.Name;
        }

        public static object ConvertValue(object value, Type type)
        {
            if (value == null)
                return type.IsValueType ? Activator.CreateInstance(type) : null;

            var conv1 = TypeDescriptor.GetConverter(type);
            if (conv1.CanConvertFrom(value.GetType()))
            {
                return conv1.ConvertFrom(value);
            }

            var conv2 = TypeDescriptor.GetConverter(value.GetType());
            if (conv2.CanConvertTo(type))
            {
                return conv2.ConvertTo(value, type);
            }

            return Convert.ChangeType(value, type);
        }

        private static Expression BuildConvertExpression(Expression instance, PropertyInfo source, PropertyInfo target)
        {
            return source.PropertyType == target.PropertyType
                ? (Expression)Expression.Property(instance, source)
                : Expression.Convert(
                    Expression.Call(ConvertMethod,
                        Expression.Convert(
                            Expression.Property(instance, source),
                            typeof(object)),
                        Expression.Constant(target.PropertyType)),
                    target.PropertyType);
        }

        private static Expression<Func<T, TResult, TResult>> BuildMappingExpression<T, TResult>()
        {
            var sourceProps = typeof(T).GetProperties().ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
            var sourceParam = Expression.Parameter(typeof(T), "source");
            var resultParam = Expression.Parameter(typeof(TResult), "result");

            // assign properties
            var lines = typeof(TResult).GetProperties()
                .Where(p => p.CanWrite && !p.IsDefined(typeof(MappingIgnoreAttribute)))
                .ToDictionary(p => GetMappingName(typeof(T), p))
                .Where(kv => sourceProps.ContainsKey(kv.Key))
                .Select(kv => (Expression)Expression.Assign(
                    Expression.Property(resultParam, kv.Value),
                    BuildConvertExpression(sourceParam, sourceProps[kv.Key], kv.Value)))
                .ToList();
            // ICustomMap<>
            var mapType = typeof(ICustomMap<>).MakeGenericType(typeof(T));
            var intfs = typeof(TResult).GetInterfaces();
            if (intfs.Contains(mapType))
            {
                lines.Add(
                    Expression.Call(
                        Expression.Convert(resultParam, mapType),
                        mapType.GetMethod("MapFrom"), sourceParam));
            }
            else if (intfs.Contains(typeof(ICustomMap)))
            {
                lines.Add(
                    Expression.Call(
                        Expression.Convert(resultParam, mapType),
                        mapType.GetMethod("MapFrom"), Expression.Convert(sourceParam, typeof(object))));
            }

            // return result
            lines.Add(resultParam);

            return Expression.Lambda<Func<T, TResult, TResult>>(
                // body
                Expression.Block(lines),
                // parameters
                sourceParam, resultParam);
        }

        public static TResult MapTo<T, TResult>(this T source) where TResult : new()
        {
            return MapTo(source, x => new TResult());
        }

        public static TResult MapTo<T, TResult>(this T source, Func<T, TResult> selector)
        {
            object obj;
            Func<T, TResult, TResult> func;
            var key = new Tuple<Type, Type>(typeof(TResult), typeof(T));
            lock (locker)
            {
                if (!mappingExpressions.TryGetValue(key, out obj))
                {
                    func = BuildMappingExpression<T, TResult>().Compile();
                    mappingExpressions.Add(key, func);
                }
                else
                    func = (Func<T, TResult, TResult>)obj;
            }

            return func(source, selector(source));
        }

        public static IEnumerable<TResult> MapAllTo<T, TResult>(this IEnumerable<T> source) where TResult : new()
        {
            return MapAllTo(source, x => new TResult());
        }

        public static IEnumerable<TResult> MapAllTo<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector)
        {
            object obj;
            Func<T, TResult, TResult> func;
            var key = new Tuple<Type, Type>(typeof(TResult), typeof(T));
            lock (locker)
            {
                if (!mappingExpressions.TryGetValue(key, out obj))
                {
                    func = BuildMappingExpression<T, TResult>().Compile();
                    mappingExpressions.Add(key, func);
                }
                else
                    func = (Func<T, TResult, TResult>)obj;
            }

            return source.Select(x => func(x, selector(x)));
        }

        /// <summary>
        /// 自定义转换函数。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector">返回值选择函数。</param>
        /// <returns></returns>
        [Obsolete("该方法已废弃，请使用 Map 方法。")]
        public static TResult To<T, TResult>(this T source, Func<T, TResult> selector)
        {
            return selector(source);
        }
    }
}