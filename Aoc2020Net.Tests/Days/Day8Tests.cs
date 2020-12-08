using Aoc2020Net.Tests.Attributes;
using NUnit.Framework;

namespace Aoc2020Net.Tests.Days
{
    [DayDataPart1(@"
        nop +0
        acc +1
        jmp +4
        acc +3
        jmp -3
        acc -99
        acc +1
        jmp -4
        acc +6", 5)]
    [DayDataPart1(1816)]
    [DayDataPart2(@"
        nop +0
        acc +1
        jmp +4
        acc +3
        jmp -3
        acc -99
        acc +1
        jmp -4
        acc +6", 8)]
    [DayDataPart2(1149)]
    [TestFixture]
    public sealed class Day8Tests : DayTests<Day8Tests>
    {
    }
}
