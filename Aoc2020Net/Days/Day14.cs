using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Aoc2020Net.Utilities;

namespace Aoc2020Net.Days
{
    internal sealed class Day14 : Day
    {
        private abstract record Instruction();
        private record SetMaskInstruction(string Mask) : Instruction;
        private record WriteValueInstruction(long Address, long Value) : Instruction;

        public override object SolvePart1() => SumMemoryValues((writeValueInstruction, mask, memory) =>
        {
            memory[writeValueInstruction.Address] = ApplyMaskToValue(writeValueInstruction.Value, mask);
        });

        public override object SolvePart2() => SumMemoryValues((writeValueInstruction, mask, memory) =>
        {
            foreach (var address in GetAddressesByMask(writeValueInstruction.Address, mask))
            {
                memory[address] = writeValueInstruction.Value;
            }
        });

        private long SumMemoryValues(Action<WriteValueInstruction, string, Dictionary<long, long>> writeValueHandler)
        {
            var memory = new Dictionary<long, long>();
            var mask = string.Empty;

            foreach (var instruction in GetInputInstructions())
            {
                switch (instruction)
                {
                    case SetMaskInstruction setMaskInstruction:
                        mask = setMaskInstruction.Mask;
                        break;
                    case WriteValueInstruction writeValueInstruction:
                        writeValueHandler(writeValueInstruction, mask, memory);
                        break;
                }
            }

            return memory.Values.Sum();
        }

        private long ApplyMaskToValue(long value, string mask) =>
            Convert.ToInt64(ApplyMask(value, mask, 'X'), 2);

        private IEnumerable<long> GetAddressesByMask(long addressBase, string mask) =>
            GetAddressesByFloatingBits(ApplyMask(addressBase, mask, '0'), 0).Select(a => Convert.ToInt64(a, 2));

        private string ApplyMask(long value, string mask, char ignoreMaskSymbol)
        {
            var sValue = Convert.ToString(value, 2).PadLeft(mask.Length, '0');
            return new string(sValue.Zip(mask, (v, m) => m != ignoreMaskSymbol ? m : v).ToArray());
        }

        private string[] GetAddressesByFloatingBits(string address, int index)
        {
            var xIndex = address.IndexOf('X', index);
            if (xIndex < 0)
                return new[] { address };

            return new[] { '0', '1' }
                .SelectMany(c => GetAddressesByFloatingBits(GetStringWithChangedChar(address, xIndex, c), xIndex + 1))
                .ToArray();
        }

        private string GetStringWithChangedChar(string s, int index, char c)
        {
            var stringBuilder = new StringBuilder(s);
            stringBuilder[index] = c;
            return stringBuilder.ToString();
        }

        private IEnumerable<Instruction> GetInputInstructions() => InputData.GetInputLines().Select(ParseInstruction);

        private Instruction ParseInstruction(string line) =>
            TryParseSetMaskInstruction(line) ?? TryParseWriteValueInstruction(line);

        private Instruction TryParseSetMaskInstruction(string line) => TryParseInstruction(
            line,
            "mask = ([0X1]+)",
            match => new SetMaskInstruction(match.GetStringGroup(1)));

        private Instruction TryParseWriteValueInstruction(string line) => TryParseInstruction(
            line,
            @"mem\[(\d+)\] = (\d+)",
            match => new WriteValueInstruction(match.GetInt64Group(1), match.GetInt64Group(2)));

        private TInstruction TryParseInstruction<TInstruction>(string line, string regexPattern, Func<Match, TInstruction> createInstruction)
            where TInstruction : Instruction
        {
            var regex = new Regex(regexPattern);
            var match = regex.Match(line);
            if (!match.Success)
                return null;

            return createInstruction(match);
        }
    }
}
