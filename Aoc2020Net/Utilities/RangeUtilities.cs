using System;

namespace Aoc2020Net.Utilities
{
    internal static class RangeUtilities
    {
        public static bool ContainsValue(this Range range, int value) => value >= range.Start.Value && value <= range.End.Value;
    }
}
