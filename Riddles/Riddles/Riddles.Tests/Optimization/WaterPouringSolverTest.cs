using NUnit.Framework;
using Riddles.Optimization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Tests.Optimization
{
    public class WaterPouringSolverTest
    {
        private Dictionary<int, (List<int>, Dictionary<int, int>)> _testCases
            = new Dictionary<int, (List<int>, Dictionary<int, int>)>
            {
                { 1, (new List<int> { 10, 3, }, new Dictionary<int, int>{
                    { 0, 0 },
                    { 1, 6 },
                    { 2, 8 },
                    { 3, 1 },
                    { 4, 4 },
                    { 5, 12 },
                    { 6, 4 },
                    { 7, 2 },
                    { 8, 10 },
                    { 9, 6 },
                    { 10, 1 }
                })}
            };


        [TestCase(1)]
        public void TestCalculateNumMovesToFindExactlyNLitersOfWater(
            int testCaseId)
        {
            var waterPouringSolver = new WaterPouringSolver();
            var testCase = _testCases[testCaseId];
            var actualResult = waterPouringSolver
                .CalculateNumMovesToFindExactlyNLitersOfWater(
                    testCase.Item1
                );
            Assert.AreEqual( testCase.Item2.Keys.Count, actualResult.Keys.Count );
            foreach(var key in testCase.Item2.Keys)
            {
                Assert.AreEqual(testCase.Item2[key], actualResult[key]);
            }
        }

        [TestCase(new int[] { 10, 3}, 5, 12)]
        [TestCase(new int[] { 100, 93 }, 50, 190)]
        public void TestCalculateNumberOfMovesForSlowestValue(
            int[] pitchers, 
            int slowestValue, 
            int numMoves)
        {
            var waterPouringSolver = new WaterPouringSolver();
            var slowestPair = waterPouringSolver
                .CalculateNumberOfMovesForSlowestValue(pitchers.ToList());
            Assert.AreEqual(slowestPair.Item1, slowestValue);
            Assert.AreEqual(slowestPair.Item2, numMoves);
        }
    }
}
