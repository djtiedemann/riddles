using NUnit.Framework;
using Riddles.Probability;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Probability
{
    public class OligopolySolverTest
    {
        double epsilon = 0.000000001;

        private Dictionary<int, Dictionary<int, double>> outputDictionary =
            new Dictionary<int, Dictionary<int, double>> {
                {1,  new Dictionary<int, double>{
                    { 0, 1}, 
                    { 1, 1.0/6.0 },
                    { 2, 7.0/36.0 },
                    { 3, 49.0/216.0 },
                    { 4, 343.0/1296.0 },
                    { 5, 2401.0/7776.0 },
                    { 6, 16807.0/46656.0 },
                }},
                {2,  new Dictionary<int, double>{
                    { 0, 1},
                    { 1, 0 },
                    { 2, 1.0/36.0 },
                    { 3, 2.0/36.0 },
                    { 4, 109.0/1296.0 },
                    { 5, 37.0/324.0 },
                    { 6, 6841.0/46656.0 },
                }},
            };

        [TestCase(6, 1, 6, 1)]
        [TestCase(6, 2, 6, 2)]
        public void TestGetProbabilityOfLandingOnEachSpace(
            int numSpaces, 
            int numDice, 
            int sizeOfDice,
            int outputId)
        {
            var oligopolySolver = new OligopolySolver();
            var actualOutput = oligopolySolver.GetProbabilityOfLandingOnEachSpace(
                numSpaces,
                numDice,
                sizeOfDice
            );
            var expectedOutput = this.outputDictionary[outputId];
            Assert.AreEqual(expectedOutput.Keys.Count, actualOutput.Keys.Count);
            foreach(var key in expectedOutput.Keys)
            {
                Assert.LessOrEqual(Math.Abs(expectedOutput[key] - actualOutput[key])
                    , epsilon);
            }
        }

        [TestCase(40, 2, 6, 7, 0.18222736625514405)]
        [TestCase(40, 3, 6, 11, 0.13040569987425696)]
        public void TestGetMostLikelyLandingSpace(int numSpaces,
            int numDice,
            int sizeOfDice,
            int expectedBestSpace,
            double expectedBestProbability)
        {
            var oligopolySolver = new OligopolySolver();
            var (bestSpace, bestProbability) = oligopolySolver.GetMostLikelyLandingSpace(
                numSpaces,
                numDice,
                sizeOfDice
            );
            Assert.AreEqual(expectedBestSpace, bestSpace);
            Assert.LessOrEqual(Math.Abs(bestProbability - expectedBestProbability), epsilon);
        }
    }
}
