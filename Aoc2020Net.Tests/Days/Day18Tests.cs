using Aoc2020Net.Tests.Attributes;
using NUnit.Framework;

namespace Aoc2020Net.Tests.Days
{
    [DayDataPart1("1 + 2 * 3 + 4 * 5 + 6", 71)]
    [DayDataPart1("1 + (2 * 3) + (4 * (5 + 6))", 51)]
    [DayDataPart1("2 * 3 + (4 * 5)", 26)]
    [DayDataPart1("5 + (8 * 3 + 9 + 3 * 4 * 3)", 437)]
    [DayDataPart1("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 12240)]
    [DayDataPart1("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 13632)]
    [DayDataPart1(13976444272545)]
    [DayDataPart2("1 + 2 * 3 + 4 * 5 + 6", 231)]
    [DayDataPart2("1 + (2 * 3) + (4 * (5 + 6))", 51)]
    [DayDataPart2("2 * 3 + (4 * 5)", 46)]
    [DayDataPart2("5 + (8 * 3 + 9 + 3 * 4 * 3)", 1445)]
    [DayDataPart2("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 669060)]
    [DayDataPart2("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 23340)]
    [DayDataPart2(88500956630893)]
    [TestFixture]
    public sealed class Day18Tests : DayTests<Day18Tests>
    {
    }
}
