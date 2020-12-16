using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Aoc2020Net.Utilities;

namespace Aoc2020Net.Days
{
    internal sealed class Day16 : Day
    {
        private record FieldData(string FieldName, Range FirstRange, Range SecondRange);
        private record TicketData(int[] Numbers);
        private record TicketsData(FieldData[] FieldsData, TicketData MyTicketData, TicketData[] NearbyTicketsData);

        public override object SolvePart1() => GetInvalidNumbers(GetInputTicketsData()).Sum();

        public override object SolvePart2()
        {
            var ticketsData = GetInputTicketsData();
            var invalidNumbers = GetInvalidNumbers(ticketsData).ToArray();
            var validNearbyTicketsData = ticketsData.NearbyTicketsData.Where(t => !t.Numbers.Intersect(invalidNumbers).Any()).ToArray();
            var myTicketNumbers = ticketsData.MyTicketData.Numbers;

            var possibleFieldsIndices = ticketsData
                .FieldsData
                .Select(f => (
                    FieldName: f.FieldName,
                    PossibleIndices: Enumerable.Range(0, myTicketNumbers.Length)
                                               .Where(i => validNearbyTicketsData.All(t => f.FirstRange.ContainsValue(t.Numbers[i]) ||
                                                                                           f.SecondRange.ContainsValue(t.Numbers[i])))
                                               .ToArray()))
                .OrderBy(i => i.PossibleIndices.Length)
                .ToArray();

            var assignedIndices = new HashSet<int>();
            var fieldsIndices = new Dictionary<string, int>();

            foreach (var indices in possibleFieldsIndices)
            {
                assignedIndices.Add(fieldsIndices[indices.FieldName] = indices.PossibleIndices.Except(assignedIndices).First());
            }

            return fieldsIndices.Where(i => i.Key.StartsWith("departure")).Select(i => myTicketNumbers[i.Value]).Product();
        }

        private static IEnumerable<int> GetInvalidNumbers(TicketsData ticketsData)
        {
            var nearbyTicketsNumbers = ticketsData.NearbyTicketsData.SelectMany(t => t.Numbers);
            var validNumbersRanges = ticketsData.FieldsData.SelectMany(f => new[] { f.FirstRange, f.SecondRange });
            return nearbyTicketsNumbers.Where(n => !validNumbersRanges.Any(r => n >= r.Start.Value && n <= r.End.Value));
        }

        private TicketsData GetInputTicketsData()
        {
            var dataLinesGroups = InputData.GetInputLinesGroups();

            var fieldDataRegex = new Regex(@"(.+): (\d+)-(\d+) or (\d+)-(\d+)");
            var fieldsData = from line in dataLinesGroups[0]
                             let match = fieldDataRegex.Match(line)
                             select new FieldData(
                                 match.GetStringGroup(1),
                                 match.GetInt32Group(2)..match.GetInt32Group(3),
                                 match.GetInt32Group(4)..match.GetInt32Group(5));

            var myTicketNumbers = from n in dataLinesGroups[1][1].Split(',')
                                  select int.Parse(n);
            var myTicketData = new TicketData(myTicketNumbers.ToArray());

            var nearbyTicketsData = from line in dataLinesGroups[2].Skip(1)
                                    let numbers = from n in line.Split(',')
                                                  select int.Parse(n)
                                    select new TicketData(numbers.ToArray());

            return new TicketsData(fieldsData.ToArray(), myTicketData, nearbyTicketsData.ToArray());
        }
    }
}
