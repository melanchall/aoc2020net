using Aoc2020Net.Tests.Attributes;
using NUnit.Framework;

namespace Aoc2020Net.Tests.Days
{
    [DayDataPart1(@"
        F10
        N3
        F7
        R90
        F11", 25)]
    [DayDataPart1(521)]
    [DayDataPart2(@"
        F10
        N3
        F7
        R90
        F11", 286)]
    [DayDataPart2(22848)]
    [TestFixture]
    public sealed class Day12Tests : DayTests<Day12Tests>
    {
    }
}
