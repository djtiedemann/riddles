using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Probability.Application;

namespace Riddles.Tests.Probability.Application
{
    public class XkcdPoisonArrowProbabilityCalculatorTest
    {
        private double epsilon = 0.00000000001;

        private Dictionary<int, (int[], int, int, int, int, int)> _testCaseDictionary
            = new Dictionary<int, (int[], int, int, int, int, int)>
            {
                {
                    1,
                    (
                        new int[] { 4, 6, 8, 10, 12, 20},
                        4,
                        10,
                        5,
                        2,
                        3
                    )
                }
            };

        [TestCase(10, 5, 2, 2.0/9.0)]
        public void TestCalculateOddsOfDrawingAllNonPoisonedArrows(
            int numArrows,
            int numPoisonedArrows,
            int numDraws,
            double expected)
        {
            var xkcdPoisonArrowProbabilityCalculator =
                new XkcdPoisonArrowProbabilityCalculator();

            var actual = xkcdPoisonArrowProbabilityCalculator
                .CalculateOddsOfDrawingAllNonPoisonedArrows(
                   numArrows,
                   numPoisonedArrows,
                   numDraws
                );

            Assert.LessOrEqual(Math.Abs(expected - actual), epsilon);
        }

        [TestCase(1)]
        public void TestFindSkillChecksThatSumToSameOddsAsAvoidingPoisonArrows(
            int testCaseId
        )
        {
            var testCase = this._testCaseDictionary[testCaseId];
            var xkcdPoisonArrowProbabilityCalculator =
                new XkcdPoisonArrowProbabilityCalculator();

            var skillChecksOfSameDifficulty =
                xkcdPoisonArrowProbabilityCalculator
                    .FindSkillChecksThatSumToSameOddsAsAvoidingPoisonArrows(
                        testCase.Item1,
                        testCase.Item2,
                        testCase.Item3,
                        testCase.Item4,
                        testCase.Item5
                    );

            Assert.AreEqual(skillChecksOfSameDifficulty.Count, testCase.Item6);
        }
    }
}
