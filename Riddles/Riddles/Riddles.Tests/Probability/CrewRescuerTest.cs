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
                }
            };

        [TestCase(1)]
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
    }
}
