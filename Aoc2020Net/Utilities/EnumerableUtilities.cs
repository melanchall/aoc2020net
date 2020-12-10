using System.Collections.Generic;
using System.Linq;

namespace Aoc2020Net.Utilities
{
    internal static class EnumerableUtilities
    {
        public static int CountValue<T>(this IEnumerable<T> source, T value) => source.Count(e => e.Equals(value));
    }
}
