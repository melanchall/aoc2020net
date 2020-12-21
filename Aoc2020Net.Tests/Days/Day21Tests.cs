using Aoc2020Net.Tests.Attributes;
using NUnit.Framework;

namespace Aoc2020Net.Tests.Days
{
    [DayDataPart1(@"
        mxmxvkd kfcds sqjhc nhms (contains dairy, fish)
        trh fvjkl sbzzf mxmxvkd (contains dairy)
        sqjhc fvjkl (contains soy)
        sqjhc mxmxvkd sbzzf (contains fish)", 5)]
    [DayDataPart1(2410)]
    [DayDataPart2(@"
        mxmxvkd kfcds sqjhc nhms (contains dairy, fish)
        trh fvjkl sbzzf mxmxvkd (contains dairy)
        sqjhc fvjkl (contains soy)
        sqjhc mxmxvkd sbzzf (contains fish)", "mxmxvkd,sqjhc,fvjkl")]
    [DayDataPart2("tmp,pdpgm,cdslv,zrvtg,ttkn,mkpmkx,vxzpfp,flnhl")]
    [TestFixture]
    public sealed class Day21Tests : DayTests<Day21Tests>
    {
    }
}
