using System;
using System.Linq;
using Aoc2020Net.Tests.Attributes;
using NUnit.Framework;

namespace Aoc2020Net.Tests
{
    public abstract class DayTests<T>
    {
        public TestContext TestContext { get; set; }

        [Test]
        [TestCaseSource(nameof(GetDayDataPart1))]
        public void SolvePart1(string input, object expectedSolution, object[] parameters, bool skip) =>
            SolvePart(true, input, expectedSolution, parameters, skip);

        [Test]
        [TestCaseSource(nameof(GetDayDataPart2))]
        public void SolvePart2(string input, object expectedSolution, object[] parameters, bool skip) =>
            SolvePart(false, input, expectedSolution, parameters, skip);

        public static object[] GetDayDataPart1() => GetDayData<DayDataPart1Attribute>();

        public static object[] GetDayDataPart2() => GetDayData<DayDataPart2Attribute>();

        private void SolvePart(bool part1, string input, object expectedSolution, object[] parameters, bool skip)
        {
            if (skip)
                Assert.Inconclusive("Chosen to be skipped.");

            var solution = GetDaySolution(input, parameters, part1);
            Assert.AreEqual(expectedSolution, solution, "Solution is wrong.");
        }

        private static object[] GetDayData<TAttribute>()
            where TAttribute : DayDataAttribute
        {
            return (from a in Attribute.GetCustomAttributes(typeof(T), typeof(TAttribute))
                    let dayDataAttribute = (TAttribute)a
                    select new object[] { dayDataAttribute.Input, dayDataAttribute.Solution, dayDataAttribute.Parameters, dayDataAttribute.Skip }).ToArray();
        }

        private object GetDaySolution(string input, object[] parameters, bool part1)
        {
            var className = GetType().Name;
            var testsWordIndex = className.IndexOf("Tests");
            var dayClassName = className.Substring(0, testsWordIndex);
            var dayType = typeof(Day).Assembly.GetTypes().FirstOrDefault(t => t.Name == dayClassName);
            var day = (Day)Activator.CreateInstance(dayType);

            if (!string.IsNullOrWhiteSpace(input))
                day.Input = input;

            if (parameters != null)
                day.Parameters = parameters;

            return part1 ? day.SolvePart1() : day.SolvePart2();
        }
    }
}
