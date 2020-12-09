using Aoc2020Net.Tests.Attributes;
using NUnit.Framework;

namespace Aoc2020Net.Tests.Days
{
    [DayDataPart1(@"
        35
        20
        15
        25
        47
        40
        62
        55
        65
        95
        102
        117
        150
        182
        127
        219
        299
        277
        309
        576", 127, Parameters = new object[] { 5 })]
    [DayDataPart1(675280050)]
    [DayDataPart2(@"
        35
        20
        15
        25
        47
        40
        62
        55
        65
        95
        102
        117
        150
        182
        127
        219
        299
        277
        309
        576", 62, Parameters = new object[] { 5 })]
    [DayDataPart2(96081673)]
    [TestFixture]
    public sealed class Day9Tests : DayTests<Day9Tests>
    {
    }
}
