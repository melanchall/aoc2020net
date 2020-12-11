using System;
using System.Collections.Generic;
using System.Linq;
using Aoc2020Net.Utilities;

namespace Aoc2020Net.Days
{
    internal sealed class Day11 : Day
    {
        private enum PlaceType
        {
            [GridSymbol('.')] Floor = 0,
            [GridSymbol('#')] OccupiedSeat,
            [GridSymbol('L')] EmptySeat
        }

        public override object SolvePart1() => CountStabilizedOccupiedSeats(GetAdjacentPlaces, 4);

        public override object SolvePart2() => CountStabilizedOccupiedSeats(GetVisiblePlaces, 5);

        private int CountStabilizedOccupiedSeats(Func<PlaceType[,], int, int, int, int, IEnumerable<PlaceType>> seatsSelector, int minOccupiedSeatsNumberToMakeEmpty)
        {
            var (grid, width, height) = InputData.GetInputGrid<PlaceType>();

            while (true)
            {
                var newGrid = new PlaceType[width, height];

                for (var x = 0; x < width; x++)
                {
                    for (var y = 0; y < height; y++)
                    {
                        newGrid[x, y] = grid[x, y] switch
                        {
                            PlaceType.EmptySeat => seatsSelector(grid, width, height, x, y).All(p => p != PlaceType.OccupiedSeat)
                                ? PlaceType.OccupiedSeat
                                : PlaceType.EmptySeat,
                            PlaceType.OccupiedSeat => seatsSelector(grid, width, height, x, y).CountValue(PlaceType.OccupiedSeat) >= minOccupiedSeatsNumberToMakeEmpty
                                ? PlaceType.EmptySeat
                                : PlaceType.OccupiedSeat,
                            _ => PlaceType.Floor
                        };
                    }
                }

                if (newGrid.OfType<PlaceType>().SequenceEqual(grid.OfType<PlaceType>()))
                    return grid.OfType<PlaceType>().CountValue(PlaceType.OccupiedSeat);

                grid = newGrid;
            }
        }

        private IEnumerable<PlaceType> GetAdjacentPlaces(PlaceType[,] grid, int width, int height, int pointX, int pointY)
        {
            for (var x = pointX - 1; x <= pointX + 1; x++)
            {
                for (var y = pointY - 1; y <= pointY + 1; y++)
                {
                    if ((x == pointX && y == pointY) || x < 0 || x >= width || y < 0 || y >= height)
                        continue;

                    yield return grid[x, y];
                }
            }
        }

        private IEnumerable<PlaceType> GetVisiblePlaces(PlaceType[,] grid, int width, int height, int x, int y)
        {
            yield return GetFirstSeat(Enumerable.Range(1, x).Select(i => grid[x - i, y]));
            yield return GetFirstSeat(Enumerable.Range(x + 1, width - x - 1).Select(i => grid[i, y]));

            yield return GetFirstSeat(Enumerable.Range(1, y).Select(i => grid[x, y - i]));
            yield return GetFirstSeat(Enumerable.Range(y + 1, height - y - 1).Select(i => grid[x, i]));

            yield return GetFirstSeat(Enumerable.Range(1, x)
                .Zip(Enumerable.Range(1, y), (xx, yy) => new { X = x - xx, Y = y - yy })
                .Where(p => p.X >= 0 && p.Y >= 0)
                .Select(p => grid[p.X, p.Y]));

            yield return GetFirstSeat(Enumerable.Range(x + 1, width - x - 1)
                .Zip(Enumerable.Range(y + 1, height - y - 1), (xx, yy) => new { X = xx, Y = yy })
                .Where(p => p.X < width && p.Y < height)
                .Select(p => grid[p.X, p.Y]));

            yield return GetFirstSeat(Enumerable.Range(1, x)
                .Zip(Enumerable.Range(y + 1, height - y - 1), (xx, yy) => new { X = x - xx, Y = yy })
                .Where(p => p.X >= 0 && p.Y < height)
                .Select(p => grid[p.X, p.Y]));

            yield return GetFirstSeat(Enumerable.Range(x + 1, width - x - 1)
                .Zip(Enumerable.Range(1, y), (xx, yy) => new { X = xx, Y = y - yy })
                .Where(p => p.X < width && p.Y >= 0)
                .Select(p => grid[p.X, p.Y]));
        }

        private static PlaceType GetFirstSeat(IEnumerable<PlaceType> places) => places.FirstOrDefault(p => p != PlaceType.Floor);
    }
}
