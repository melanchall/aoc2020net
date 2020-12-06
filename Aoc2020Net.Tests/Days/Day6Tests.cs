using Aoc2020Net.Tests.Attributes;
using NUnit.Framework;

namespace Aoc2020Net.Tests.Days
{
    [DayDataPart1(@"
        abcx
        abcy
        abcz", 6)]
    [DayDataPart1(@"
        abc

        a
        b
        c

        ab
        ac

        a
        a
        a
        a

        b", 11)]
    [DayDataPart1(6587)]
    [DayDataPart2(@"
        abc

        a
        b
        c

        ab
        ac

        a
        a
        a
        a

        b", 6)]
    [DayDataPart2(3235)]
    [TestFixture]
    public sealed class Day6Tests : DayTests<Day6Tests>
    {
    }
}
