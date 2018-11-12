using System.Collections.Generic;
using System.Linq;

namespace EDBitor.Parsers
{
    static class CollectionExtentions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                return false;

            var collection = enumerable as ICollection<T>;
            if (collection != null)
                return collection.Count == 0;

            return !enumerable.Any();
        }
    }
}
