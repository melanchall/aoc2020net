using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Aoc2020Net.Utilities;

namespace Aoc2020Net.Days
{
    internal sealed class Day7 : Day
    {
        private const string ShinyGoldBagColor = "shiny gold";

        private record InnerBagsRule(string Color, int Quantity);
        private record BagRule(string Color, InnerBagsRule[] InnerBagsRules);

        public override object SolvePart1()
        {
            var rules = GetInputRules().ToArray();
            return rules.Count(r => r.Color != ShinyGoldBagColor && ContainsShinyGoldBag(r.Color, rules));
        }

        public override object SolvePart2() => CountInnerBags(ShinyGoldBagColor, GetInputRules().ToArray());

        private bool ContainsShinyGoldBag(string bagColor, IEnumerable<BagRule> rules) =>
            bagColor == ShinyGoldBagColor ||
            rules.First(r => r.Color == bagColor).InnerBagsRules.Any(r => ContainsShinyGoldBag(r.Color, rules));

        private int CountInnerBags(string bagColor, IEnumerable<BagRule> rules)
        {
            var innerBagsRules = rules.First(r => r.Color == bagColor).InnerBagsRules;
            return !innerBagsRules.Any()
                ? 0
                : innerBagsRules.Sum(b => b.Quantity * (1 + CountInnerBags(b.Color, rules)));
        }

        private IEnumerable<BagRule> GetInputRules()
        {
            const string noBagsGroupName = "empty";
            const string bagColorGroupName = "color";
            const string innerBagColorGroupName = "icolor";
            const string innerBagsCountGroupName = "icount";

            var regex = new Regex($@"(?<{bagColorGroupName }>.+) bags contain (((?<{innerBagsCountGroupName}>\d+) (?<{innerBagColorGroupName}>.+?) bags?(, )?)+|(?<{noBagsGroupName}>no other bags))");

            return from line in InputData.GetInputLines()
                   let match = regex.Match(line)
                   select new BagRule(
                       match.GetStringGroup(bagColorGroupName),
                       match.IsGroupCaptured(noBagsGroupName)
                        ? Array.Empty<InnerBagsRule>()
                        : Enumerable.Range(0, match.GetGroupCapturesCount(innerBagColorGroupName))
                                    .Select(i => new InnerBagsRule(match.GetStringGroup(innerBagColorGroupName, i), match.GetInt32Group(innerBagsCountGroupName, i)))
                                    .ToArray());
        }
    }
}
