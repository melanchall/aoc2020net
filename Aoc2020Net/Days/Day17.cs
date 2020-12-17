using System;
using System.Collections.Generic;
using System.Linq;
using Aoc2020Net.Utilities;

namespace Aoc2020Net.Days
{
    internal sealed class Day17 : Day
    {
        private enum CubeState
        {
            [GridSymbol('#')] Active,
            [GridSymbol('.')] Inactive
        }

        private record Position;
        private record Position3(int X, int Y, int Z) : Position;
        private record Position4(int X, int Y, int Z, int W) : Position;

        public override object SolvePart1() => GetActiveCubesCountAfterBootProcess((x, y) => new Position3(x, y, 0));

        public override object SolvePart2() => GetActiveCubesCountAfterBootProcess((x, y) => new Position4(x, y, 0, 0));

        private int GetActiveCubesCountAfterBootProcess(Func<int, int, Position> createInitialPosition)
        {
            var (grid, width, height) = InputData.GetInputGrid<CubeState>();

            var cubes = DataProvider.GetGridCoordinates(width, height).ToDictionary(
                c => createInitialPosition(c.X, c.Y),
                c => grid[c.X, c.Y]);

            for (var i = 0; i < 6; i++)
            {
                var newCubes = new Dictionary<Position, CubeState>();

                foreach (var position in GetCoreAndShellPositions(cubes.Keys))
                {
                    var activeNeighborCubesCount = GetNeighborPositions(position)
                        .Select(p => GetCubeState(p, cubes))
                        .CountValue(CubeState.Active);

                    var cubeState = GetCubeState(position, cubes);

                    newCubes[position] = cubeState switch
                    {
                        CubeState.Active   => activeNeighborCubesCount == 2 || activeNeighborCubesCount == 3 ? CubeState.Active : CubeState.Inactive,
                        CubeState.Inactive => activeNeighborCubesCount == 3 ? CubeState.Active : CubeState.Inactive,
                        _                  => cubeState
                    };
                }

                cubes = newCubes;
            }

            return cubes.Values.CountValue(CubeState.Active);
        }

        private static IEnumerable<Position> GetCoreAndShellPositions(IEnumerable<Position> corePositions) =>
            corePositions.SelectMany(GetNeighborPositions).Distinct();

        private static CubeState GetCubeState(Position position, Dictionary<Position, CubeState> cubes) =>
            cubes.TryGetValue(position, out var state) ? state : CubeState.Inactive;

        private static IEnumerable<Position> GetNeighborPositions(Position position) =>
            position switch
            {
                Position3 position3 => GetNeighborPositions3(position3),
                Position4 position4 => GetNeighborPositions4(position4),
                _                   => Enumerable.Empty<Position>()
            };

        private static IEnumerable<Position3> GetNeighborPositions3(Position3 position) =>
            GetNeighborCoordinates(new[] { position.X, position.Y, position.Z }).Select(c => new Position3(c[0], c[1], c[2]));

        private static IEnumerable<Position4> GetNeighborPositions4(Position4 position) =>
            GetNeighborCoordinates(new[] { position.X, position.Y, position.Z, position.W }).Select(c => new Position4(c[0], c[1], c[2], c[3]));

        private static IEnumerable<int[]> GetNeighborCoordinates(int[] coordinates) =>
            GetNeighborCoordinates(coordinates, 0).Where(c => !c.SequenceEqual(coordinates));

        private static IEnumerable<int[]> GetNeighborCoordinates(int[] coordinates, int index)
        {
            if (index >= coordinates.Length)
                return new[] { coordinates };

            return Enumerable
                .Range(coordinates[index] - 1, 3)
                .SelectMany(c =>
                {
                    var newCoordinates = (int[])coordinates.Clone();
                    newCoordinates[index] = c;
                    return GetNeighborCoordinates(newCoordinates, index + 1);
                });
        }
    }
}
