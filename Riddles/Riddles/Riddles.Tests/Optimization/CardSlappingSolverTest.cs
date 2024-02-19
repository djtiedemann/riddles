using NUnit.Framework;
using Riddles.Optimization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Tests.Optimization
{
    public class CardSlappingSolverTest
    {

        [TestCase(4, 9)]
        [TestCase(5, 15)]
        [TestCase(6, 21)]
        //[TestCase(7, 31)] // commented out, takes 10s
        public void TestFindLongestSequenceThatIsNeverSlappable(int numCards, int expected)
        {
            var cardSlappingSolver = new CardSlappingSolver();
            var longestSequence = cardSlappingSolver.FindLongestSequenceThatIsNeverSlappable(numCards);
            Assert.AreEqual(expected, longestSequence.Count);
        }

        [TestCase(new int[] { 3, 2, 3, 4, 1 }, true)]
        [TestCase(new int[] { 1, 2, 4, 3, 3 }, true)]
        [TestCase(new int[] { 2, 3, 1, 2 }, true)]
        [TestCase(new int[] { 2, 3 }, false)]
        [TestCase(new int[] { 2, 2 }, true)]
        [TestCase(new int[] { 2, 2, 3 }, false)]
        [TestCase(new int[] { 3, 2, 2 }, true)]
        [TestCase(new int[] { 3, 2, 2, 4, 3 }, true)]
        [TestCase(new int[] { 1, 2, 3, 4, 3 }, false)]
        public void TestCanSequenceBeSlapped(int[] testSequence, bool expected)
        {
            var cardSlappingSolver = new CardSlappingSolver();
            var actual = cardSlappingSolver.CanSequenceBeSlapped(testSequence.ToList());
            Assert.AreEqual(expected, actual);
        }
    }
}
