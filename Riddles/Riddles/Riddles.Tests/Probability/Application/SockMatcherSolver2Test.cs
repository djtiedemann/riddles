using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Probability.Application;
using System.Linq;

namespace Riddles.Tests.Probability.Application
{
    public class SockMatcherSolver2Test
    {
        private double epsilon = 0.00000000001;

        [TestCase(1, 2, 1.0)]
        [TestCase(2, 3, 2.0/3.0)]
        [TestCase(3, 3, 2.0/5.0)]
        [TestCase(4, 4, 12.0/35.0)]
        [TestCase(5, 4, 2.0/7.0)]
        public void TestCalculateIdealNumberOfSocksToDraw(
            int numPairs,
            int expectedBestNumSocksToDraw,
            double expectedBestProbabilityOfWinning
        )
        {
            var sockMatcherSolver = new SockMatcherSolver2();
            var actual = sockMatcherSolver
                .CalculateIdealNumberOfSocksToDrawInternal(
                    numPairs
                );
            Assert.LessOrEqual(
                Math.Abs(actual.Select(x => x.Item2).Sum() - 1),
                epsilon
            );
            Assert.LessOrEqual(
                Math.Abs(actual.Select(x => x.Item2).Max() 
                    - expectedBestProbabilityOfWinning),
                epsilon
            );
            Assert.LessOrEqual(
                Math.Abs(actual.Single(x => x.Item1 
                    == expectedBestNumSocksToDraw).Item2
                    - expectedBestProbabilityOfWinning),
                epsilon
            );

            var bestValues = sockMatcherSolver
                .CalculateIdealNumberOfSocksToDraw(numPairs);
            Assert.LessOrEqual(
                Math.Abs(bestValues.Item2
                    - expectedBestProbabilityOfWinning),
                epsilon
            );
            Assert.AreEqual(
                bestValues.Item1,
                expectedBestNumSocksToDraw
            );
        }

        [TestCase(50, 11)]
        [TestCase(100, 15)]
        [TestCase(500, 33)]
        [TestCase(1000, 46)]
        [TestCase(5000, 101)]
        [TestCase(10000, 142)]
        [TestCase(100000, 448)]
        [TestCase(1000000, 1415)]
        [TestCase(10000000, 4473)]
        public void TestWithLargeValues(
            int numPairs, 
            int bestNumDraws
        )
        {
            var sockMatcherSolver = new SockMatcherSolver2();
            var bestValues = sockMatcherSolver
                .CalculateIdealNumberOfSocksToDraw(numPairs);
            Assert.AreEqual(bestNumDraws, bestValues.Item1);
        }
    }
}
