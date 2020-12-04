using Aoc2020Net.Tests.Attributes;
using NUnit.Framework;

namespace Aoc2020Net.Tests.Days
{
    [DayDataPart1(@"
        1-3 a: abcde
        1-3 b: cdefg
        2-9 c: ccccccccc", 2)]
    [DayDataPart1(456)]
    [DayDataPart2(@"
        1-3 a: abcde
        1-3 b: cdefg
        2-9 c: ccccccccc", 1)]
    [DayDataPart2(308)]
    [TestFixture]
    public sealed class Day2Tests : DayTests<Day2Tests>
    {
    }
}
