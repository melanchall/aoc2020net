using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Aoc2020Net.Utilities;

namespace Aoc2020Net.Days
{
    internal sealed class Day21 : Day
    {
        private record Food(string[] Ingridients, string[] Allergens);

        public override object SolvePart1()
        {
            var inputData = GetInputFoods();
            var (safeIngridients, _) = GetIngridientsInfo(inputData);
            return inputData.Sum(d => d.Ingridients.Count(i => safeIngridients.Contains(i)));
        }

        public override object SolvePart2()
        {
            var inputData = GetInputFoods();
            var (_, ingridientsToAllergens) = GetIngridientsInfo(inputData);
            return string.Join(",", ingridientsToAllergens.OrderBy(i => i.Value).Select(i => i.Key));
        }

        private Food[] GetInputFoods()
        {
            var regex = new Regex(@"(?:(\w+)\s)+\(contains (?:(\w+)(?:,\s)?)+\)");
            return InputData.GetInputLines()
                .Select(line =>
                {
                    var match = regex.Match(line);
                    return new Food(match.GetGroupStringCaptures(1).ToArray(), match.GetGroupStringCaptures(2).ToArray());
                })
                .ToArray();
        }

        private static (string[] SafeIngridients, Dictionary<string, string> IngridientsToAllergens) GetIngridientsInfo(Food[] foods)
        {
            var allergens = foods.SelectMany(f => f.Allergens).Distinct().ToList();
            var ingridients = foods.SelectMany(i => i.Ingridients).Distinct().ToList();

            var ingridientsToAllergens = new Dictionary<string, string>();

            while (allergens.Any())
            {
                var allergensToIngridientsCounts = new Dictionary<string, Dictionary<string, int>>();

                foreach (var allergen in allergens)
                {
                    if (!allergensToIngridientsCounts.TryGetValue(allergen, out var ingridientsCounts))
                        allergensToIngridientsCounts.Add(allergen, ingridientsCounts = new Dictionary<string, int>());

                    foreach (var food in foods.Where(f => f.Allergens.Contains(allergen)))
                    {
                        foreach (var ingridient in food.Ingridients.Where(i => ingridients.Contains(i)))
                        {
                            if (!ingridientsCounts.ContainsKey(ingridient))
                                ingridientsCounts.Add(ingridient, 0);

                            ingridientsCounts[ingridient]++;
                        }
                    }
                }

                var allergenAnalyzed = allergensToIngridientsCounts.First(a => a.Value.Values.Count(c => c == a.Value.Values.Max()) == 1);
                var ingridientAnalyzed = allergenAnalyzed.Value.OrderByDescending(i => i.Value).First();

                allergens.Remove(allergenAnalyzed.Key);
                ingridients.Remove(ingridientAnalyzed.Key);

                ingridientsToAllergens[ingridientAnalyzed.Key] = allergenAnalyzed.Key;
            }

            return (ingridients.ToArray(), ingridientsToAllergens);
        }
    }
}
