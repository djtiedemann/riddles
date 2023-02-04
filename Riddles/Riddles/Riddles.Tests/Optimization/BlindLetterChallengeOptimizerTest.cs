using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Optimization;

namespace Riddles.Tests.Optimization
{
    public class BlindLetterChallengeOptimizerTest
    {
        private double Epsilon = 0.00000001;

        [TestCase(26, 2, Strategy.EvenSpacing, 0.76)]
        [TestCase(26, 3, Strategy.EvenSpacing, 0.53410256410256407)]
        [TestCase(26, 4, Strategy.EvenSpacing, 0.36256967670011148)]
        [TestCase(26, 5, Strategy.EvenSpacing, 0.24141177662916793)] // - takes 53 seconds, commented out
        public void TestGetPercentSuccessfulAttempts(int numLetters, int numSlots, Strategy strategy, double expectedPercentSuccessfulPermutations)
        {
            var blindLetterChallengeOptimizer = new BlindLetterChallengeOptimizer();
            var percentSuccessfulPermutations = blindLetterChallengeOptimizer.GetPercentSuccessfulAttempts(numLetters, numSlots, strategy);
            Assert.LessOrEqual(Math.Abs(expectedPercentSuccessfulPermutations - percentSuccessfulPermutations), Epsilon); 
        }

        [TestCase(new int[] { 19, 15, 3, 2, 1 }, 26, false)]
        [TestCase(new int[] { 19, 2, 18, 22, 1 }, 26, false)]
        [TestCase(new int[] { 1, 2, 3, 4, 5 }, 26, true)]
        [TestCase(new int[] { 5, 4, 3, 2, 1 }, 26, false)]
        [TestCase(new int[] { 22, 23, 24, 25, 26 }, 26, false)]
        [TestCase(new int[] { 26, 25, 24, 23, 22 }, 26, true)]
        [TestCase(new int[] { 13, 2, 6, 25, 4 }, 26, false)]
        [TestCase(new int[] { 13, 2, 6, 17, 26 }, 26, true)]
        [TestCase(new int[] { 8, 18, 7, 21, 14 }, 26, true)]
        public void TestIsSuccessfulAttempt(int[] attempt, int numLetters, bool expectedResult)
        {
            var blindLetterChallengeOptimizer = new BlindLetterChallengeOptimizer();
            var isSuccess = blindLetterChallengeOptimizer.IsSuccessfulAttemptEvenSpacingStrategy(attempt, numLetters);
            Assert.AreEqual(expectedResult, isSuccess);
        }
    }
}
