using NUnit.Framework;
using Riddles.MarkovChains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.MarkovChains
{
    public class TennisGameSolverTest
    {
        private Dictionary<int, double> _standardGameExpected =
            new Dictionary<int, double>{
                { 4, 1.0/8.0 },
                { 3, 1.0/4.0 },
                { 2, 5.0/8.0 }
            };

        private Dictionary<int, double> _tiebreakGameExpected =
            new Dictionary<int, double>{
                { 7, 2.0/128.0 },
                { 6, 7.0/128.0 },
                { 5, 14.0/128.0 },
                { 4, 21.0/128.0 },
                { 3, 105.0/512.0 },
                { 2, 231.0/512.0 }
            };

        private Dictionary<int, double> _setExpected =
            new Dictionary<int, double>
            {
                { 6, 2.0/64.0 },
                { 5, 6.0/64.0 },
                { 4, 21.0/128.0 },
                { 3, 56.0/256.0 },
                { 2, 189.0/512.0 },
                { 1, 63/512.0 }
            };

        private Dictionary<int, double> _matchExpected =
            new Dictionary<int, double>
            {
                { 2, 0.5 },
                { 1, 0.5 }
            };

        [Test]
        public void TestCalculateOddsOfWinningGameByXPoints()
        {
            var tennisGameSolver = new TennisGameSolver();
            var standardGameActual = tennisGameSolver
                .CalculateOddsOfWinningStandardGame();
            var tiebreakGameActual = tennisGameSolver
                .CalculateOddsOfWinningTiebreakGame();
            var setActual = tennisGameSolver
                .CalculateOddsOfWinningSet();
            var matchActual = tennisGameSolver
                .CalculateOddsOfWinningMatch();
            this.CompareOutcomes(standardGameActual, _standardGameExpected);
            this.CompareOutcomes(tiebreakGameActual, _tiebreakGameExpected);
            this.CompareOutcomes(setActual, _setExpected);
            this.CompareOutcomes(matchActual, _matchExpected);
        }

        private void CompareOutcomes(
            Dictionary<int, double> actual,
            Dictionary<int, double> expected
        )
        {
            Assert.AreEqual(expected.Keys.Count, actual.Keys.Count);
            foreach (var key in expected.Keys)
            {
                Assert.IsTrue(actual.ContainsKey(key));
                Assert.AreEqual(expected[key], actual[key]);
            }
        }
    }
}
