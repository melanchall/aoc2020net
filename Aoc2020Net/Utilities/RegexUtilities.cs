using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2020Net.Utilities
{
    internal static class RegexUtilities
    {
        public static int GetInt32Group(this Match match, int groupIndex) => int.Parse(match.Groups[groupIndex].Value);

        public static int GetInt32Group(this Match match, string groupName) => int.Parse(match.Groups[groupName].Value);

        public static string GetStringGroup(this Match match, int groupIndex) => match.Groups[groupIndex].Value;

        public static string GetStringGroup(this Match match, string groupName) => match.Groups[groupName].Value;

        public static char GetCharGroup(this Match match, int groupIndex) => match.GetStringGroup(groupIndex).First();

        public static char GetCharGroup(this Match match, string groupName) => match.GetStringGroup(groupName).First();
    }
}
