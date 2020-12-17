using Aoc2020Net.Tests.Attributes;
using NUnit.Framework;

namespace Aoc2020Net.Tests.Days
{
    [DayDataPart1(@"
        .#.
        ..#
        ###", 112)]
    [DayDataPart1(286)]
    [DayDataPart2(@"
        .#.
        ..#
        ###", 848)]
    [DayDataPart2(960)]
    [TestFixture]
    public sealed class Day17Tests : DayTests<Day17Tests>
    {
    }
}
