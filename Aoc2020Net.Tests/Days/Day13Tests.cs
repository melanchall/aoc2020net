using Aoc2020Net.Tests.Attributes;
using NUnit.Framework;

namespace Aoc2020Net.Tests.Days
{
    [DayDataPart1(@"
        939
        7,13,x,x,59,x,31,19", 295)]
    [DayDataPart1(3606)]
    [DayDataPart2(@"
        0
        7,13,x,x,59,x,31,19", 1068781)]
    [DayDataPart2(@"
        0
        17,x,13,19", 3417)]
    [DayDataPart2(@"
        0
        67,7,59,61", 754018)]
    [DayDataPart2(@"
        0
        67,x,7,59,61", 779210)]
    [DayDataPart2(@"
        0
        67,7,x,59,61", 1261476)]
    [DayDataPart2(@"
        0
        1789,37,47,1889", 1202161486)]
    [DayDataPart2(379786358533423)]
    [TestFixture]
    public sealed class Day13Tests : DayTests<Day13Tests>
    {
    }
}
