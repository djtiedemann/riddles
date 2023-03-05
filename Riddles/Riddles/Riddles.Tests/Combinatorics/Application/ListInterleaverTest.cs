using NUnit.Framework;
using Riddles.Combinatorics.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Combinatorics.Application
{
    public class ListInterleaverTest
    {
        Dictionary<int, (List<int>, int)> testCaseDictionary = new Dictionary<int, (List<int>, int)>
        {
            {1, (new List<int> { 1 }, 1) },
            {2, (new List<int> { 4 }, 1) },
            {3, (new List<int> {  }, 0) },
            {4, (new List<int> { 4, 1, -1 }, 0) },
            {5, (new List<int> { 4, 0, 0, 0 }, 1) },
            {6, (new List<int> { 4, 1 }, 5) },
            {7, (new List<int> { 4, 4 }, 70) },
            {8, (new List<int> { 1, 1, 1, 1, 1 }, 120) },
            {9, (new List<int> {2, 2, 2}, 90) },
            {10, (new List<int> {8, 4}, 495 )  },
            {11, (new List<int> {12, 2}, 91 )  },
            {12, (new List<int> {14, 1}, 15) }
        };

        [TestCase(1)] // one list
        [TestCase(2)] // one list
        [TestCase(3)] // no lists
        [TestCase(4)] // list with invalid count
        [TestCase(5)] // multiple lists with 0 elements
        [TestCase(6)] // two lists, one of size 1
        [TestCase(7)] // two non-trivial lists
        [TestCase(8)] // five lists of size 1
        [TestCase(9)] // three lists of size 2
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        public void TestCountNumberOfWaysToInterleaveLists(int testCaseId)
        {
            var listInterleaver = new ListInterleaver();
            var testCase = testCaseDictionary[testCaseId];
            var actual = listInterleaver.CountNumberOfWaysToInterleaveLists(testCase.Item1);
            var expected = testCase.Item2;
            Assert.AreEqual(expected, actual);
        }
    }
}
