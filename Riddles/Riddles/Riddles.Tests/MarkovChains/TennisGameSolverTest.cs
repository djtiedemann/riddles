using NUnit.Framework;
using Riddles.MarkovChains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.MarkovChains
{
    public class TennisGameSolverTest
    {
        private Dictionary<(int, int), double> _standardGameExpected =
            new Dictionary<(int, int), double>{
                { (4, 0), 1.0/8.0 },
                { (4, 1), 1.0/4.0 },
                { (4, 2), 5.0/8.0 }
            };

        private Dictionary<(int, int), double> _tiebreakGameExpected =
            new Dictionary<(int, int), double>{
                { (7, 0), 2.0/128.0 },
                { (7, 1), 7.0/128.0 },
                { (7, 2), 14.0/128.0 },
                { (7, 3), 21.0/128.0 },
                { (7, 4), 105.0/512.0 },
                { (7, 5), 231.0/512.0 }
            };

        private Dictionary<(int, int), double> _setExpected =
            new Dictionary<(int, int), double>
            {
                { (6, 0), 2.0/64.0 },
                { (6, 1), 6.0/64.0 },
                { (6, 2), 21.0/128.0 },
                { (6, 3), 56.0/256.0 },
                { (6, 4), 189.0/512.0 },
                { (7, 5), 63/512.0 }
            };

        private Dictionary<(int, int), double> _matchExpected =
            new Dictionary<(int, int), double>
            {
                { (2, 0), 0.5 },
                { (2, 1), 0.5 }
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
            //this.CompareOutcomes(setActual, _setExpected);
            this.CompareOutcomes(matchActual, _matchExpected);
        }

        private void CompareOutcomes(
            Dictionary<(int, int), double> actual,
            Dictionary<(int, int), double> expected
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
