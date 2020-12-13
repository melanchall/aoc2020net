using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2020Net.Days
{
    internal sealed class Day13 : Day
    {
        private record Schedule(int StartTime, IEnumerable<int?> BusesIds);

        public override object SolvePart1()
        {
            var (startTime, busesIds) = GetInputSchedule();

            var nearestBus = busesIds
                .Where(id => id != null)
                .Select(id => new
                {
                    BusId = id,
                    DepartureOffset = id * ((startTime / id) + 1) - startTime
                })
                .OrderBy(t => t.DepartureOffset)
                .First();

            return nearestBus.BusId * nearestBus.DepartureOffset;
        }

        public override object SolvePart2()
        {
            var (_, busesIds) = GetInputSchedule();

            var cms = busesIds
                .Select((id, i) => new { Id = id, Offset = i })
                .Where(id => id.Id != null)
                .Select(x => new
                {
                    C = x.Id - x.Offset,
                    M = x.Id.Value
                })
                .ToArray();
            var ams = cms
                .Select(cm => new
                {
                    A = cms.Where(xxx => xxx.M != cm.M).Aggregate(1L, (a, b) => a * b.M),
                    M = cm.M
                })
                .ToArray();
            var xs = ams
                .Select(am =>
                {
                    GetGcdExtended(am.A, am.M, out var x, out _);
                    return Enumerable.Range(0, int.MaxValue).Select(i => x + am.M * i).First(p => p > 0);
                })
                .ToArray();

            return Enumerable.Range(0, cms.Length)
                .Select(i => ams[i].A * xs[i] * cms[i].C)
                .Sum() % cms.Aggregate(1L, (m, cm) => m * cm.M);
        }

        private Schedule GetInputSchedule()
        {
            var lines = InputData.GetInputLines();
            var startTime = int.Parse(lines.First());
            return new Schedule(
                startTime,
                lines
                    .Last()
                    .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.TryParse(s, out var id) ? (int?)id : null));
        }

        private void GetGcdExtended(long a, long b, out long x, out long y)
        {
            if (b == 0)
            {
                x = 1;
                y = 0;
                return;
            }

            GetGcdExtended(b, a % b, out x, out y);

            var yTmp = y;
            y = x - (a / b) * y;
            x = yTmp;
        }
    }
}
