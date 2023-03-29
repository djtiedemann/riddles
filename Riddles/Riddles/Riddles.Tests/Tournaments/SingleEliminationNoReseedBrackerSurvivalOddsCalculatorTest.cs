using NUnit.Framework;
using Riddles.Probability;
using Riddles.Tournaments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Tests.Tournaments
{
    public class SingleEliminationNoReseedBrackerSurvivalOddsCalculatorTest
    {
        private double Epsilon = 0.00000001;

        private Dictionary<int, (SingleEliminationNoReseedBrackerSurvivalOddsCalculator.WinningLikelihoodModel, int[], double[])> TestCaseDictionary =
            new Dictionary<int, (SingleEliminationNoReseedBrackerSurvivalOddsCalculator.WinningLikelihoodModel, int[], double[])>
            {
                { 1, (
                    SingleEliminationNoReseedBrackerSurvivalOddsCalculator.WinningLikelihoodModel.EqualOdds,
                    new int[] { 1, 16, 8, 9},
                    new double[] {0.25, 0.25, 0.25, 0.25 }
                ) },
                { 2, (
                    SingleEliminationNoReseedBrackerSurvivalOddsCalculator.WinningLikelihoodModel.EqualOdds,
                    new int[] { 4, 13, 5, 12},
                    new double[] {0.25, 0.25, 0.25, 0.25 }
                ) },
                { 3, (
                    SingleEliminationNoReseedBrackerSurvivalOddsCalculator.WinningLikelihoodModel.EqualOdds,
                    new int[] { 3, 14, 6, 11},
                    new double[] {0.25, 0.25, 0.25, 0.25 }
                ) },
                { 4, (
                    SingleEliminationNoReseedBrackerSurvivalOddsCalculator.WinningLikelihoodModel.EqualOdds,
                    new int[] { 2, 15, 7, 10},
                    new double[] {0.25, 0.25, 0.25, 0.25 }
                ) },
                { 5, (
                    SingleEliminationNoReseedBrackerSurvivalOddsCalculator.WinningLikelihoodModel.WeightedOdds,
                    new int[] { 1, 16, 8, 9},
                    new double[] { 0.742678945, 0.0012570550000000013, 0.144696175, 0.11136782499999998 }
                ) },
                { 6, (
                    SingleEliminationNoReseedBrackerSurvivalOddsCalculator.WinningLikelihoodModel.WeightedOdds,
                    new int[] { 4, 13, 5, 12},
                    new double[] { 0.474325783, 0.060522216999999982, 0.38544972099999997, 0.079702279 }
                ) },
                { 7, (
                    SingleEliminationNoReseedBrackerSurvivalOddsCalculator.WinningLikelihoodModel.WeightedOdds,
                    new int[] { 3, 14, 6, 11},
                    new double[] { 0.56463932499999991, 0.039904674999999994, 0.299736115, 0.095719884999999977 }
                ) },
                { 8, (
                    SingleEliminationNoReseedBrackerSurvivalOddsCalculator.WinningLikelihoodModel.WeightedOdds,
                    new int[] { 2, 15, 7, 10},
                    new double[] { 0.654665371, 0.019574628999999986, 0.21890994099999994, 0.10685005899999998 }
                ) },
                { 9, (
                    SingleEliminationNoReseedBrackerSurvivalOddsCalculator.WinningLikelihoodModel.WeightedOdds,
                    new int[] { 1, 2, 3, 4, 5, 6, 7, 8 },
                    new double[] { 
                        0.2642096966605828,
                        0.20633362732316921,
                        0.1591677212675254,
                        0.12077636935295818,
                        0.091652024338911439,
                        0.06955308421469919,
                        0.051460123002606217,
                        0.036847353839547577
                    }
                ) },
                { 10, (
                    SingleEliminationNoReseedBrackerSurvivalOddsCalculator.WinningLikelihoodModel.WeightedOdds,
                    new int[] { 1, 2, 3, 4, 5, 6, 7 },
                    new double[] {
                        0.3614359735438889,
                        0.19455251178300281,
                        0.14949019396398325,
                        0.10457189766039186,
                        0.0784117342457192,
                        0.064302527715000724,
                        0.047235161088013226
                    }
                ) },
                { 11, (
                    SingleEliminationNoReseedBrackerSurvivalOddsCalculator.WinningLikelihoodModel.WeightedOdds,
                    new int[] { 1, 2, 3, 4, 5, 6 },
                    new double[] {
                        0.34512090418781505,
                        0.29256016809474106,
                        0.13366889202274698,
                        0.098896431695945,
                        0.07384827011624,
                        0.055905333882511996,
                    }
                ) },
                { 12, (
                    SingleEliminationNoReseedBrackerSurvivalOddsCalculator.WinningLikelihoodModel.WeightedOdds,
                    new int[] { 1, 2, 3, 4, 5 },
                    new double[] {
                        0.336949750921,
                        0.272280149947,
                        0.22315340905300002,
                        0.096053961263000018,
                        0.071562728816,
                    }
                ) },
                { 13, (
                    SingleEliminationNoReseedBrackerSurvivalOddsCalculator.WinningLikelihoodModel.EqualOdds,
                    new int[] { 1, 2, 3, 4, 5, 6, 7, 8 },
                    new double[] {
                        1.0/8.0,
                        1.0/8.0,
                        1.0/8.0,
                        1.0/8.0,
                        1.0/8.0,
                        1.0/8.0,
                        1.0/8.0,
                        1.0/8.0,
                    }
                ) },
                { 14, (
                    SingleEliminationNoReseedBrackerSurvivalOddsCalculator.WinningLikelihoodModel.EqualOdds,
                    new int[] { 1, 2, 3, 4, 5, 6, 7 },
                    new double[] {
                        1.0/4.0,
                        1.0/8.0,
                        1.0/8.0,
                        1.0/8.0,
                        1.0/8.0,
                        1.0/8.0,
                        1.0/8.0,
                    }
                ) },
                { 15, (
                    SingleEliminationNoReseedBrackerSurvivalOddsCalculator.WinningLikelihoodModel.EqualOdds,
                    new int[] { 1, 2, 3, 4, 5, 6 },
                    new double[] {
                        1.0/4.0,
                        1.0/4.0,
                        1.0/8.0,
                        1.0/8.0,
                        1.0/8.0,
                        1.0/8.0,
                    }
                ) },
                { 16, (
                    SingleEliminationNoReseedBrackerSurvivalOddsCalculator.WinningLikelihoodModel.EqualOdds,
                    new int[] { 1, 2, 3, 4, 5 },
                    new double[] {
                        1.0/4.0,
                        1.0/4.0,
                        1.0/4.0,
                        1.0/8.0,
                        1.0/8.0,
                    }
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
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        [TestCase(13)]
        [TestCase(14)]
        [TestCase(15)]
        [TestCase(16)]
        public void TestCalculateOddsOfTeamEachAdvancing(int testCaseId)
        {
            var (likelihoodModel, seeds, expectedOdds) = this.TestCaseDictionary[testCaseId];
            var singleEliminationNoReseedBrackerSurvivalOddsCalculator = new SingleEliminationNoReseedBrackerSurvivalOddsCalculator();
            var actualOdds = singleEliminationNoReseedBrackerSurvivalOddsCalculator.CalculateOddsOfTeamEachAdvancing(
                seeds.ToList(),
                likelihoodModel
            );
            Assert.AreEqual(expectedOdds.Length, actualOdds.Count);
            var cumulativeProbability = 0.0;
            for (int i = 0; i < seeds.Length; i++)
            {
                Assert.LessOrEqual(Math.Abs(expectedOdds[i] - actualOdds[seeds[i]]), this.Epsilon);
                cumulativeProbability += actualOdds[seeds[i]];
            }
            Assert.LessOrEqual(Math.Abs(cumulativeProbability - 1), this.Epsilon);
        }

        // test internals. may be helpful for debugging
        /*[TestCase(0, 0, 0, 0)]
        [TestCase(1, 0, 1, 1)]
        [TestCase(2, 0, 2, 2)]
        [TestCase(3, 0, 3, 3)]
        [TestCase(4, 0, 4, 4)]
        [TestCase(5, 0, 5, 5)]
        [TestCase(6, 0, 6, 6)]
        [TestCase(7, 0, 7, 7)]
        [TestCase(0, 1, 1, 1)]
        [TestCase(1, 1, 0, 0)]
        [TestCase(2, 1, 3, 3)]
        [TestCase(3, 1, 2, 2)]
        [TestCase(4, 1, 5, 5)]
        [TestCase(5, 1, 4, 4)]
        [TestCase(6, 1, 7, 7)]
        [TestCase(7, 1, 6, 6)]
        [TestCase(0, 2, 2, 3)]
        [TestCase(1, 2, 2, 3)]
        [TestCase(2, 2, 0, 1)]
        [TestCase(3, 2, 0, 1)]
        [TestCase(4, 2, 6, 7)]
        [TestCase(5, 2, 6, 7)]
        [TestCase(6, 2, 4, 5)]
        [TestCase(7, 2, 4, 5)]
        [TestCase(0, 3, 4, 7)]
        [TestCase(1, 3, 4, 7)]
        [TestCase(2, 3, 4, 7)]
        [TestCase(3, 3, 4, 7)]
        [TestCase(4, 3, 0, 3)]
        [TestCase(5, 3, 0, 3)]
        [TestCase(6, 3, 0, 3)]
        [TestCase(7, 3, 0, 3)]
        public void TestMinMax(int seed, int round, int expectedMin, int expectedMax)
        {
            var singleEliminationNoReseedBrackerSurvivalOddsCalculator
                = new SingleEliminationNoReseedBrackerSurvivalOddsCalculator();
            var (actualMin, actualMax) = singleEliminationNoReseedBrackerSurvivalOddsCalculator
                .GenerateCompetitionRangeInclusive(seed, round);
            Assert.AreEqual(expectedMin, actualMin);
            Assert.AreEqual(expectedMax, actualMax);
        }*/
    }
}
