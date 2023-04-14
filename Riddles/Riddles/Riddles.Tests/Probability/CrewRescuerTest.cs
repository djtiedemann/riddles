using NUnit.Framework;
using Riddles.Probability;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Probability
{
    public class CrewRescuerTest
    {
        private double Epsilon = 0.00000001;

        private Dictionary<int, (List<List<(double, double)>>, double)>
            _testCaseDictionary = new Dictionary<int, (List<List<(double, double)>>, double)>
            {
                { 1, (new List<List<(double, double)>> {
                    new List<(double, double)> { (0, 0.5) },
                    new List<(double, double)> { (0, 0.5) },
                    new List<(double, double)> { (0, 0.5) },
                    new List <(double, double) > {(0, 0.5) },
                    new List <(double, double) > {(0, 0.5) },
                    new List <(double, double) > {(0, 0.5) },
                    new List <(double, double) > {(0, 0.5) }
                    },
                    2/8.0/3.0 + 3/8.0/2.0 + 3/8.0)
                },
                { 2, (new List<List<(double, double)>> {
                    new List<(double, double)> { (0, 0.624334) },
                    new List<(double, double)> { (0.0, 0.82492) },
                    new List<(double, double)> { (0.0, 0.460442) },
                    new List <(double, double) > {(0.0, 0.918159) },
                    new List <(double, double) > {(0, 0.813443) },
                    new List <(double, double) > {(0, 0.730221) },
                    new List <(double, double) > {(0, 0.347818) }
                    },
                    0.82395)
                }
            };

        private Dictionary<int, (List<List<(double, double)>>, double)>
            _simpleTestCaseDictionary = new Dictionary<int, (List<List<(double, double)>>, double)>
            {
                { 1, (new List<List<(double, double)>> {
                    new List<(double, double)> { (0.5, 1.0) },
                    new List<(double, double)> { (0.75, 1.0) },
                    new List<(double, double)> { (0.25, 1.0) }
                    },
                    0.875)
                }
            };

        [TestCase(1)]
        //[TestCase(2)]
        public void TestCalculateProbabilityOfRescuingCrew(int testCaseId)
        {
            var crewRescuer = new CrewRescuer();
            var testCase = this._testCaseDictionary[testCaseId];
            var questions = testCase.Item1;
            var expected = testCase.Item2;

            var actual = crewRescuer
                .CalculateProbabilityOfRescuingCrew(
                    questions[0],
                    questions[1],
                    questions[2],
                    questions[3],
                    questions[4],
                    questions[5],
                    questions[6]
                );
            Assert.LessOrEqual( Math.Abs(expected - actual), this.Epsilon );
        }

        [TestCase(1)]
        public void TestCalculateProbabilityOfRescuingTwoMemberCrew(int testCaseId)
        {
            var crewRescuer = new CrewRescuer();
            var testCase = this._simpleTestCaseDictionary[testCaseId];
            var questions = testCase.Item1;
            var expected = testCase.Item2;

            var actual = crewRescuer
                .CalculateProbabilityOfRescuingCrew(
                    questions[0],
                    questions[1],
                    questions[2]
                );
            Assert.LessOrEqual(Math.Abs(expected - actual), this.Epsilon);
        }
    }
}
