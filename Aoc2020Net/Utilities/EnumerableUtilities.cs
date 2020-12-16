using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2020Net.Utilities
{
    internal static class EnumerableUtilities
    {
        public static int CountValue<T>(this IEnumerable<T> source, T value) => source.Count(e => e.Equals(value));

        public static long Product(this IEnumerable<long> source) => source.Aggregate((x, y) => x * y);

        public static long Product<T>(this IEnumerable<T> source, Func<T, long> selector) => source.Aggregate(1L, (x, y) => x * selector(y));

        public static long Product(this IEnumerable<int> source) => source.Aggregate(1L, (x, y) => x * y);
    }
}
