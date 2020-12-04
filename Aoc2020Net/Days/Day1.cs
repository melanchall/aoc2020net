namespace Aoc2020Net.Days
{
    internal sealed class Day1 : Day
    {
        public override object SolvePart1()
        {
            var numbers = InputData.GetInputInt32NumbersFromLines();

            for (var i = 0; i < numbers.Length - 1; i++)
            {
                for (var j = i + 1; j < numbers.Length; j++)
                {
                    if (numbers[i] + numbers[j] == 2020)
                    {
                        return numbers[i] * numbers[j];
                    }
                }
            }

            return null;
        }

        public override object SolvePart2()
        {
            var numbers = InputData.GetInputInt32NumbersFromLines();

            for (var i = 0; i < numbers.Length - 2; i++)
            {
                for (var j = i + 1; j < numbers.Length - 1; j++)
                {
                    for (var k = j + 1; k < numbers.Length; k++)
                    {
                        if (numbers[i] + numbers[j] + numbers[k] == 2020)
                        {
                            return numbers[i] * numbers[j] * numbers[k];
                        }
                    }
                }
            }

            return null;
        }
    }
}
