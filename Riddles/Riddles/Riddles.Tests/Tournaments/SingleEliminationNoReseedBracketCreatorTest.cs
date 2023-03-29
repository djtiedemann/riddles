using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Tournaments;

namespace Riddles.Tests.Tournaments
{
    public class SingleEliminationNoReseedBracketCreatorTest
    {
        private Dictionary<int, (List<int>, List<int?>)> TestCaseDictionary =
        new Dictionary<int, (List<int>, List<int?>)>
        {
            {1, (new List<int> {}, new List<int?>{ })},
            {2, (new List<int> { 22 }, new List<int?>{ 22 })},
            {3, (new List<int> { 44, 22 }, new List<int?>{ 22, 44 })},
            {4, (new List<int> { 3, 2, 1 }, new List<int?>{ 1, null, 2, 3 })},
            {5, (new List<int> { 4, 3, 2, 1 }, new List<int?>{ 1, 4, 2, 3 })},
            {6, (new List<int> { 5, 4, 3, 2, 1 }, 
                new List<int?>{ 1, null, 4, 5, 2, null, 3, null })},
            {7, (new List<int> { 6, 5, 4, 3, 2, 1 },
                new List<int?>{ 1, null, 4, 5, 2, null, 3, 6 })},
            {8, (new List<int> { 7, 6, 5, 4, 3, 2, 1 },
                new List<int?>{ 1, null, 4, 5, 2, 7, 3, 6 })},
            {9, (new List<int> { 8, 7, 6, 5, 4, 3, 2, 1 },
                new List<int?>{ 1, 8, 4, 5, 2, 7, 3, 6 })},
            {10, (new List<int> { 9, 8, 7, 6, 5, 4, 3, 2, 1 },
                new List<int?>{ 1, null, 8, 9, 4, null, 5, null, 2, null, 7, null, 3, null, 6, null })},
            {11, (new List<int> { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 },
                new List<int?>{ 1, null, 8, 9, 4, null, 5, null, 2, null, 7, 10, 3, null, 6, null })},
            {12, (new List<int> { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 },
                new List<int?>{ 1, null, 8, 9, 4, null, 5, null, 2, null, 7, 10, 3, null, 6, 11 })},
            {13, (new List<int> { 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 },
                new List<int?>{ 1, null, 8, 9, 4, null, 5, 12, 2, null, 7, 10, 3, null, 6, 11 })},
            {14, (new List<int> { 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 },
                new List<int?>{ 1, null, 8, 9, 4, 13, 5, 12, 2, null, 7, 10, 3, null, 6, 11 })},
            {15, (new List<int> { 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 },
                new List<int?>{ 1, null, 8, 9, 4, 13, 5, 12, 2, null, 7, 10, 3, 14, 6, 11 })},
            {16, (new List<int> { 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 },
                new List<int?>{ 1, null, 8, 9, 4, 13, 5, 12, 2, 15, 7, 10, 3, 14, 6, 11 })},
            {17, (new List<int> { 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 },
                new List<int?>{ 1, 16, 8, 9, 4, 13, 5, 12, 2, 15, 7, 10, 3, 14, 6, 11 })},
            {18, (new List<int> { 116, 115, 114, 113, 112, 111, 110, 109, 108, 107, 106, 105, 104, 103, 102, 101 },
                new List<int?>{ 101, 116, 108, 109, 104, 113, 105, 112, 102, 115, 107, 110, 103, 114, 106, 111 })},
        };

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        [TestCase(13)]
        [TestCase(14)]
        [TestCase(15)]
        [TestCase(16)]
        [TestCase(17)]
        [TestCase(18)]
        public void TestCreateBracket(int testCaseId)
        {
            var testCase = this.TestCaseDictionary[testCaseId];
            var bracketCalculator = new SingleEliminationNoReseedBracketCreator();
            var actualBracket = bracketCalculator
                .CreateBracket(testCase.Item1);
            var expectedBracket = testCase.Item2.ToArray();

            Assert.AreEqual(expectedBracket.Length, actualBracket.Length);
            for(int i=0; i<expectedBracket.Length; i++)
            {
                Assert.AreEqual(expectedBracket[i], actualBracket[i]);
            }
        }
    }
}
