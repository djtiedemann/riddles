using NUnit.Framework;
using Riddles.Probability.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Tests.Probability.Core
{
    public class IndependentTrialOutcomeDistributionCalculatorTest
    {
        double epsilon = 0.000000001;

        private Dictionary<int, Dictionary<char, double>> testCaseDictionary = new Dictionary<int, Dictionary<char, double>> {
            { 1, new Dictionary<char, double> { {'H', 0.5}, { 'T', 0.5 } } },
            { 2, new Dictionary<char, double> { {'H', 0.75}, { 'T', 0.25 } } },
            { 3, new Dictionary<char, double> { { '1', 0.2 }, { '2', 0.5 }, { '3', 0.3 } } }
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
            },
            { 3, new Dictionary<string, double> {
                    { "111", 1.0/125.0 }, { "112", 1.0/50.0 }, { "113", 3.0/250.0 },
                    { "121", 1.0/50.0 },{ "122", 1.0/20.0 }, { "123", 3.0/100.0 },
                    { "131", 3.0/250.0 }, { "132", 3.0/100.0 }, { "133", 9.0/500.0 },
                    { "211", 1.0/50.0 }, { "212", 1.0/20.0 }, { "213", 3.0/100.0 },
                    { "221", 1.0/20.0 },{ "222", 1.0/8.0 }, { "223", 3.0/40.0 },
                    { "231", 3.0/100.0 }, { "232", 3.0/40.0 }, { "233", 9.0/200.0 },
                    { "311", 3.0/250.0 }, { "312", 3.0/100.0 }, { "313", 9.0/500.0 },
                    { "321", 3.0/100.0 },{ "322", 3.0/40.0 }, { "323", 9.0/200.0 },
                    { "331", 9.0/500.0 }, { "332", 9.0/200.0 }, { "333", 27.0/1000.0 }
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
            { 3, new Dictionary<string, double>
                {
                    { "111", 1.0/125.0 }, { "112", 3.0/50.0 }, { "113", 9.0/250.0 },
                    { "122", 3.0/20.0 }, { "123", 18.0/100.0 }, { "133", 27.0/500.0 },
                    { "222", 1.0/8.0 }, { "223", 9.0/40.0 }, { "233", 27.0/200.0 },
                    { "333", 27.0/1000.0 }
                }
            }
        };

        [TestCase(1, 1, 3)]
        [TestCase(2, 2, 3)]
        [TestCase(3, 3, 3)]
        public void TestCalculateOrderDependentOutcomeProbabilities(int inputId, int outputId, int numTrials)
        {
            var outcomeDistributionCalculator = new IndependentTrialOutcomeDistributionCalculator();
            var probabilities = this.testCaseDictionary[inputId];
            var orderDependentProbabilities = this.orderDependentResultDictionary[outputId];
            var outcomeDistribution = outcomeDistributionCalculator.CalculateOrderDependentOutcomeProbabilities(numTrials, probabilities);
            Assert.AreEqual(outcomeDistribution.Count, orderDependentProbabilities.Keys.Count);
            Assert.LessOrEqual(Math.Abs(1.0 - probabilities.Values.Sum()), 0);
            foreach (var outcome in outcomeDistribution)
            {
                Assert.LessOrEqual(Math.Abs(outcome.Item2 - orderDependentProbabilities[outcome.Item1]), epsilon);
            }
            Assert.LessOrEqual(Math.Abs(1.0 - orderDependentProbabilities.Values.Sum()), epsilon);
        }

        [TestCase(1, 1, 3)]
        [TestCase(2, 2, 3)]
        [TestCase(3, 3, 3)]
        public void TestCalculateOrderIndependentOutcomeProbabilities(int inputId, int outputId, int numTrials)
        {
            var outcomeDistributionCalculator = new IndependentTrialOutcomeDistributionCalculator();
            var probabilities = this.testCaseDictionary[inputId];
            var orderIndependentProbabilities = this.orderIndependentResultDictionary[outputId];
            var outcomeDistribution = outcomeDistributionCalculator.CalculateOrderIndependentOutcomeProbabilities(numTrials, probabilities);
            Assert.AreEqual(outcomeDistribution.Count, orderIndependentProbabilities.Keys.Count);
            Assert.LessOrEqual(Math.Abs(1.0 - probabilities.Values.Sum()), 0);
            foreach (var outcome in outcomeDistribution)
            {
                Assert.LessOrEqual(Math.Abs(outcome.Item2 - orderIndependentProbabilities[outcome.Item1]), epsilon);
            }
            Assert.LessOrEqual(Math.Abs(1.0 - orderIndependentProbabilities.Values.Sum()), epsilon);
        }
    }
}
