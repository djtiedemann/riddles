using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Probability.Application;

namespace Riddles.Tests.Probability.Application
{
    public class RedAndGreenBallInUrnSolverTest
    {
        private double epsilon = 0.00000000001;

        [TestCase(100, 2.0/3.0)]
        public void TestCalculateOddsOfSecondBallBeingRed(int numBalls, double expected)
        {
            var redAndGreenBallInUrnSolver = new RedAndGreenBallInUrnSolver();
            var actual =
                redAndGreenBallInUrnSolver.CalculateOddsOfSecondBallBeingRed(numBalls);
            Assert.LessOrEqual(Math.Abs(expected-actual), epsilon);
        }
    }
}
