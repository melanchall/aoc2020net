namespace Aoc2020Net.Days
{
    internal sealed class Day3 : Day
    {
        private enum CellType
        {
            [GridSymbol('#')] Tree,
            [GridSymbol('.')] Space
        }

        public override object SolvePart1() => CountTrees((3, 1));

        public override object SolvePart2() => CountTrees((1, 1), (3, 1), (5, 1), (7, 1), (1, 2));

        private long CountTrees(params (int XOffset, int YOffset)[] slopes)
        {
            var (grid, width, height) = InputData.GetInputGrid<CellType>();

            var totalTreesCount = 1L;

            foreach (var slope in slopes)
            {
                var point = (X: 0, Y: 0);
                var slopeTreesCount = 0;

                do
                {
                    if (grid[point.X, point.Y] == CellType.Tree)
                        slopeTreesCount++;

                    point.X = (point.X + slope.XOffset) % width;
                    point.Y += slope.YOffset;
                }
                while (point.Y < height);

                totalTreesCount *= slopeTreesCount;
            }

            return totalTreesCount;
        }
    }
}
