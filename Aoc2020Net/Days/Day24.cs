using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Aoc2020Net.Utilities;

namespace Aoc2020Net.Days
{
    internal sealed class Day24 : Day
    {
        private enum TileColor
        {
            Black,
            White
        }

        private record Step(double XStep, double YStep);
        private record Position(double X, double Y);

        private static readonly Dictionary<string, Step> DirectionsSteps = new Dictionary<string, Step>
        {
            ["e"]  = new Step(1, 0),
            ["se"] = new Step(0.5, -0.5),
            ["sw"] = new Step(-0.5, -0.5),
            ["w"]  = new Step(-1, 0),
            ["nw"] = new Step(-0.5, 0.5),
            ["ne"] = new Step(0.5, 0.5)
        };

        public override object SolvePart1() => GetInitialTilesColors().Values.CountValue(TileColor.Black);

        public override object SolvePart2()
        {
            var tilesColors = GetInitialTilesColors();

            for (var i = 0; i < 100; i++)
            {
                var newTilesColors = GetCoreAndShellPositions(tilesColors.Keys).ToDictionary(
                    p => p,
                    p => tilesColors.TryGetValue(p, out var color) ? color : TileColor.White);

                foreach (var position in newTilesColors.Keys)
                {
                    var blackNeigborsCount = GetNeighborColors(position, tilesColors).CountValue(TileColor.Black);
                    var currentColor = newTilesColors[position];

                    newTilesColors[position] = currentColor switch
                    {
                        TileColor.Black => blackNeigborsCount switch
                        {
                            0 or > 2 => TileColor.White,
                            _        => TileColor.Black
                        },
                        TileColor.White => blackNeigborsCount switch
                        {
                            2 => TileColor.Black,
                            _ => TileColor.White
                        },
                        _ => currentColor
                    };
                }

                tilesColors = newTilesColors;
            }

            return tilesColors.Values.CountValue(TileColor.Black);
        }

        private static IEnumerable<TileColor> GetNeighborColors(Position position, Dictionary<Position, TileColor> tilesColors) =>
            GetNeighborPositions(position).Select(p => tilesColors.TryGetValue(p, out var color) ? color : TileColor.White);

        private static IEnumerable<Position> GetCoreAndShellPositions(IEnumerable<Position> corePositions) =>
            corePositions.SelectMany(GetNeighborPositions).Distinct();

        private static IEnumerable<Position> GetNeighborPositions(Position position) =>
            DirectionsSteps.Values.Select(s => new Position(position.X + s.XStep, position.Y + s.YStep));

        private Dictionary<Position, TileColor> GetInitialTilesColors()
        {
            var regex = new Regex(string.Join("|", DirectionsSteps.Keys));
            var steps = from line in InputData.GetInputLines()
                        let matches = regex.Matches(line)
                        select matches.Select(m => DirectionsSteps[m.Value]).ToArray();

            var tilesColors = new Dictionary<Position, TileColor>();

            foreach (var tileSteps in steps)
            {
                var position = tileSteps.Aggregate(new Position(0, 0), (result, step) => new Position(result.X + step.XStep, result.Y + step.YStep));

                if (!tilesColors.TryGetValue(position, out var color))
                    tilesColors.Add(position, color = TileColor.White);

                tilesColors[position] = color == TileColor.White ? TileColor.Black : TileColor.White;
            }

            return tilesColors;
        }
    }
}
