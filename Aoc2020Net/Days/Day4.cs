using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Aoc2020Net.Utilities;

namespace Aoc2020Net.Days
{
    internal sealed class Day4 : Day
    {
        [AttributeUsage(AttributeTargets.Parameter)]
        private sealed class FieldNameAttribute : Attribute
        {
            public FieldNameAttribute(string fieldName) => FieldName = fieldName;

            public string FieldName { get; }
        }

        private static class FieldsNames
        {
            public const string BirthYear = "byr";
            public const string IssueYear = "iyr";
            public const string ExpirationYear = "eyr";
            public const string Height = "hgt";
            public const string HairColor = "hcl";
            public const string EyeColor = "ecl";
            public const string PassportId = "pid";
        }

        private record Passport(
            [FieldName(FieldsNames.BirthYear)] string BirthYear,
            [FieldName(FieldsNames.IssueYear)] string IssueYear,
            [FieldName(FieldsNames.ExpirationYear)] string ExpirationYear,
            [FieldName(FieldsNames.Height)] string Height,
            [FieldName(FieldsNames.HairColor)] string HairColor,
            [FieldName(FieldsNames.EyeColor)] string EyeColor,
            [FieldName(FieldsNames.PassportId)] string PassportId);

        public override object SolvePart1() => GetInputPassports().Count(AreAllFieldsFilled);

        public override object SolvePart2() => GetInputPassports().Count(AreAllFieldsFilledCorrectly);

        private bool AreAllFieldsFilled(Passport passport) => typeof(Passport)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .All(p => !string.IsNullOrWhiteSpace(p.GetValue(passport)?.ToString()));

        private bool AreAllFieldsFilledCorrectly(Passport passport) =>
            AreAllFieldsFilled(passport) &&
            IsBirthYearValid(passport.BirthYear) &&
            IsIssueYearValid(passport.IssueYear) &&
            IsExpirationYearValid(passport.ExpirationYear) &&
            IsHeightValid(passport.Height) &&
            IsHairColorValid(passport.HairColor) &&
            IsEyeColorValid(passport.EyeColor) &&
            IsPassportIdValid(passport.PassportId);

        private static bool IsBirthYearValid(string birthYear) => IsYearValid(birthYear, 1920, 2002);

        private static bool IsIssueYearValid(string issueYear) => IsYearValid(issueYear, 2010, 2020);

        private static bool IsExpirationYearValid(string expirationYear) => IsYearValid(expirationYear, 2020, 2030);

        private static bool IsHeightValid(string height)
        {
            const string centimeterUnit = "cm";
            const string inchUnit = "in";

            var match = Regex.Match(height, $@"(\d+)({centimeterUnit}|{inchUnit})");
            if (!match.Success)
                return false;

            return match.Groups[2].Value switch
            {
                centimeterUnit => match.GetInt32Group(1) >= 150 && match.GetInt32Group(1) <= 193,
                inchUnit       => match.GetInt32Group(1) >= 59 && match.GetInt32Group(1) <= 76,
                _              => false
            };
        }

        private static bool IsHairColorValid(string hairColor) => Regex.IsMatch(hairColor, @"^#\w{6}$");

        private static bool IsEyeColorValid(string eyeColor) => Regex.IsMatch(eyeColor, "^(amb|blu|brn|gry|grn|hzl|oth)$");

        private static bool IsPassportIdValid(string passportId) => Regex.IsMatch(passportId, @"^\d{9}$");

        private static bool IsYearValid(string year, int minYear, int maxYear) =>
            int.TryParse(year, out var value) && value >= minYear && value <= maxYear;

        private IEnumerable<Passport> GetInputPassports()
        {
            var lines =
                string.Join(" ", InputData.GetInputLines().Select(l => string.IsNullOrWhiteSpace(l) ? "$$" : l))
                .Split("$$", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            var requiredFieldsNames = typeof(Passport).GetConstructors().First()
                .GetParameters()
                .Select(p => p.GetCustomAttribute<FieldNameAttribute>().FieldName)
                .ToArray();

            const string fieldNameGroupName = "f";
            const string valueGroupName = "v";
            var regex = new Regex($@"(?<{fieldNameGroupName }>{string.Join("|", requiredFieldsNames)}):(?<{valueGroupName}>.+?)(\s+|$)");

            foreach (var line in lines)
            {
                var fieldsValues = regex.Matches(line).ToDictionary(
                    m => m.GetStringGroup(fieldNameGroupName)?.Trim(),
                    m => m.GetStringGroup(valueGroupName)?.Trim());

                yield return new Passport(
                    fieldsValues.GetValueOrDefault(FieldsNames.BirthYear),
                    fieldsValues.GetValueOrDefault(FieldsNames.IssueYear),
                    fieldsValues.GetValueOrDefault(FieldsNames.ExpirationYear),
                    fieldsValues.GetValueOrDefault(FieldsNames.Height),
                    fieldsValues.GetValueOrDefault(FieldsNames.HairColor),
                    fieldsValues.GetValueOrDefault(FieldsNames.EyeColor),
                    fieldsValues.GetValueOrDefault(FieldsNames.PassportId));
            }
        }
    }
}
