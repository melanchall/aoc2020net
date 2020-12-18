using System;
using System.Linq;
using System.Text.RegularExpressions;
using Aoc2020Net.Utilities;

namespace Aoc2020Net.Days
{
    internal sealed class Day18 : Day
    {
        public override object SolvePart1() => GetEvaluatedExpressionsSum(EvaluateSimpleExpression);

        public override object SolvePart2() => GetEvaluatedExpressionsSum(EvaluateSimpleExpressionWithAdditionFirst);

        private long GetEvaluatedExpressionsSum(Func<string, long> evaluateSimpleExpression) =>
            InputData.GetInputLines().Select(line => EvaluateExpression(line, evaluateSimpleExpression)).Sum();

        private static long EvaluateExpression(string expression, Func<string, long> evaluateSimpleExpression)
        {
            var subExpressionRegex = new Regex(@"\([^()]+?\)");
            var subExpressionsCount = expression.CountValue('(');

            for (var i = 0; i < subExpressionsCount; i++)
            {
                expression = subExpressionRegex.Replace(expression, m => evaluateSimpleExpression(m.Value.Trim('(', ')')).ToString());
            }

            return evaluateSimpleExpression(expression);
        }

        private static long EvaluateSimpleExpressionWithAdditionFirst(string expression)
        {
            var expressionWithAdditionsInParentheses = Regex.Replace(expression, @"[\d\s+]+", m => $" ({m.Value.Trim()}) ");
            return EvaluateExpression(expressionWithAdditionsInParentheses, EvaluateSimpleExpression);
        }

        private static long EvaluateSimpleExpression(string expression)
        {
            const char noOperation = ' ';
            const string noNumber = "";

            var result = 0L;

            var operation = noOperation;
            var numberString = noNumber;

            foreach (var symbol in $"{expression}.".Where(c => c != ' '))
            {
                if (char.IsDigit(symbol))
                    numberString += symbol;
                else
                {
                    var number = long.Parse(numberString);
                    result = operation == noOperation
                        ? number
                        : operation switch
                        {
                            '+' => result + number,
                            '*' => result * number,
                            _   => result
                        };

                    operation = symbol;
                    numberString = noNumber;
                }
            }

            return result;
        }
    }
}
