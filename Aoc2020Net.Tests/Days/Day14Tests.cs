using Aoc2020Net.Tests.Attributes;
using NUnit.Framework;

namespace Aoc2020Net.Tests.Days
{
    [DayDataPart1(@"
        mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
        mem[8] = 11
        mem[7] = 101
        mem[8] = 0", 165)]
    [DayDataPart1(7997531787333)]
    [DayDataPart2(@"
        mask = 000000000000000000000000000000X1001X
        mem[42] = 100
        mask = 00000000000000000000000000000000X0XX
        mem[26] = 1", 208)]
    [DayDataPart2(3564822193820)]
    [TestFixture]
    public sealed class Day14Tests : DayTests<Day14Tests>
    {
    }
}
