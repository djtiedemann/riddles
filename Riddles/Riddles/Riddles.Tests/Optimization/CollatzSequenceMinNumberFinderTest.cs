using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Optimization;

namespace Riddles.Tests.Optimization
{
    public class CollatzSequenceMinNumberFinderTest
    {
        [TestCase(2024, 399)]
        public void TestFindMinNumberWithValueInCollatzSequence(int searchingNumber, int expected)
        {
            var collatzSequenceMinNumberFinder = new CollatzSequenceMinNumberFinder();
            var actual = collatzSequenceMinNumberFinder
                .FindMinNumberWithValueInCollatzSequence(searchingNumber);
            Assert.AreEqual(expected, actual);
        }
    }
}
