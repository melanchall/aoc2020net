using System.Collections.Generic;
using System.Linq;

namespace Aoc2020Net.Days
{
    internal sealed class Day23 : Day
    {
        public override object SolvePart1()
        {
            var initialCups = GetInitialCups().ToArray();
            var cupsNodes = Play(initialCups, 100);
            
            var cupNode = cupsNodes.Find(1);
            return Enumerable
                .Range(0, initialCups.Length - 1)
                .Aggregate(string.Empty, (result, _) => result + (cupNode = GetNextCupNode(cupsNodes, cupNode)).Value);
        }

        public override object SolvePart2()
        {
            var initialCups = GetInitialCups().ToArray();
            var additionalCups = Enumerable.Range(initialCups.Max() + 1, 1_000_000 - initialCups.Length);
            
            var cupsNodes = Play(initialCups.Concat(additionalCups), 10_000_000);
            
            var cupNode = cupsNodes.Find(1);
            var nextCupNode = GetNextCupNode(cupsNodes, cupNode);
            return (long)nextCupNode.Value * GetNextCupNode(cupsNodes, nextCupNode).Value;
        }

        private IEnumerable<int> GetInitialCups() => InputData.GetInputText().Select(c => int.Parse(c.ToString()));

        private static LinkedList<int> Play(IEnumerable<int> initialCups, int iterations)
        {
            var cupsNodes = new LinkedList<int>();
            var cupsToCupsNodes = new Dictionary<int, LinkedListNode<int>>();

            foreach (var cup in initialCups)
            {
                cupsToCupsNodes[cup] = cupsNodes.AddLast(cup);
            }

            var currentCupNode = cupsNodes.First;

            for (var i = 0; i < iterations; i++)
            {
                var cups = ExtractCupsAfter(cupsNodes, currentCupNode, cupsToCupsNodes);
                var destinationCupNode = GetDestination(cupsNodes, currentCupNode.Value, cupsToCupsNodes);
                InsertCupsAfter(cupsNodes, destinationCupNode, cups, cupsToCupsNodes);

                currentCupNode = GetNextCupNode(cupsNodes, currentCupNode);
            }

            return cupsNodes;
        }

        private static void InsertCupsAfter(LinkedList<int> cupsNodes, LinkedListNode<int> afterCupNode, int[] cups, Dictionary<int, LinkedListNode<int>> cupsToCupsNodes)
        {
            for (var i = 0; i < cups.Length; i++)
            {
                cupsToCupsNodes[cups[i]] = afterCupNode = cupsNodes.AddAfter(afterCupNode, cups[i]);
            }
        }

        private static LinkedListNode<int> GetDestination(LinkedList<int> cupsNodes, int currentCup, Dictionary<int, LinkedListNode<int>> cupsToCupsNodes)
        {
            currentCup--;

            for (; currentCup > 0; currentCup--)
            {
                var node = cupsToCupsNodes[currentCup];
                if (node != null)
                    return node;
            }

            return cupsToCupsNodes[cupsNodes.Max()];
        }

        private static int[] ExtractCupsAfter(LinkedList<int> cupsNodes, LinkedListNode<int> afterCupNode, Dictionary<int, LinkedListNode<int>> cupsToCupsNodes)
        {
            var i = 0;
            var result = new int[3];

            for (var cupNode = GetNextCupNode(cupsNodes, afterCupNode); i < 3; i++)
            {
                var nextCupNode = GetNextCupNode(cupsNodes, cupNode);
                cupsNodes.Remove(cupNode);
                result[i] = cupNode.Value;
                cupsToCupsNodes[cupNode.Value] = null;
                cupNode = nextCupNode;
            }

            return result;
        }

        private static LinkedListNode<int> GetNextCupNode(LinkedList<int> numbers, LinkedListNode<int> current) =>
            current.Next ?? numbers.First;
    }
}
