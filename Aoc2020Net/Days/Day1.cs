using System.Linq;
using Aoc2020Net.Utilities;

namespace Aoc2020Net.Days
{
    internal sealed class Day1 : Day
    {
        public override object SolvePart1()
        {
            var numbers = InputData.GetInputInt32NumbersFromLines();
            var indices = DataProvider.GetIndicesPairs(0, numbers.Length - 1).First(index => numbers[index.I] + numbers[index.J] == 2020);
            return numbers[indices.I] * numbers[indices.J];
        }

        public override object SolvePart2()
        {
            var numbers = InputData.GetInputInt32NumbersFromLines();

            for (var i = 0; i < numbers.Length - 2; i++)
            {
                foreach (var indices in DataProvider.GetIndicesPairs(i + 1, numbers.Length - 1))
                {
                    if (numbers[i] + numbers[indices.I] + numbers[indices.J] == 2020)
                    {
                        return numbers[i] * numbers[indices.I] * numbers[indices.J];
                    }
                }
            }

            return null;
        }
    }
}
