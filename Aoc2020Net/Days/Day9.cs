using System.Collections.Generic;
using System.Linq;

namespace Aoc2020Net.Days
{
    internal sealed class Day9 : Day
    {
        public Day9()
        {
            Parameters = new object[] { 25 };
        }

        private int PreambleSize => (int)Parameters.First();

        public override object SolvePart1() => FindInvalidNumber();

        public override object SolvePart2()
        {
            var invalidNumber = FindInvalidNumber();
            var numbers = InputData.GetInputInt64NumbersFromLines();

            for (var i = 0; i < numbers.Length; i++)
            {
                var accumulated = 0L;
                var set = new List<long>();

                for (var j = i; j < numbers.Length && accumulated < invalidNumber; j++)
                {
                    set.Add(numbers[j]);
                    accumulated += numbers[j];
                }

                if (accumulated == invalidNumber)
                    return set.Min() + set.Max();
            }

            return null;
        }

        private long FindInvalidNumber()
        {
            var numbers = InputData.GetInputInt64NumbersFromLines();

            for (var n = PreambleSize; n < numbers.Length; n++)
            {
                var num = numbers[n];
                var matchPreamble = false;

                for (var i = n - PreambleSize; i < n + PreambleSize; i++)
                {
                    for (var j = i + 1; j < n + PreambleSize; j++)
                    {
                        matchPreamble |= num == (numbers[i] + numbers[j]);
                    }
                }

                if (!matchPreamble)
                    return num;
            }

            return -1;
        }
    }
}
