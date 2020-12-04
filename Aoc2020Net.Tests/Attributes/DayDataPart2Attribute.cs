namespace Aoc2020Net.Tests.Attributes
{
    public sealed class DayDataPart2Attribute : DayDataAttribute
    {
        public DayDataPart2Attribute(string input, object solution)
            : base(input, solution)
        {
        }

        public DayDataPart2Attribute(object myInputSolution)
            : this(null, myInputSolution)
        {
        }
    }
}
