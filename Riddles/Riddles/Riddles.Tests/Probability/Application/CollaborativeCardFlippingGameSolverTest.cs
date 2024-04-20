using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Riddles.Probability.Application;

namespace Riddles.Tests.Probability.Application
{
    internal class CollaborativeCardFlippingGameSolverTest
    {
        private double epsilon = 0.00000000001;

        [TestCase(1, 0.0)]
        [TestCase(2, 0.5)]
        [TestCase(3, 1.0/3.0)]
        [TestCase(4, 3.0/8.0)]
        [TestCase(5, 11.0/30.0)]
        [TestCase(52, 0.36787944117144217)]
        public void TestCalculateProbabilityOfWinning(int numCards, double expected)
        {
            var collaborativeCardFlippingGameSolver = new CollaborativeCardFlippingGameSolver();
            var actual = collaborativeCardFlippingGameSolver.CalculateProbabilityOfWinning(numCards);
            Assert.LessOrEqual(Math.Abs(actual - expected), epsilon);
        }

        //[Test]
        public void VerifyThroughSimulations()
        {
            int numWins = 0;
            var numTries = 1_000_000;
            for(int i=0; i< numTries; i++)
            {
                var random = new Random();
                var sequence1 = Enumerable.Range(0, 52).ToList();
                var sequence2 = Enumerable.Range(0, 52).ToList();
                sequence1 = sequence1.OrderBy(_ => random.Next()).ToList();
                sequence2 = sequence1.OrderBy(_ => random.Next()).ToList();
                bool won = true;
                for(int j=0; j<sequence1.Count(); j++)
                {
                    if (sequence1[j] == sequence2[j])
                    {
                        won = false;
                        break;
                    }
                }
                if (won)
                {
                    numWins++;
                }
            }
            var winRate = (double)numWins / numTries;
        }
    }
}
