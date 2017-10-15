﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Utilities
{
    public static class ObjectSerializer
    {
        private static readonly ConcurrentDictionary<Type, Func<object, IDictionary<string, ObjectPropertyValue>>> CompiledPropertyExtractors = new ConcurrentDictionary<Type, Func<object, IDictionary<string, ObjectPropertyValue>>>();
        ////private static readonly ConstructorInfo DictionaryCtor = typeof(Dictionary<string, ObjectPropertyValue>).GetConstructor(new[] { typeof(IComparer<string>) });

        public static IDictionary<string,ObjectPropertyValue> ExtractProperties(object obj)
        {
            Guard.NotNull(obj, nameof(obj));

            var extractor = CompiledPropertyExtractors.GetOrAdd(obj.GetType(), t => CompileObjectPropertyExtractor(t));
            return extractor(obj);
        }

        private static Func<object, IDictionary<string, ObjectPropertyValue>> CompileObjectPropertyExtractor(Type objectType)
        {
            var memebersToExtract = objectType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty)
                .Select(p => new { Name = p.Name, Type = p.PropertyType })
                .Concat(objectType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Select(f => new { Name = f.Name, Type = f.FieldType }))
                .ToList();

            if (memebersToExtract.Count == 0)
            {
                return obj => new Dictionary<string, ObjectPropertyValue>(StringComparer.Ordinal);
            }

            var parameter = Expression.Parameter(typeof(object));
            var result = Expression.Variable(typeof(IDictionary<string, ObjectPropertyValue>));
            var block = new List<Expression>(memebersToExtract.Count + 10)
            {
                Expression.Assign(result, CallFunc(() => new Dictionary<string, ObjectPropertyValue>(StringComparer.Ordinal))),
            };

            var objectPropertyInfoCtor = typeof(ObjectPropertyValue).GetConstructor(new[] { typeof(Type), typeof(object) });
            foreach (var p in memebersToExtract)
            {
                var memberExtractExpression = Expression.PropertyOrField(Expression.TypeAs(parameter, objectType), p.Name);

                var itemExpression = Expression.Variable(typeof(ObjectPropertyValue));

                var addblock = Expression.Block(new[] { itemExpression },
                    Expression.Assign(itemExpression, Expression.New(objectPropertyInfoCtor, Expression.Constant(p.Type), Expression.TypeAs(memberExtractExpression, typeof(object)))),
                    Expression.Call(result, nameof(IDictionary<string, ObjectPropertyValue>.Add), null, Expression.Constant(p.Name), itemExpression));

                block.Add(addblock);
            }

            block.Add(result);
            return Expression.Lambda<Func<object, IDictionary<string, ObjectPropertyValue>>>(Expression.Block(new[] { result }, block), parameter).Compile();
        }

        private static Expression GetPropertyExpression<TSource>(Expression obj, Expression<Func<TSource, object>> propertyFunc)
        {
            return Expression.Property(obj, (PropertyInfo)((MemberExpression)propertyFunc.Body).Member);
        }

        private static Expression CallFunc<T>(Func<T> func)
        {
            Expression<Func<T>> result = () => func();
            return Expression.Invoke(result);
        }
    }
}
