using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Climbing.Web.Utilities.Mapper
{
    public static class AutoMapper
    {
        internal static readonly ConcurrentDictionary<string,ICollection<Delegate>> MappingFunctions = new ConcurrentDictionary<string, ICollection<Delegate>>(StringComparer.Ordinal);
        public static AutoMapSetup<TObject, TProperty> Setup<TObject, TProperty>(Expression<Func<TObject, TProperty>> propertyExpression)
        {
            Guard.NotNull(propertyExpression, nameof(propertyExpression));
            
            return new AutoMapSetup<TObject, TProperty>(propertyExpression);
        }

        internal static string GetKey<TSource,TTarget>() => GetKey(typeof(TSource), typeof(TTarget));

        

        public static TTarget Convert<TTarget>(object source) where TTarget : new()
        {
            Guard.NotNull(source, nameof(source));

            var key = GetKey(source.GetType(), typeof(TTarget));
            if(!MappingFunctions.ContainsKey(key))
            {
                throw new ArgumentException("Mapping not set", nameof(source));
            }

            var result = new TTarget();
            foreach(var act in MappingFunctions[key])
            {
                ((Action<object, TTarget>)act).Invoke(source, result);
            }

            return result;
        }

        public static TTarget Map<TSource, TTarget>(TSource source, TTarget target)
        {
            var key = GetKey<TSource, TTarget>();
            if(!MappingFunctions.ContainsKey(key))
            {
                throw new ArgumentException("Mapping not set", nameof(source));
            }

            foreach(var act in MappingFunctions[key])
            {
                ((Action<object, TTarget>)act).Invoke(source, target);
            }

            return target;
        }

        private static string GetKey(Type tsource, Type ttarget) => $"{tsource.FullName}|{ttarget.FullName}";
    }
}