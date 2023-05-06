using NUnit.Framework;
using System;
using Riddles.Combinatorics.Application;
using System.Collections.Generic;
using System.Linq;

namespace Riddles.Tests.Combinatorics.Application
{
    public class DigitGameNumDistinctEquationsCalculatorTest
    {
        Dictionary<int, (int, int)> testCaseDictionary
            = new Dictionary<int, (int, int)>
            {
                { 1, (1, 1) },
                { 2, (2, 8) },
                { 3, (3, 3+18+68) },
                // 0:A 1:B 2:C
                // 0:(A+B) 1:(A-B) 8:(B-A) 2:(A*B) 4:(A/B) 9:(B/A)
                // 4:(A+C) 5:(A-C) 14:(C-A) 6:(A*C) 7:(A/C) 15:(C/A)
                // 10:(B+C) 11:(B-C) 16:(C-B) 12:(B*C) 13:(B/C) 17:(C/B)
                // 0:(A+B+C) 4:(A+B-C) 5:(A-B+C) 23:(B+C-A) 1:(A-B-C) 20:(B-A-C) 36:(C-A-B)
                // 10:(A*B*C) 11:(A/B/C) 28:(B/A/C) 43:(C/A/B) 14:(A*B/C) 15:(A*C/B) 31:(B*C/A)
                // 2:(A*(B+C)) 6:(A*(B-C)) 16: (A*(C-B)) 3:(A/(B+C)) 7:(A/(B-C)) 17:(A/(C-B))
                // 8:(A+(B*C)) 12:(A+(B/C)) 18: (A+(C/B)) 9:(A-(B*C)) 13:(A-(B/C)) 19:(A-(C/B))
                // 26:(B+(A*C)) 29:(B+(A/C)) 34:(B+(C/A)) 27:(B-(A*C)) 30:(B-(A/C)) 35:(B-(C/A))
                // 21:(B*(A+C)) 24:(B*(A-C)) 32:(B*(C-A)) 22:(B/(A+C)) 25:(B/(A-C)) 33:(B/(C-A))
                // 41:(C+(A*B)) 44:(C+(A/B)) 48:(C+(B/A)) 42:(C-(A*B)) 45:(C-(A/B)) 49:(C-(B/A))
                // 37:(C*(A+B)) 39:(C*(A-B)) 46:(C*(B-A)) 38:(C/(A+B)) 40:(C/(A-B)) 47:(C/(B-A))
                // 50:((A+B)/C) 51:((A-B)/C) 58:((B-A)/C) 52:((A*B)-C) 53:((A/B)-C) 59:((B/A)-C)
                // 54:((A+C)/B) 55:((A-C)/B) 64:((C-A)/B) 56:((A*C)-B) 57:((A/C)-B) 65:((C/A)-B)
                // 60:((B+C)/A) 61:((B-C)/A) 66:((C-B)/A) 62:((B*C)-A) 63:((B/C)-A) 67:((C/B)-A)
                {4, (4, 1_482) },
                {5, (5, 33_737) },
                {6, (6, 974_860) }
            };

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        //[TestCase(6)] // takes 7 s
        public void TestCalculateNumDistinctEquations(int testCaseId)
        {
            var digitGameNumDistinctEquationsCalculator =
                new DigitGameNumDistinctEquationsCalculator();
            var random = new Random();
            var variables = Enumerable.Range(0, testCaseId)
                .Select(x => random.NextDouble())
                .ToArray();
            var numDistinctEquations = digitGameNumDistinctEquationsCalculator
                .CalculateNumDistinctEquations(variables);
            Assert.AreEqual(
                numDistinctEquations,
                testCaseDictionary[testCaseId].Item2);
        }
    }
}
