using NUnit.Framework;
using Riddles.Combinatorics.Core.Sets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Tests.Combinatorics.Core.Sets
{
    public class PowerSetCalculatorTest
    {
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        public void TestCalculateNextSubset(int testCaseId)
        {
            var expectedNumSubsets = Math.Pow(2, testCaseId);
            var set = Enumerable.Range(0, testCaseId).Select(i => Math.Pow(10, i)).Reverse().ToArray();
            var expectedMinPowerSetValue = 0;
            var expectedMaxPowerSetValue = set.Sum();
            var powerSetCalculator = new PowerSetCalculator();

            var actualNumSubsets = 0;
            (var powerSet, var indicator) = powerSetCalculator.CalculateNextSubset(
                set,
                (int[])null
            );
            var actualMinPowerSetValue = int.MaxValue;
            var actualMaxPowerSetValue = int.MinValue;
            var lastPowerSetValue = int.MinValue;
            while(powerSet != null)
            {
                actualNumSubsets++;
                var value = powerSet.Sum();
                Assert.Greater(value, lastPowerSetValue);
                lastPowerSetValue = (int)value;
                if (value < actualMinPowerSetValue)
                {
                    actualMinPowerSetValue = (int)value;
                }
                if (value > actualMaxPowerSetValue)
                {
                    actualMaxPowerSetValue = (int)value;
                }
                (powerSet, indicator) = powerSetCalculator.CalculateNextSubset(
                    set, indicator
                );
            }
            Assert.AreEqual(expectedNumSubsets, actualNumSubsets);
            Assert.AreEqual(expectedMaxPowerSetValue, actualMaxPowerSetValue);
            Assert.AreEqual(expectedMinPowerSetValue, actualMinPowerSetValue);
        }
    }
}
