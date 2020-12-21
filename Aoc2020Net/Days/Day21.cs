using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Aoc2020Net.Utilities;

namespace Aoc2020Net.Days
{
    internal sealed class Day21 : Day
    {
        private record Food(string[] Ingredients, string[] Allergens);
        private record IngredientsInfo(string[] SafeIngredients, Dictionary<string, string> IngredientsToAllergens);

        public override object SolvePart1()
        {
            var inputData = GetInputFoods();
            var (safeIngredients, _) = GetIngredientsInfo(inputData);
            return inputData.Sum(d => d.Ingredients.Count(i => safeIngredients.Contains(i)));
        }

        public override object SolvePart2()
        {
            var inputData = GetInputFoods();
            var (_, ingredientsToAllergens) = GetIngredientsInfo(inputData);
            return string.Join(",", ingredientsToAllergens.OrderBy(i => i.Value).Select(i => i.Key));
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

        private static IngredientsInfo GetIngredientsInfo(Food[] foods)
        {
            var allergens = foods.SelectMany(f => f.Allergens).Distinct().ToList();
            var ingredients = foods.SelectMany(i => i.Ingredients).Distinct().ToList();

            var ingredientsToAllergens = new Dictionary<string, string>();

            while (allergens.Any())
            {
                var allergensToIngredientsCounts = GetAllergensToIngredientsCounts(foods, allergens, ingredients);

                var allergenAnalyzed = allergensToIngredientsCounts.First(a => a.Value.Values.Count(c => c == a.Value.Values.Max()) == 1);
                allergens.Remove(allergenAnalyzed.Key);

                var ingredientAnalyzed = allergenAnalyzed.Value.OrderByDescending(i => i.Value).First();
                ingredients.Remove(ingredientAnalyzed.Key);

                ingredientsToAllergens[ingredientAnalyzed.Key] = allergenAnalyzed.Key;
            }

            return new IngredientsInfo(ingredients.ToArray(), ingredientsToAllergens);
        }

        private static Dictionary<string, Dictionary<string, int>> GetAllergensToIngredientsCounts(Food[] foods, IEnumerable<string> allergens, IEnumerable<string> ingredients)
        {
            var allergensToIngredientsCounts = new Dictionary<string, Dictionary<string, int>>();

            foreach (var allergen in allergens)
            {
                if (!allergensToIngredientsCounts.TryGetValue(allergen, out var ingredientsCounts))
                    allergensToIngredientsCounts.Add(allergen, ingredientsCounts = new Dictionary<string, int>());

                foreach (var food in foods.Where(f => f.Allergens.Contains(allergen)))
                {
                    foreach (var ingredient in food.Ingredients.Where(i => ingredients.Contains(i)))
                    {
                        ingredientsCounts.TryAdd(ingredient, 0);
                        ingredientsCounts[ingredient]++;
                    }
                }
            }

            return allergensToIngredientsCounts;
        }
    }
}
