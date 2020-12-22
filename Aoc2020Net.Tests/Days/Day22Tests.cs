using Aoc2020Net.Tests.Attributes;
using NUnit.Framework;

namespace Aoc2020Net.Tests.Days
{
    [DayDataPart1(@"
        Player 1:
        9
        2
        6
        3
        1

        Player 2:
        5
        8
        4
        7
        10", 306)]
    [DayDataPart1(30197)]
    [DayDataPart2(@"
        Player 1:
        9
        2
        6
        3
        1

        Player 2:
        5
        8
        4
        7
        10", 291)]
    [DayDataPart2(34031)]
    [TestFixture]
    public sealed class Day22Tests : DayTests<Day22Tests>
    {
    }
}
