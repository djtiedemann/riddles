using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Probability;

namespace Riddles.Tests.Probability
{
    public class DistinctSeedSweetSixteenCalculatorTest
    {
        private double Epsilon = 0.00000001;
        private double TinyEpsilon = 0.1E-15;

        private Dictionary<int, (DistinctSeedSweetSixteenCalculator.WinningLikelihoodModel, int[], double[])> TestCaseDictionary =
            new Dictionary<int, (DistinctSeedSweetSixteenCalculator.WinningLikelihoodModel, int[], double[])>
            {
                { 1, (
                    DistinctSeedSweetSixteenCalculator.WinningLikelihoodModel.EqualOdds,
                    new int[] { 1, 16, 8, 9},
                    new double[] {0.25, 0.25, 0.25, 0.25 }
                ) },
                { 2, (
                    DistinctSeedSweetSixteenCalculator.WinningLikelihoodModel.EqualOdds,
                    new int[] { 4, 13, 5, 12},
                    new double[] {0.25, 0.25, 0.25, 0.25 }
                ) },
                { 3, (
                    DistinctSeedSweetSixteenCalculator.WinningLikelihoodModel.EqualOdds,
                    new int[] { 3, 14, 6, 11},
                    new double[] {0.25, 0.25, 0.25, 0.25 }
                ) },
                { 4, (
                    DistinctSeedSweetSixteenCalculator.WinningLikelihoodModel.EqualOdds,
                    new int[] { 2, 15, 7, 10},
                    new double[] {0.25, 0.25, 0.25, 0.25 }
                ) },
                { 5, (
                    DistinctSeedSweetSixteenCalculator.WinningLikelihoodModel.WeightedOdds,
                    new int[] { 1, 16, 8, 9},
                    new double[] { 0.742678945, 0.0012570550000000013, 0.144696175, 0.11136782499999998 }
                ) },
                { 6, (
                    DistinctSeedSweetSixteenCalculator.WinningLikelihoodModel.WeightedOdds,
                    new int[] { 4, 13, 5, 12},
                    new double[] { 0.474325783, 0.060522216999999982, 0.38544972099999997, 0.079702279 }
                ) },
                { 7, (
                    DistinctSeedSweetSixteenCalculator.WinningLikelihoodModel.WeightedOdds,
                    new int[] { 3, 14, 6, 11},
                    new double[] { 0.56463932499999991, 0.039904674999999994, 0.299736115, 0.095719884999999977 }
                ) },
                { 8, (
                    DistinctSeedSweetSixteenCalculator.WinningLikelihoodModel.WeightedOdds,
                    new int[] { 2, 15, 7, 10},
                    new double[] { 0.654665371, 0.019574628999999986, 0.21890994099999994, 0.10685005899999998 }
                ) },
            };

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        public void TestCalculateOddsOfTeamEachAdvancing(int testCaseId)
        {
            var (likelihoodModel, seeds, expectedOdds) = this.TestCaseDictionary[testCaseId];
            var distinctSeedSweetSixteenCalculator = new DistinctSeedSweetSixteenCalculator();
            var actualOdds = distinctSeedSweetSixteenCalculator.CalculateOddsOfTeamEachAdvancing(
                seeds,
                likelihoodModel
            );
            Assert.AreEqual(expectedOdds.Length, actualOdds.Length);
            var cumulativeProbability = 0.0;
            for(int i=0; i<seeds.Length; i++)
            {
                Assert.LessOrEqual(Math.Abs(expectedOdds[i] - actualOdds[i]), this.Epsilon);
                cumulativeProbability += actualOdds[i];
            }
            Assert.LessOrEqual(Math.Abs(cumulativeProbability - 1), this.Epsilon);
        }

        [TestCase(DistinctSeedSweetSixteenCalculator.WinningLikelihoodModel.EqualOdds, (double)81/1_048_576)]

        [TestCase(DistinctSeedSweetSixteenCalculator.WinningLikelihoodModel.WeightedOdds, 8.529753572810109E-10)]
        public void TestCalculateOddsOfDistinctSeedsSurvivingBracket(
            DistinctSeedSweetSixteenCalculator.WinningLikelihoodModel likelihoodModel,
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
