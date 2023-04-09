using NUnit.Framework;
using Riddles.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.LinearAlgebra
{
    public class TransformationUtilTest
    {
        private double Epsilon = 0.00000001;
        private Dictionary<int, (double, List<(double, double)>, double)>
            _testCaseDictionary
                = new Dictionary<int, (double, List<(double, double)>, double)>
                {
                    { 1, (0.75, 
                        new List<(double, double)> { (0.0, 0.25), (0.75, 1.0), },
                        0.875) 
                    },
                    { 2, (0.25,
                        new List<(double, double)> { (0.0, 0.25), (0.75, 1.0), },
                        0.125)
                    },
                    { 3, (0.9,
                        new List<(double, double)> { 
                            (0.0, 1.5), (3.0, 3.5), (6.0, 7.5), (9.5, 11.0)},
                        10.5)
                    },
                    { 4, (0.25,
                        new List<(double, double)> {
                            (0.0, 1.5), (3.0, 3.5), (6.0, 7.5), (9.5, 11.0)},
                        1.25)
                    },
                    { 5, (0.313,
                        new List<(double, double)> {
                            (0.0, 1.5), (3.0, 3.5), (6.0, 7.5), (9.5, 11.0)},
                        3.065)
                    },
                    { 6, (0.317,
                        new List<(double, double)> {
                            (2.0, 4.0)
                        }, 2.634)
                    }
                };

        [TestCase(1)] // two equal ranges
        [TestCase(2)] // two equal ranges
        [TestCase(3)] // several unequal ranges
        [TestCase(4)] // several unequal ranges
        [TestCase(5)] // several unequal ranges
        [TestCase(6)] // single range
        public void TestTransformFractionToUniformDistribution(int testCaseId)
        {
            var testCase = this._testCaseDictionary[testCaseId];
            var transformationUtil = new TransformationUtil();
            var fraction = testCase.Item1;
            var ranges = testCase.Item2;
            var expectedResult = testCase.Item3;
            var actualResult = transformationUtil
                .TransformFractionToUniformDistribution(fraction, ranges);
            Assert.LessOrEqual(Math.Abs(expectedResult - actualResult), this.Epsilon);
        }
    }
}
