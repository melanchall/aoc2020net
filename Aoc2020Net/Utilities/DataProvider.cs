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

        public static IEnumerable<(int X, int Y)> GetGridCoordinates(int width, int height)
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    yield return (x, y);
                }
            }
        }
    }
}
