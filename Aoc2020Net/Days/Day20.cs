using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Aoc2020Net.Utilities;

namespace Aoc2020Net.Days
{
    internal sealed class Day20 : Day
    {
        private enum PixelType
        {
            [GridSymbol('#')] Black,
            [GridSymbol('.')] White
        }

        private enum BorderType
        {
            Bottom,
            Top,
            Left,
            Right
        }

        private static readonly Action<PixelType[,]>[] Flips = new Action<PixelType[,]>[]
        {
            _ => { },
            FlipVertically,
            FlipHorizontally
        };

        public override object SolvePart1()
        {
            var (tilesIdsGrid, _) = GetImageData();
            var tilesIdsGridSize = tilesIdsGrid.GetLength(0);
            return (long)tilesIdsGrid[0, 0] *
                         tilesIdsGrid[0, tilesIdsGridSize - 1] *
                         tilesIdsGrid[tilesIdsGridSize - 1, 0] *
                         tilesIdsGrid[tilesIdsGridSize - 1, tilesIdsGridSize - 1];
        }

        public override object SolvePart2()
        {
            var (tilesIdsGrid, tiles) = GetImageData();
            var image = GetImage(tilesIdsGrid, tiles);

            foreach (var flip in Flips)
            {
                flip(image);

                for (var i = 0; i < 4; i++)
                {
                    Rotate(ref image);

                    var monstersCount = GetMonstersCount(image);
                    if (monstersCount > 0)
                        return image.CountValue(PixelType.Black) - monstersCount * 15;
                }
            }

            return null;
        }

        private static PixelType[,] GetImage(int[,] tilesIdsGrid, Dictionary<int, PixelType[,]> tiles)
        {
            var tilesIdsGridSize = tilesIdsGrid.GetLength(0);
            var tileSize = tiles.First().Value.GetLength(0);
            var innerTileSize = tileSize - 2;

            var image = new PixelType[tilesIdsGridSize * innerTileSize, tilesIdsGridSize * innerTileSize];

            DataProvider.GetGridCoordinates(tilesIdsGridSize, tilesIdsGridSize)
                .ToList()
                .ForEach(xy =>
                {
                    var tileId = tilesIdsGrid[xy.X, xy.Y];
                    var tile = tiles[tileId];

                    DataProvider.GetGridCoordinates(innerTileSize, innerTileSize)
                        .ToList()
                        .ForEach(subXY => image[xy.X * innerTileSize + subXY.X, xy.Y * innerTileSize + subXY.Y] = tile[subXY.X + 1, subXY.Y + 1]);
                });

            return image;
        }

        private static int GetMonstersCount(PixelType[,] image)
        {
            var imageSize = image.GetLength(0);
            var lines = GetImageLines(image);

            return Enumerable
                .Range(1, imageSize - 2)
                .Sum(y => Regex.Matches(lines[y], "#....##....##....###")
                               .OfType<Match>()
                               .Count(match => Regex.IsMatch(lines[y - 1][match.Index..], "^..................#.") &&
                                               Regex.IsMatch(lines[y + 1][match.Index..], "^.#..#..#..#..#..#...")));
        }

        private static string[] GetImageLines(PixelType[,] grid)
        {
            var size = grid.GetLength(0);
            return Enumerable.Range(0, size)
                .Select(y => string.Join(string.Empty, Enumerable.Range(0, size).Select(x => grid[x, y] == PixelType.White ? '.' : '#')))
                .ToArray();
        }

        private (int[,] TilesIdsGrid, Dictionary<int, PixelType[,]> Tiles) GetImageData()
        {
            var tiles = InputData.GetInputLinesGroups()
                .Select(g =>
                {
                    var tileId = Regex.Match(g.First(), @"Tile (\d+):").GetInt32Group(1);
                    var tile = InputData.GetGrid<PixelType>(g.Skip(1).ToArray());
                    return (tileId, tile.Grid);
                })
                .ToDictionary(t => t.tileId, t => t.Grid);

            var tilesBorders = tiles.ToDictionary(t => t.Key, t => GetBordersVariants(t.Value).ToArray());
            var neighborTilesIds = tiles.Keys.ToDictionary(
                id => id,
                id => tiles.Keys.Where(otherTileId => otherTileId != id && tilesBorders[otherTileId].Any(b => tilesBorders[id].Any(bb => bb.SequenceEqual(b)))).ToArray());

            //

            var centerTileId = neighborTilesIds.First(id => id.Value.Count() == 4).Key;
            var fixedTilesIds = new HashSet<int> { centerTileId };
            var freeTilesIds = tiles.Keys.Except(fixedTilesIds).ToList();
            var fixedTiles = new Dictionary<int, PixelType[,]> { [centerTileId] = tiles[centerTileId] };
            var tilesPositions = new Dictionary<int, (int X, int Y)> { [centerTileId] = (0, 0) };

            for (var i = freeTilesIds.Count - 1; i >= 0; i--)
            {
                var attached = false;

                foreach (var fromTileId in freeTilesIds)
                {
                    var fromTile = tiles[fromTileId];

                    foreach (var toTileId in fixedTilesIds)
                    {
                        if (!neighborTilesIds[toTileId].Contains(fromTileId))
                            continue;

                        var toTile = fixedTiles[toTileId];
                        var toTilePosition = tilesPositions[toTileId];

                        var attachBorder = GetAttachBorder(ref fromTile, toTile);
                        if (attachBorder != null)
                        {
                            attached = true;

                            var position = attachBorder switch
                            {
                                BorderType.Bottom => (toTilePosition.X, toTilePosition.Y + 1),
                                BorderType.Top => (toTilePosition.X, toTilePosition.Y - 1),
                                BorderType.Left => (toTilePosition.X + 1, toTilePosition.Y),
                                BorderType.Right => (toTilePosition.X - 1, toTilePosition.Y)
                            };

                            fixedTiles[fromTileId] = fromTile;
                            tilesPositions[fromTileId] = position;
                            break;
                        }
                    }

                    if (attached)
                    {
                        fixedTilesIds.Add(fromTileId);
                        freeTilesIds.Remove(fromTileId);
                        break;
                    }
                }
            }

            var tilesIdsGrid = GetTilesIdsGrid(tilesPositions);
            return (tilesIdsGrid, fixedTiles);
        }

        private static int[,] GetTilesIdsGrid(Dictionary<int, (int X, int Y)> tilesPositions)
        {
            var positions = tilesPositions.Values;
            var minX = positions.Select(p => p.X).Min();
            var maxX = positions.Select(p => p.X).Max();
            var minY = positions.Select(p => p.Y).Min();
            var maxY = positions.Select(p => p.Y).Max();

            var width = maxX - minX + 1;
            var height = maxY - minY + 1;

            var tilesIdsGrid = new int[width, height];
            DataProvider.GetGridCoordinates(width, height)
                .ToList()
                .ForEach(xy => tilesIdsGrid[xy.X, xy.Y] = tilesPositions.First(p => p.Value.X == minX + xy.X && p.Value.Y == maxY - xy.Y).Key);

            return tilesIdsGrid;
        }

        private static BorderType? GetAttachBorder(ref PixelType[,] tile, PixelType[,] to)
        {
            if (CanBeAttachedToTile(ref tile, GetBottomBorder, to, GetTopBorder))
                return BorderType.Bottom;

            if (CanBeAttachedToTile(ref tile, GetTopBorder, to, GetBottomBorder))
                return BorderType.Top;

            if (CanBeAttachedToTile(ref tile, GetLeftBorder, to, GetRightBorder))
                return BorderType.Left;

            if (CanBeAttachedToTile(ref tile, GetRightBorder, to, GetLeftBorder))
                return BorderType.Right;

            return null;
        }

        private static bool CanBeAttachedToTile(ref PixelType[,] tile, Func<PixelType[,], PixelType[]> getTileBorder, PixelType[,] to, Func<PixelType[,], PixelType[]> getToBorder)
        {
            foreach (var flip in Flips)
            {
                flip(tile);

                for (var i = 0; i < 4; i++)
                {
                    Rotate(ref tile);
                    if (getTileBorder(tile).SequenceEqual(getToBorder(to)))
                        return true;
                }
            }

            return false;
        }

        private static PixelType[] GetBottomBorder(PixelType[,] tile) => GetBorder(tile, (size, p) => (p, size - 1));

        private static PixelType[] GetTopBorder(PixelType[,] tile) => GetBorder(tile, (size, p) => (p, 0));

        private static PixelType[] GetLeftBorder(PixelType[,] tile) => GetBorder(tile, (size, p) => (0, p));

        private static PixelType[] GetRightBorder(PixelType[,] tile) => GetBorder(tile, (size, p) => (size - 1, p));

        private static PixelType[] GetBorder(PixelType[,] tile, Func<int, int, (int X, int Y)> getPoint)
        {
            var size = tile.GetLength(0);
            return (from p in Enumerable.Range(0, size)
                    let point = getPoint(size, p)
                    select tile[point.X, point.Y]).ToArray();
        }

        private static void FlipVertically(PixelType[,] tile)
        {
            var size = tile.GetLength(0);
            DataProvider.GetGridCoordinates(size, size / 2)
                .ToList()
                .ForEach(xy => Swap(tile, xy.X, xy.Y, xy.X, size - xy.Y - 1));
        }

        private static void FlipHorizontally(PixelType[,] tile)
        {
            var size = tile.GetLength(0);
            DataProvider.GetGridCoordinates(size / 2, size)
                .ToList()
                .ForEach(xy => Swap(tile, xy.X, xy.Y, size - xy.X - 1, xy.Y));
        }

        private static void Swap(PixelType[,] tile, int x1, int y1, int x2, int y2)
        {
            var tmp = tile[x1, y1];
            tile[x1, y1] = tile[x2, y2];
            tile[x2, y2] = tmp;
        }

        private static void Rotate(ref PixelType[,] tile)
        {
            var size = tile.GetLength(0);
            var result = new PixelType[size, size];

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    result[x, y] = tile[size - y - 1, x];
                }
            }

            tile = result;
        }

        private static IEnumerable<PixelType[]> GetBordersVariants(PixelType[,] tile) =>
            new[]
            {
                GetTopBorder(tile),
                GetBottomBorder(tile),
                GetLeftBorder(tile),
                GetRightBorder(tile)
            }
            .SelectMany(border => new[] { border, border.Reverse().ToArray() });
    }
}
