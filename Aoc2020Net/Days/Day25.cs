namespace Aoc2020Net.Days
{
    internal sealed class Day25 : Day
    {
        public override object SolvePart1()
        {
            var publicKeys = InputData.GetInputInt64NumbersFromLines();
            var cardLoopSize = FindLoopSizeByPublicKey(7, publicKeys[0]);
            return TransformSubjectNumber(publicKeys[1], cardLoopSize);
        }

        public override object SolvePart2()
        {
            return "MERRY CHRISTMAS";
        }

        private long TransformSubjectNumber(long subjectNumber, int loopSize)
        {
            var value = 1L;

            for (var i = 0; i < loopSize; i++)
            {
                TransformValue(ref value, subjectNumber);
            }

            return value;
        }

        private int FindLoopSizeByPublicKey(long subjectNumber, long publicKey)
        {
            var value = 1L;

            for (var i = 0; ; i++)
            {
                TransformValue(ref value, subjectNumber);
                if (value == publicKey)
                    return i + 1;
            }
        }

        private void TransformValue(ref long value, long subjectNumber) => value = (value * subjectNumber) % 20201227;
    }
}
