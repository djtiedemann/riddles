using NUnit.Framework;
using Riddles.Probability;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Probability
{
    public class IterativeCoinFlipperProbabilityCalculatorTest
    {
        double epsilon = 0.000000001;

        private Dictionary<int, Dictionary<char, double>> _testCaseDictionary =
            new Dictionary<int, Dictionary<char, double>>
            {
                { 1, new Dictionary<char, double> {
                    { 'H', 0.5 },
                    { 'T', 0.5 }
                } }
            };

        private Dictionary<int, Dictionary<string, double>> _outputDictionary =
            new Dictionary<int, Dictionary<string, double>>
            {
                { 1, new Dictionary<string, double>{ 
                    { "01112223", Math.Pow(3, 6)*5.0*7.0 / Math.Pow(2, 19) } 
                } },
            };

        private Dictionary<int, Dictionary<string, double>> _outputTupleDictionary =
            new Dictionary<int, Dictionary<string, double>>
            {
                { 1, new Dictionary<string, double>{
                    { "4310", 0.17503738403320313 },
                    { "3311", 0.070896148681640625 }
                } },
            };

        [TestCase(1, 3, 8)]
        public void TestCalculateOddsOfGettingExactOutcome(
            int testCaseId,
            int numTrials,
            int numIterations
        )
        {
            var iterativeCoinFlipperProbabilityCalculator =
                new IterativeCoinFlipperProbabilityCalculator();
            var outcomes = iterativeCoinFlipperProbabilityCalculator
                .CalculateOddsOfGettingExactOutcome(
                    this._testCaseDictionary[testCaseId],
                    numTrials,
                    numIterations
                );
            var expectedOutcomes = _outputDictionary[testCaseId];
            foreach(var expectedOutcome in expectedOutcomes)
            {
                Assert.LessOrEqual(
                    Math.Abs(expectedOutcome.Value - outcomes[expectedOutcome.Key])
                    , epsilon
                );
            }
        }

        [TestCase(1, 3, 8)]
        public void TestFindMostLikelyCombinationOfFlips(
            int testCaseId,
            int numTrials,
            int numIterations
        )
        {
            var iterativeCoinFlipperProbabilityCalculator =
                new IterativeCoinFlipperProbabilityCalculator();
            var outcomes = iterativeCoinFlipperProbabilityCalculator
                .CalculateOrderedTupleProbabilities(
                    this._testCaseDictionary[testCaseId],
                    numTrials,
                    numIterations
                );

            var expectedOutcomes = _outputTupleDictionary[testCaseId];
            foreach (var expectedOutcome in expectedOutcomes)
            {
                Assert.LessOrEqual(
                    Math.Abs(expectedOutcome.Value - outcomes[expectedOutcome.Key])
                    , epsilon
                );
            }
        }
    }
}
