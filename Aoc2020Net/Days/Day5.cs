using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2020Net.Days
{
    internal sealed class Day5 : Day
    {
        private const int MaxRow = 127;
        private const int MaxColumn = 7;

        public override object SolvePart1() => GetSeatsIds().Max();

        public override object SolvePart2()
        {
            var seatsIds = GetSeatsIds().ToArray();
            return Enumerable
                .Range(1, GetSeatId(MaxRow, MaxColumn))
                .FirstOrDefault(id => !seatsIds.Contains(id) &&
                                       seatsIds.Contains(id + 1) &&
                                       seatsIds.Contains(id - 1));
        }

        private IEnumerable<int> GetSeatsIds() => InputData
            .GetInputLines()
            .Select(line => GetSeatId(GetSeatIdPart(line.Take(7), 'F', MaxRow),
                                      GetSeatIdPart(line.Skip(7), 'L', MaxColumn)));

        private int GetSeatId(int row, int column) => row * 8 + column;

        private static int GetSeatIdPart(IEnumerable<char> directions, char lowerHalfSymbol, int rangeMax)
        {
            var rangeMin = 0;

            foreach (var direction in directions)
            {
                if (direction == lowerHalfSymbol)
                    rangeMax = rangeMin + (rangeMax - rangeMin) / 2;
                else
                    rangeMin = (rangeMax + rangeMin) / 2 + 1;
            }

            return rangeMin;
        }
    }
}
