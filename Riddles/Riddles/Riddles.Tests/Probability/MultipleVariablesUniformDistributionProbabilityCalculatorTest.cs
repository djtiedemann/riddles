using NUnit.Framework;
using Riddles.Probability;
using Riddles.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Tests.Probability
{
    public class MultipleVariablesUniformDistributionProbabilityCalculatorTest
    {
        private double Epsilon = 0.00000001;
        private Dictionary<int, (List<List<(double, double)>>, double[])>
            _testCaseDictionary =
            new Dictionary<int, (List<List<(double, double)>>, double[])>
            {
                {1, (new List<List<(double, double)>>{
                        new List<(double, double)> {
                            (0.0, 1.0)
                        },new List<(double, double)> {
                            (0.25, 0.75)
                        }
                    },
                    new double[]{
                        0.5,
                        0.5
                    })
                },
                {2, (new List<List<(double, double)>>{
                        new List<(double, double)> {
                            (0.0, 0.25),
                            (0.75, 1.0)
                        },new List<(double, double)> {
                            (0.25, 0.75)
                        }
                    },
                    new double[]{
                        0.5,
                        0.5
                    })
                },
                {3, (new List<List<(double, double)>>{
                        new List<(double, double)> {
                            (0.0, 1.0)
                        },
                        new List<(double, double)> {
                            (0.0, 0.25),
                            (0.75, 1.0)
                        }
                    },
                    new double[]{
                        1.0/2.0,
                        1.0/2.0
                    })
                },
                {4, (new List<List<(double, double)>>{
                        new List<(double, double)> {
                            (0.0, 1.0)
                        },
                        new List<(double, double)> {
                            (0.0, 1.0)
                        },
                        new List<(double, double)> {
                            (0.0, 1.0)
                        }
                    },
                    new double[]{
                        1.0/3.0,
                        1.0/3.0,
                        1.0/3.0
                    })
                },
                {5, (new List<List<(double, double)>>{
                        new List<(double, double)> {
                            (0.5, 1.0)
                        },
                        new List<(double, double)> {
                            (0.0, 1.0)
                        }
                    },
                    new double[]{
                        3.0/4.0,
                        1.0/4.0
                    })
                },
                {6, (new List<List<(double, double)>>{
                        new List<(double, double)> {
                            (0.0, 0.5)
                        },
                        new List<(double, double)> {
                            (0.0, 1.0)
                        }
                    },
                    new double[]{
                        1.0/4.0,
                        3.0/4.0
                    })
                },
                {7, (new List<List<(double, double)>>{
                        new List<(double, double)> { 
                            (0.0, 1.0)
                        },
                        new List<(double, double)> {
                            (0.0, 0.25),
                            (0.75, 1.0)
                        },new List<(double, double)> {
                            (0.25, 0.75)
                        }
                    }, 
                    new double[]{ 
                        1.0/3.0,
                        1.0/3.0,
                        1.0/3.0
                    }) 
                }
            };

        [TestCase(1)] // two teams, equal probability
        [TestCase(2)] // two teams, equal probability
        [TestCase(3)] // two teams, equal probability
        [TestCase(4)] // three teams, equal probability
        [TestCase(5)] // two teams, skewed probability
        [TestCase(6)] // two teams, skewed probability
        //[TestCase(7)]
        public void TestCalculateProbabilityTeamHasHighestValue(int testCaseId)
        {
            var testCase = this._testCaseDictionary[testCaseId];
            var distribution = testCase.Item1;
            var expectedProbabilities = testCase.Item2;
            var multipleVariablesUniformDistributionProbabilityCalculator =
                new MultipleVariablesUniformDistributionProbabilityCalculator();
            var actualProbabilities =
                multipleVariablesUniformDistributionProbabilityCalculator
                    .CalculateProbabilityTeamHasHighestValue(
                        distribution
                    );
            Assert.AreEqual(
                expectedProbabilities.Length, 
                actualProbabilities.Length
            );
            Assert.LessOrEqual(
                Math.Abs(actualProbabilities.Sum() - 1),
                this.Epsilon
            );
            for(int i=0; i<expectedProbabilities.Length; i++)
            {
                Assert.LessOrEqual(
                    Math.Abs(expectedProbabilities[i] - actualProbabilities[i]),
                    this.Epsilon
                );
            }
        }
    }
}
