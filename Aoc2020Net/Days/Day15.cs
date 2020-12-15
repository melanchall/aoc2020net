using System.Linq;

namespace Aoc2020Net.Days
{
    internal sealed class Day15 : Day
    {
        private const int UndefinedTurn = -1;

        public override object SolvePart1() => GetNumber(2020);

        public override object SolvePart2() => GetNumber(30000000);

        private int GetNumber(int turnNumber)
        {
            var numbers = InputData.GetCommaSeparatedInt32Numbers();
            var numbersTurns = numbers
                .Select((n, i) => new { Number = n, Turn = i + 1 })
                .ToDictionary(n => n.Number, n => (LastTurn: n.Turn, PreviousTurn: UndefinedTurn));
            var lastNumber = numbers.Last();

            for (var i = numbers.Length + 1; i <= turnNumber; i++)
            {
                lastNumber = !numbersTurns.TryGetValue(lastNumber, out var turns) || turns.PreviousTurn < 0
                    ? 0
                    : turns.LastTurn - turns.PreviousTurn;

                numbersTurns[lastNumber] = !numbersTurns.TryGetValue(lastNumber, out turns)
                    ? (i, UndefinedTurn)
                    : (i, turns.LastTurn);
            }

            return lastNumber;
        }
    }
}
