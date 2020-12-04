using System;
using Aoc2020Net;

void WriteHello()
{
    Output.WriteInfo("Welcome to Advent of Code 2020! Please ");
    Output.WriteImportant("write the number of a day");
    Output.WriteInfo(" to solve: ");
}

void WriteBye()
{
    Output.WriteInfo("Press any key to exit...");
    Console.ReadKey();
    Output.WriteNewLine();
}

Day GetDay()
{
    var dayNumberString = Console.ReadLine();
    if (!int.TryParse(dayNumberString, out var dayNumber))
    {
        Output.WriteError("Failed to parse a day number.");
        return null;
    }

    try
    {
        return DayProvider.GetDay(dayNumber);
    }
    catch (Exception ex)
    {
        Output.WriteError($"Failed to get day: {ex.Message}");
        Output.WriteNewLine();
        return null;
    }
}

void WriteSolution(Day day, Func<object> solve, int partNumber)
{
    Output.WriteImportant($"Part #{partNumber} solution: ");

    try
    {
        var solution = solve();
        Output.WriteInfo(solution?.ToString());
        Output.WriteNewLine();
    }
    catch (Exception ex)
    {
        Output.WriteError($"Failed to get solution: {ex.Message}");
        Output.WriteNewLine();
    }
}

void WriteSolutions()
{
    var day = GetDay();
    if (day != null)
    {
        WriteSolution(day, day.SolvePart1, 1);
        WriteSolution(day, day.SolvePart2, 2);
    }
}

WriteHello();
WriteSolutions();
WriteBye();