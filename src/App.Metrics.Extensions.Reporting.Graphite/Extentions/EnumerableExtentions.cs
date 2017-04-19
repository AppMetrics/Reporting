using System.Collections.Generic;
using System.Linq;

namespace System.Linq
{
    internal static class EnumerableExtentions
    {
#if NET452
        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T value)
        {
            return source.Concat(new []{value});
        }
#endif
    }
}
