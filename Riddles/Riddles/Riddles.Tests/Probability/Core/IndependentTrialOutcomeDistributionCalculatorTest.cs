using NUnit.Framework;
using Riddles.Probability.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Probability.Core
{
    public class IndependentTrialOutcomeDistributionCalculatorTest
    {
        double epsilon = 0.000000001;

        private Dictionary<int, Dictionary<char, double>> testCaseDictionary = new Dictionary<int, Dictionary<char, double>> {
            { 1, new Dictionary<char, double> { {'H', 0.5}, { 'T', 0.5 } } },
            { 2, new Dictionary<char, double> { {'H', 0.75}, { 'T', 0.25 } } }
        };
        private Dictionary<int, Dictionary<string, double>> orderDependentResultDictionary = new Dictionary<int, Dictionary<string, double>> {
            { 1, new Dictionary<string, double> { 
                    {"HHH", 1.0/8.0 }, { "HHT", 1.0 / 8.0 }, { "HTH", 1.0 / 8.0 }, { "HTT", 1.0 / 8.0 },
                    {"THH", 1.0/8.0 }, { "THT", 1.0 / 8.0 }, { "TTH", 1.0 / 8.0 }, { "TTT", 1.0 / 8.0 }
                } 
            },
            { 2, new Dictionary<string, double> {
                    {"HHH", 27.0/64.0 }, { "HHT", 9.0 / 64.0 }, { "HTH", 9.0 / 64.0 }, { "HTT", 3.0 / 64.0 },
                    {"THH", 9.0/64.0 }, { "THT", 3.0 / 64.0 }, { "TTH", 3.0 / 64.0 }, { "TTT", 1.0 / 64.0 }
                }
            }
        };
        private Dictionary<int, Dictionary<string, double>> orderIndependentResultDictionary = new Dictionary<int, Dictionary<string, double>>
        {
            { 1, new Dictionary<string, double> {
                    {"HHH", 1.0/8.0 }, { "HHT", 3.0 / 8.0 }, { "HTT", 3.0 / 8.0 }, { "TTT", 1.0 / 8.0 },
                }
            },
            { 2, new Dictionary<string, double> {
                    {"HHH", 27.0/64.0 }, { "HHT", 27.0 / 64.0 }, { "HTT", 9.0 / 64.0 }, { "TTT", 1.0 / 64.0 },
                }
            },
        };

        [TestCase(1, 1, 3)]
        [TestCase(2, 2, 3)]
        public void TestCalculateOrderDependentOutcomeProbabilities(int inputId, int outputId, int numTrials)
        {
            var outcomeDistributionCalculator = new IndependentTrialOutcomeDistributionCalculator();
            var probabilities = this.testCaseDictionary[inputId];
            var orderDependentProbabilities = this.orderDependentResultDictionary[outputId];
            var outcomeDistribution = outcomeDistributionCalculator.CalculateOrderDependentOutcomeProbabilities(numTrials, probabilities);
            Assert.AreEqual(outcomeDistribution.Count, orderDependentProbabilities.Keys.Count);
            foreach(var outcome in outcomeDistribution)
            {
                Assert.LessOrEqual(Math.Abs(outcome.Item2 - orderDependentProbabilities[outcome.Item1]), epsilon);
            }
        }

        [TestCase(1, 1, 3)]
        [TestCase(2, 2, 3)]
        public void TestCalculateOrderIndependentOutcomeProbabilities(int inputId, int outputId, int numTrials)
        {
            var outcomeDistributionCalculator = new IndependentTrialOutcomeDistributionCalculator();
            var probabilities = this.testCaseDictionary[inputId];
            var orderIndependentProbabilities = this.orderIndependentResultDictionary[outputId];
            var outcomeDistribution = outcomeDistributionCalculator.CalculateOrderIndependentOutcomeProbabilities(numTrials, probabilities);
            Assert.AreEqual(outcomeDistribution.Count, orderIndependentProbabilities.Keys.Count);
            foreach (var outcome in outcomeDistribution)
            {
                Assert.LessOrEqual(Math.Abs(outcome.Item2 - orderIndependentProbabilities[outcome.Item1]), epsilon);
            }
        }
    }
}
