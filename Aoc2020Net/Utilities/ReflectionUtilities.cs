using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Aoc2020Net.Utilities
{
    internal static class ReflectionUtilities
    {
        public static IEnumerable<(TEnum Value, TAttribute Attribute)> GetAttributedEnumValues<TEnum, TAttribute>()
            where TEnum : Enum
            where TAttribute: Attribute =>
            typeof(TEnum)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(f => ((TEnum)f.GetValue(null), f.GetCustomAttribute<TAttribute>()));
    }
}
