using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Probability.Application;
using NUnit.Framework;

namespace Riddles.Tests.Probability.Application
{
    public class TennisMatchProbabilitySolverTest
    {
        private double epsilon = 0.00000000001;

        [Test]
        public void TestCalculateProbabilityOfWinningMatchAndLosingMostGames()
        {
            var tennisMatchProbabilitySolver = 
                new TennisMatchProbabilitySolver();
            var actual = tennisMatchProbabilitySolver
                .CalculateProbabilityOfWinningMatchAndLosingMostGames();
            var expected = 0.062672336416418381;

            Assert.LessOrEqual(Math.Abs(actual - expected), epsilon);
        }
    }
}
