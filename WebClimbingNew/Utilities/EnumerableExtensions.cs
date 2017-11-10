using System.Collections.Generic;
using System.Linq;

namespace Climbing.Web.Utilities
{
    public static class EnumerableExtensions
    {
        public static ICollection<T> AsCollection<T>(this IEnumerable<T> e)
        {
            if(e == null)
            {
                return null;
            }

            if(e is ICollection<T> cl)
            {
                return cl;
            }

            return e.ToList();
        }
    }
}