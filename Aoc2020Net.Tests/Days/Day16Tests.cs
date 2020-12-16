using Aoc2020Net.Tests.Attributes;
using NUnit.Framework;

namespace Aoc2020Net.Tests.Days
{
    [DayDataPart1(@"
        class: 1-3 or 5-7
        row: 6-11 or 33-44
        seat: 13-40 or 45-50

        your ticket:
        7,1,14

        nearby tickets:
        7,3,47
        40,4,50
        55,2,20
        38,6,12", 71)]
    [DayDataPart1(20091)]
    [DayDataPart2(2325343130651)]
    [TestFixture]
    public sealed class Day16Tests : DayTests<Day16Tests>
    {
    }
}
