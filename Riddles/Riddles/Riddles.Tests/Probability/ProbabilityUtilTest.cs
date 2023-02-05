using NUnit.Framework;
using Riddles.Combinatorics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Probability
{
    public class ProbabilityUtilTest
    {
        private Dictionary<int, List<BaseSetInfo>> testCaseDictionary = new Dictionary<int, List<BaseSetInfo>>
        {
            {1, new List<BaseSetInfo>
            {
                new BaseSetInfo(numElements: 6, numDesired: 3, label: "Red balls"),
                new BaseSetInfo(numElements: 4, numDesired: 1, label: "Green balls")
            } },
            { 2, new List<BaseSetInfo>
            {
                new BaseSetInfo(numElements: 6, numDesired: 7, label: "Red balls"),
                new BaseSetInfo(numElements: 4, numDesired: 1, label: "Green balls")
            } },
            { 3, new List<BaseSetInfo>
            {
                new BaseSetInfo(numElements: 6, numDesired: -1, label: "Red balls"),
                new BaseSetInfo(numElements: 4, numDesired: 4, label: "Green balls")
            } },
            { 4, new List<BaseSetInfo>
            {
                new BaseSetInfo(numElements: 22, numDesired: 8, label: "Red balls"),
                new BaseSetInfo(numElements: 13, numDesired: 7, label: "Green balls"),
                new BaseSetInfo(numElements: 7, numDesired: 3, label: "Blue balls")
            } }
        };

        [TestCase(1, 80.0 / 210)]
        [TestCase(2, 0)]
        [TestCase(3, 0)]
        [TestCase(4, 19205386200.0 / 353697121050)]
        public void TestCalculateOddsOfDrawingIndistinctObjectsWithoutReplacement
            (int testCaseId, double expected)
        {
            var testCase = testCaseDictionary[testCaseId];
            var probabilityUtil = new ProbabilityUtil();
            var actual = probabilityUtil
                .CalculateOddsOfDrawingIndistinctObjectsWithoutReplacement(testCase);
            Assert.AreEqual(expected, actual);
        }
    }
}
