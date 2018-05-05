namespace Climbing.Web.Utilities.Mapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public sealed class AutoMapSetup<TSourceObject, TProperty>
    {
        private readonly Expression<Func<TSourceObject, TProperty>> source;

        internal AutoMapSetup(Expression<Func<TSourceObject, TProperty>> source)
        {
            this.source = source;
        }

        public void To<TTargetObject>(Expression<Func<TTargetObject, TProperty>> targetExpression)
        {
            Guard.NotNull(targetExpression, nameof(targetExpression));
            if (!(targetExpression.Body is MemberExpression mex))
            {
                throw new ArgumentException(nameof(MemberExpression) + " required.", nameof(targetExpression));
            }

            var sourceParameter = Expression.Parameter(typeof(object));
            var targetParameter = Expression.Parameter(typeof(TTargetObject));

            var getExpression = Expression.Invoke(this.source, Expression.Convert(sourceParameter, typeof(TSourceObject)));
            var setExpression = Expression.Assign(Expression.PropertyOrField(targetParameter, mex.Member.Name), getExpression);

            var result = Expression.Lambda<Action<object, TTargetObject>>(setExpression, sourceParameter, targetParameter);

            var key = AutoMapper.GetKey<TSourceObject, TTargetObject>();
            var collection = AutoMapper.MappingFunctions.GetOrAdd(key, _ => (ICollection<Delegate>)new List<Delegate>());

            collection.Add(result.Compile());
        }
    }
}