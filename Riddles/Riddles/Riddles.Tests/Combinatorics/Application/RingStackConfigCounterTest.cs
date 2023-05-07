using NUnit.Framework;
using Riddles.Combinatorics.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Combinatorics.Application
{
    public class RingStackConfigCounterTest
    {
        private Dictionary<int, int> testCaseDictionary = new Dictionary<int, int>
        {
            { 1, 1 },
            { 2, 3 },
            { 3, 8 },
            { 4, 22 },
            { 5, 65}
        };

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void TestCountNumberOfRingConfigurations(int testCaseId)
        {
            var ringStackConfigurationCounter = new RingStackConfigCounter();
            var expectedNumConfigs = testCaseDictionary[testCaseId];
            var actualNumConfigs = ringStackConfigurationCounter
                .CountNumberOfRingConfigurations(testCaseId);
            Assert.AreEqual(expectedNumConfigs, actualNumConfigs);
        }
    }
}
