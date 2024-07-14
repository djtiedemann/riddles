using NUnit.Framework;
using Riddles.MarkovChains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.MarkovChains
{
    public class TennisGameSolverTest
    {
        private Dictionary<int, (int, int, Dictionary<int, double>)>
            _gameTestCaseDictionary =
                new Dictionary<int, (int, int, Dictionary<int, double>)>
                {
                    { 1, (4, 2, new Dictionary<int, double>{
                        { 4, 1.0/8.0 },
                        { 3, 1.0/4.0 },
                        { 2, 5.0/8.0 }
                    }) },
                    { 2, (7, 2, new Dictionary<int, double>{
                        { 7, 2.0/128.0 },
                        { 6, 7.0/128.0 },
                        { 5, 14.0/128.0 },
                        { 4, 21.0/128.0 },
                        { 3, 105.0/512.0 },
                        { 2, 231.0/512.0 }
                    }) }

                };
        [TestCase(1)]
        [TestCase(2)]
        public void TestCalculateOddsOfWinningGameByXPoints(int testCaseId)
        {
            var tennisGameSolver = new TennisGameSolver();
            var testCase = this._gameTestCaseDictionary[testCaseId];
            var outcomes = tennisGameSolver.CalculateOddsOfWinningGameByXPoints(
                testCase.Item1,
                testCase.Item2
            );
            Assert.AreEqual(testCase.Item3.Keys.Count, outcomes.Keys.Count);
            foreach(var key in testCase.Item3.Keys)
            {
                Assert.IsTrue(outcomes.ContainsKey(key));
                Assert.AreEqual(testCase.Item3[key], outcomes[key]);
            }
        }
    }
}
