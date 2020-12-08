using System;
using System.Collections.Generic;
using System.Linq;
using Aoc2020Net.Utilities;

namespace Aoc2020Net.Days
{
    internal sealed class Day8 : Day
    {
        private enum InstructionType
        {
            [Token("nop")] NoOperation,
            [Token("acc")] Accumulate,
            [Token("jmp")] Jump
        }

        private record Instruction(InstructionType InstructionType, int Argument);
        private record ContextChange(int AccumulatorChange, int InstructionIndexChange);

        private delegate ContextChange ExecuteInstruction(int argument);

        private static readonly Dictionary<InstructionType, ExecuteInstruction> InstructionsExecutors = new Dictionary<InstructionType, ExecuteInstruction>
        {
            [InstructionType.NoOperation] = _ => new ContextChange(0, 1),
            [InstructionType.Accumulate] = argument => new ContextChange(argument, 1),
            [InstructionType.Jump] = argument => new ContextChange(0, argument)
        };

        public override object SolvePart1() => GetAccumulator(GetInputInstructions()).Accumulator;

        public override object SolvePart2()
        {
            var instructions = GetInputInstructions();
            var instructionsToChangeIndices = instructions
                .Select((instruction, i) => instruction.InstructionType == InstructionType.Jump || instruction.InstructionType == InstructionType.NoOperation ? i : -1)
                .Where(i => i >= 0)
                .ToArray();

            foreach (var i in instructionsToChangeIndices)
            {
                var originalInstruction = instructions[i];

                var instructionsCopy = instructions.ToArray();
                instructionsCopy[i] = originalInstruction with
                {
                    InstructionType = originalInstruction.InstructionType == InstructionType.NoOperation
                        ? InstructionType.Jump
                        : InstructionType.NoOperation
                };

                var (accumulator, looped) = GetAccumulator(instructionsCopy);
                if (!looped)
                    return accumulator;
            }

            return null;
        }

        private (int Accumulator, bool Looped) GetAccumulator(Instruction[] instructions)
        {
            var visitedIndices = new HashSet<int>();
            
            var instructionIndex = 0;
            var accumulator = 0;

            while (instructionIndex < instructions.Length)
            {
                if (!visitedIndices.Add(instructionIndex))
                    return (accumulator, Looped: true);

                var instruction = instructions[instructionIndex];
                var contextChange = InstructionsExecutors[instruction.InstructionType](instruction.Argument);

                accumulator += contextChange.AccumulatorChange;
                instructionIndex += contextChange.InstructionIndexChange;
            }

            return (accumulator, Looped: false);
        }

        private Instruction[] GetInputInstructions()
        {
            var operationsNamesToTypes = ReflectionUtilities
                .GetAttributedEnumValues<InstructionType, TokenAttribute>()
                .ToDictionary(v => v.Attribute.Token, v => v.Value);

            return InputData
                .GetInputLines()
                .Select(line =>
                {
                    var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    return new Instruction(operationsNamesToTypes[parts[0]], int.Parse(parts[1]));
                })
                .ToArray();
        }
    }
}
