using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Probability;
using Riddles.Tournaments;

namespace Riddles.Tests.Probability
{
    public class DistinctSeedSweetSixteenCalculatorTest
    {
        private double TinyEpsilon = 0.1E-15;

        [TestCase(SingleEliminationNoReseedBrackerSurvivalOddsCalculator.WinningLikelihoodModel.EqualOdds, (double)81/1_048_576)]

        [TestCase(SingleEliminationNoReseedBrackerSurvivalOddsCalculator.WinningLikelihoodModel.WeightedOdds, 8.529753572810109E-10)]
        public void TestCalculateOddsOfDistinctSeedsSurvivingBracket(
            SingleEliminationNoReseedBrackerSurvivalOddsCalculator.WinningLikelihoodModel likelihoodModel,
            double expectedOddsOfDistinctSeedsSurviving
        )
        {
            var brackets = new List<int[]>
            {
                new int[] { 1, 16, 8, 9},
                new int[] { 4, 13, 5, 12},
                new int[] { 3, 14, 6, 11},
                new int[] { 2, 15, 7, 10}
            };
            var distinctSeedSweetSixteenCalculator = new DistinctSeedSweetSixteenCalculator();
            var actualProbabilityOfDistinctSeedsSurviving =
                distinctSeedSweetSixteenCalculator.CalculateOddsOfDistinctSeedsSurvivingBracket(
                    brackets,
                    likelihoodModel
            );
            Assert.LessOrEqual(Math.Abs(actualProbabilityOfDistinctSeedsSurviving - expectedOddsOfDistinctSeedsSurviving), 
                this.TinyEpsilon);
        }
    }
}
