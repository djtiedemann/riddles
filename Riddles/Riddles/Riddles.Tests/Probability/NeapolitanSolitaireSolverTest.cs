using NUnit.Framework;
using System;
using System.Collections.Generic;
using Riddles.Probability;
using System.Linq;

namespace Riddles.Tests.Probability
{
    public class NeapolitanSolitaireSolverTest
    {
        private double Epsilon = 0.00000001;
        private Dictionary<int, (Dictionary<string, int>, Dictionary<string, HashSet<int>>, double)>
            TestCaseDictionary
            = new Dictionary<int, (Dictionary<string, int>, Dictionary<string, HashSet<int>>, double)> {
                {1, (
                    new Dictionary<string, int> { { "1", 1}, {"2", 1 } },
                    new Dictionary<string, HashSet<int>> { 
                        { "1", new HashSet<int> { 0 } },
                        { "2", new HashSet<int> { 1 } } 
                    },
                    0.5 )
                },
                {2, (
                    new Dictionary<string, int> { { "1", 1}, {"2", 1 } },
                    new Dictionary<string, HashSet<int>> { 
                        { "1", new HashSet<int> { 0, 1 } } 
                    },
                    0.0 )
                },
                {3, (
                    new Dictionary<string, int> { { "1", 1}, {"2", 1 } },
                    new Dictionary<string, HashSet<int>> {
                        { "1", new HashSet<int> { 0 } },
                        { "2", new HashSet<int> { 0 } } 
                    },
                    0.0 )
                },
                {4, (
                    new Dictionary<string, int> { { "1", 1}, {"2", 1 } },
                    new Dictionary<string, HashSet<int>> { { "3", new HashSet<int> { 0, 1} } },
                    1.0 )
                },
                {5, (
                    new Dictionary<string, int> { { "1", 1}, {"2", 1 }, {"3", 1 } },
                    new Dictionary<string, HashSet<int>> {
                        { "1", new HashSet<int> { 0 } },
                        { "2", new HashSet<int> { 1 } },
                        { "3", new HashSet<int> { 2 } },
                    },
                    1.0/3.0 )
                },
                {6, (
                    new Dictionary<string, int> { { "1", 1}, {"2", 1 }, {"3", 1 } },
                    new Dictionary<string, HashSet<int>> { 
                        { "1", new HashSet<int> { 2 } },
                        { "3", new HashSet<int> { 2 } }, 
                    },
                    1.0/3.0 )
                },
                {7, (
                    new Dictionary<string, int> {
                        { "1", 2}, {"2", 2 }, {"3", 2 }, {"4", 2 }, {"5", 2 }},
                    new Dictionary<string, HashSet<int>> {
                        { "1", new HashSet<int> { 1, 3, 5, 7, 9 } }
                    },
                    2.0/9.0 )
                },
                {8, (
                    new Dictionary<string, int> {
                        { "1", 2}, {"2", 2 }, {"3", 2 }, {"4", 2 }, {"5", 2 }},
                    new Dictionary<string, HashSet<int>> {
                        { "1", new HashSet<int> { 0, 1, 2, 3, 4 } }
                    },
                    2.0/9.0 )
                },
                {9, (
                    new Dictionary<string, int> {
                        { "1", 4}, {"2", 4 }, {"3", 4 }, {"4", 28 }
                    },
                    new Dictionary<string, HashSet<int>> {
                        { "1", new HashSet<int> { 0, 3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36, 39 } },
                        { "2", new HashSet<int> { 1, 4, 7, 10, 13, 16, 19, 22, 25, 28, 31, 34, 37 } },
                        { "3", new HashSet<int> { 2, 5, 8, 11, 14, 17, 20, 23, 26, 29, 32, 35, 38 } }
                    },
                    0.0083070055718199344 )
                },
            };

        [TestCase(1)] // 1 and 2, 1 can't be first
        [TestCase(2)] // 1 and 2, 1 can't be first or second: Impossible
        [TestCase(3)] // 1 and 2, 1 and 2 can't be first: Impossible
        [TestCase(4)] // 1 and 2, only conditions involve 3: Always possible
        [TestCase(5)] // 1, 2, and 3: no numbers can be in their slot
        [TestCase(6)] // 1, 2, and 3: 1 and 3 can't be last
        [TestCase(7)] // 2 copies of 1-5. 1 can't be in an even spot (5/10)*(4/9)
        [TestCase(8)] // 2 copies of 1-5: 1 can't be in first 5 slots (5/10)*(4/9)
        [TestCase(9)] // 4 copies of 1-10: 1 can't be in a slot %3 = 1, 2 can't be in %3 = 2,
        // 3 can't be in %3 = 0
        public void TestGetOddsOfSuccess(int testCaseId)
        {
            var testCase = TestCaseDictionary[testCaseId];
            var neapolitanSolitaireSolver = new NeapolitanSolitaireSolver();
            var oddsOfSuccess = neapolitanSolitaireSolver.CalculateOddsOfSuccess(
                testCase.Item1, testCase.Item2
            );
            Assert.LessOrEqual(Math.Abs(oddsOfSuccess - testCase.Item3), Epsilon);
        }

        // [TestCase(9)]
        // correct answer: 0.0083070055718199344
        // 0.008324
        // 0.0082895
        // 0.0082549
        // 0.0083064
        // 0.0083057
        // 0.0082889
        // 0.0083491
        public void SimulateGame(int testCaseId)
        {
            var testCase = TestCaseDictionary[testCaseId];
            var numIterations = 10_000_000;
            var cards =
                Enumerable.Range(0, 4).Select(c => "1")
                .Concat(Enumerable.Range(0, 4).Select(c => "2"))
                .Concat(Enumerable.Range(0, 4).Select(c => "3"))
                .Concat(Enumerable.Range(0, 28).Select(c => "4")).ToArray();
            var random = new Random();
            var restrictions = testCase.Item2;
            var numSuccesses = 0;
            for(int iteration=0; iteration<numIterations; iteration++)
            {
                cards = cards.OrderBy(x => random.Next()).ToArray();
                var isSuccess = true;
                for(int i=0; i<cards.Count(); i++)
                {
                    if (restrictions.ContainsKey(cards[i]) && restrictions[cards[i]].Contains(i))
                    {
                        isSuccess = false;
                        break;
                    }
                }
                if(isSuccess)
                {
                    numSuccesses++;
                }
            }
            var result = (double)numSuccesses / numIterations;
        }
    }
}
