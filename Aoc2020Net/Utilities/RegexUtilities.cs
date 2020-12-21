using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2020Net.Utilities
{
    internal static class RegexUtilities
    {
        public static int GetInt32Group(this Match match, int groupIndex, int captureIndex = 0) => int.Parse(match.Groups[groupIndex].Captures[captureIndex].Value);

        public static int GetInt32Group(this Match match, string groupName, int captureIndex = 0) => int.Parse(match.Groups[groupName].Captures[captureIndex].Value);

        public static long GetInt64Group(this Match match, int groupIndex, int captureIndex = 0) => long.Parse(match.Groups[groupIndex].Captures[captureIndex].Value);

        public static long GetInt64Group(this Match match, string groupName, int captureIndex = 0) => long.Parse(match.Groups[groupName].Captures[captureIndex].Value);

        public static string GetStringGroup(this Match match, int groupIndex, int captureIndex = 0) => match.Groups[groupIndex].Captures[captureIndex].Value;

        public static string GetStringGroup(this Match match, string groupName, int captureIndex = 0) => match.Groups[groupName].Captures[captureIndex].Value;

        public static char GetCharGroup(this Match match, int groupIndex, int captureIndex = 0) => match.GetStringGroup(groupIndex, captureIndex).First();

        public static char GetCharGroup(this Match match, string groupName, int captureIndex = 0) => match.GetStringGroup(groupName, captureIndex).First();

        public static bool IsGroupCaptured(this Match match, int groupIndex) => match.Groups.Count > groupIndex && match.Groups[groupIndex].Success;

        public static bool IsGroupCaptured(this Match match, string groupName) => match.Groups.TryGetValue(groupName, out var group) && group.Success;

        public static int GetGroupCapturesCount(this Match match, int groupIndex) => match.Groups[groupIndex].Captures.Count;

        public static int GetGroupCapturesCount(this Match match, string groupName) => match.Groups[groupName].Captures.Count;

        public static IEnumerable<string> GetGroupStringCaptures(this Match match, int groupIndex) => match.Groups[groupIndex].Captures.Select(c => c.Value);
    }
}
