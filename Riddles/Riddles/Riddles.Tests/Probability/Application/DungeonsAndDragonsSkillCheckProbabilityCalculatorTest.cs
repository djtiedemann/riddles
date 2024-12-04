using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Probability.Application;

namespace Riddles.Tests.Probability.Application
{
    public class DungeonsAndDragonsSkillCheckProbabilityCalculatorTest
    {
        private double _epsilon = 0.00000000001;

        private Dictionary<int, (int[], int, double)> _testCaseDictionary
            = new Dictionary<int, (int[], int, double)>
            {
                { 1, (new int[] { 6, 6, 6, 4 }, 16, 2.0/9.0) }
            };

        [TestCase(1)]
        public void TestCalculateOddsOfPassingCheckAtTarget(int testCaseId)
        {
            var dungeonsAndDragonsSkillCheckProbabilityCalculator =
                new DungeonsAndDragonsSkillCheckProbabilityCalculator();

            var testCase = this._testCaseDictionary[testCaseId];

            var actual = dungeonsAndDragonsSkillCheckProbabilityCalculator
                .CalculateOddsOfPassingCheckAtTarget(
                    testCase.Item1,
                    testCase.Item2
                );

            Assert.LessOrEqual(
                Math.Abs(testCase.Item3 - actual),
                this._epsilon
            );
        }
    }
}
