using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Utilities
{
    public static class PropertyHelper
    {
        private static readonly ConcurrentDictionary<string, Delegate> Delegates = new ConcurrentDictionary<string, Delegate>(StringComparer.Ordinal);

        public static TResult SetProperty<TObject, TResult>(this TObject obj, Expression<Func<TObject, TResult>> property, TResult value)
        {
            Guard.NotNull(property, nameof(property));

            var member = ((MemberExpression)property.Body).Member;
            var action = (Action<TObject, TResult>)Delegates.GetOrAdd(
                $"{typeof(TObject).FullName}|{member.Name}",
                key => Compile<TObject, TResult>(member));

            action(obj, value);
            return value;
        }

        private static Action<TObject, TResult> Compile<TObject, TResult>(MemberInfo member)
        {
            var parameter = Expression.Parameter(typeof(TObject));
            var assignParameter = Expression.Parameter(typeof(TResult));
            var propertyExpession = Expression.PropertyOrField(parameter, member.Name);
            
            BinaryExpression assign;
            try
            {
                assign = Expression.Assign(propertyExpession, assignParameter);
            }
            catch(ArgumentException) when (member is FieldInfo fi)
            {
                return new Action<TObject, TResult>((obj, v) => fi.SetValue(obj, v));
            }
            
            return Expression.Lambda<Action<TObject, TResult>>(assign, parameter, assignParameter).Compile();
        }
    }
}