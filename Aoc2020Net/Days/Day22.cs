using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2020Net.Days
{
    internal sealed class Day22 : Day
    {
        public override object SolvePart1() => GetWinningScore(false);

        public override object SolvePart2() => GetWinningScore(true);

        private int GetWinningScore(bool playRecursive)
        {
            var linesGroups = InputData.GetInputLinesGroups();
            var firstPlayerDeck = new Queue<int>(linesGroups[0][1..].Select(int.Parse));
            var secondPlayerDeck = new Queue<int>(linesGroups[1][1..].Select(int.Parse));

            return (IsFirstPlayerWin(firstPlayerDeck, secondPlayerDeck, playRecursive) ? firstPlayerDeck : secondPlayerDeck)
                .Reverse()
                .Select((n, i) => n * (i + 1))
                .Sum();
        }

        private static bool IsFirstPlayerWin(Queue<int> firstPlayerDeck, Queue<int> secondPlayerDeck, bool playRecursive)
        {
            var firstPlayerTurns = new List<int[]>();
            var secondPlayerTurns = new List<int[]>();

            while (firstPlayerDeck.Any() && secondPlayerDeck.Any())
            {
                var x = firstPlayerDeck.Dequeue();
                var y = secondPlayerDeck.Dequeue();

                var firstPlayerWins = firstPlayerDeck.Count >= x && secondPlayerDeck.Count >= y && playRecursive
                    ? IsFirstPlayerWin(new Queue<int>(firstPlayerDeck.Take(x)), new Queue<int>(secondPlayerDeck.Take(y)), playRecursive)
                    : x > y;

                if (firstPlayerWins)
                {
                    firstPlayerDeck.Enqueue(x);
                    firstPlayerDeck.Enqueue(y);
                }
                else
                {
                    secondPlayerDeck.Enqueue(y);
                    secondPlayerDeck.Enqueue(x);
                }

                if (firstPlayerTurns.Any(t => t.SequenceEqual(firstPlayerDeck)) &&
                    secondPlayerTurns.Any(t => t.SequenceEqual(secondPlayerDeck)))
                    return true;

                firstPlayerTurns.Add(firstPlayerDeck.ToArray());
                secondPlayerTurns.Add(secondPlayerDeck.ToArray());
            }

            return firstPlayerDeck.Any();
        }
    }
}
