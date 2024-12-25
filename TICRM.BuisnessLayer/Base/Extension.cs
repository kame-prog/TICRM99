using System.Collections.Generic;
using System.Linq;

namespace TICRM.BuisnessLayer.Base
{
    public static class Extensions
    {
        public static IEnumerable<T> CollectionNotNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }
    }
}
