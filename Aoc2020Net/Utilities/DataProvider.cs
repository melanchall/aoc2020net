using System.Collections.Generic;

namespace Aoc2020Net.Utilities
{
    internal static class DataProvider
    {
        public static IEnumerable<(int I, int J)> GetIndicesPairs(int minIndex, int maxIndex)
        {
            for (var i = minIndex; i <= maxIndex - 1; i++)
            {
                for (var j = i + 1; j <= maxIndex; j++)
                {
                    yield return (i, j);
                }
            }
        }
    }
}
