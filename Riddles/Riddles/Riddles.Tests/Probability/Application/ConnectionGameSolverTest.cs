using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Probability.Application;

namespace Riddles.Tests.Probability.Application
{
    public class ConnectionGameSolverTest
    {
        private double epsilon = 0.00000000001;

        [TestCase(4, 4, 48.0/455.0)]
        public void TestCalculateOddsOfGettingOneAwayOnFirstTry(
            int numGroups, int sizeOfGroup, double odds)
        {
            var connectionGameSolver = new ConnectionGameSolver();
            var actualOdds = connectionGameSolver
                .CalculateOddsOfGettingOneAwayOnFirstTry(
                    numGroups,
                    sizeOfGroup
                );
            Assert.LessOrEqual(Math.Abs(actualOdds - odds), epsilon);

        }
    }
}
