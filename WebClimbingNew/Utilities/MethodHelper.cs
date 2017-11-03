using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Climbing.Web.Utilities
{
    public static class MethodHelper
    {
        private static readonly ConcurrentDictionary<string, Func<object, object>> PropertyExtractors = new ConcurrentDictionary<string, Func<object, object>>(StringComparer.Ordinal);

        public static object GetPropertyOrFieldValue(object obj, string propertyOrFieldName)
        {
            Guard.NotNull(obj, nameof(obj));
            Guard.NotNullOrWhitespace(propertyOrFieldName, nameof(propertyOrFieldName));

            var func = PropertyExtractors.GetOrAdd($"{obj.GetType().FullName}|{propertyOrFieldName}", key => CompilePropertyExtractor(obj.GetType(), propertyOrFieldName));
            return func(obj);
        }

        private static Func<object,object> CompilePropertyExtractor(Type objectType, string propertyName)
        {
            var objParameter = Expression.Parameter(typeof(object));
            var getValueExpression = Expression.PropertyOrField(Expression.TypeAs(objParameter, objectType), propertyName);
            var callExpression = Expression.Lambda<Func<object, object>>(Expression.TypeAs(getValueExpression, typeof(object)), objParameter);
            return callExpression.Compile();
        }
    }
}
