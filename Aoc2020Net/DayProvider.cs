using System;
using System.Linq;

namespace Aoc2020Net
{
    public static class DayProvider
    {
        private static readonly Day[] Days = typeof(Day)
            .Assembly
            .GetTypes()
            .Where(t => !t.IsAbstract && typeof(Day).IsAssignableFrom(t))
            .Select(t => Activator.CreateInstance(t))
            .Cast<Day>()
            .ToArray();

        public static Day GetDay(int dayNumber) =>
            Days.FirstOrDefault(d => d.DayNumber == dayNumber)
            ?? throw new InvalidOperationException($"There is no day #{dayNumber}");
    }
}
