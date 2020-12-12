using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Aoc2020Net.Utilities;

namespace Aoc2020Net.Days
{
    internal sealed class Day12 : Day
    {
        private record Vector(int X, int Y);
        private record Instruction(char Command, int Value);

        public override object SolvePart1()
        {
            var instructions = GetInputInstructions();

            var direction = new Vector(1, 0);
            var point = (X: 0, Y: 0);

            foreach (var instruction in instructions)
            {
                point = instruction.Command switch
                {
                    'N' => (point.X, point.Y + instruction.Value),
                    'S' => (point.X, point.Y - instruction.Value),
                    'W' => (point.X - instruction.Value, point.Y),
                    'E' => (point.X + instruction.Value, point.Y),
                    'F' => (point.X + direction.X * instruction.Value, point.Y + direction.Y * instruction.Value),
                    _ => point
                };

                direction = instruction.Command switch
                {
                    'R' => RotateVector(direction, instruction.Value),
                    'L' => RotateVector(direction, -instruction.Value),
                    _ => direction
                };
            }

            return GetManhattanDistance(point);
        }

        public override object SolvePart2()
        {
            var instructions = GetInputInstructions();

            var direction = new Vector(10, 1);
            var point = (X: 0, Y: 0);

            foreach (var instruction in instructions)
            {
                point = instruction.Command switch
                {
                    'F' => (point.X + direction.X * instruction.Value, point.Y + direction.Y * instruction.Value),
                    _ => point
                };

                direction = instruction.Command switch
                {
                    'N' => new Vector(direction.X, direction.Y + instruction.Value),
                    'S' => new Vector(direction.X, direction.Y - instruction.Value),
                    'W' => new Vector(direction.X - instruction.Value, direction.Y),
                    'E' => new Vector(direction.X + instruction.Value, direction.Y),
                    'R' => RotateVector(direction, instruction.Value),
                    'L' => RotateVector(direction, -instruction.Value),
                    _ => direction
                };
            }

            return GetManhattanDistance(point);
        }

        private static int GetManhattanDistance((int X, int Y) point) => Math.Abs(point.X) + Math.Abs(point.Y);

        private IEnumerable<Instruction> GetInputInstructions() =>
            from line in InputData.GetInputLines()
            let match = Regex.Match(line, @"(\w)(\d+)")
            select new Instruction(match.GetCharGroup(1), match.GetInt32Group(2));

        private static Vector RotateVector(Vector v, int degrees)
        {
            var radians = DegreesToRadians(degrees);
            return new Vector(
                (int)Math.Round(v.X * Math.Cos(radians) + v.Y * Math.Sin(radians)),
                (int)Math.Round(v.Y * Math.Cos(radians) - v.X * Math.Sin(radians)));
        }

        private static double DegreesToRadians(int degrees) => degrees * Math.PI / 180;
    }
}
