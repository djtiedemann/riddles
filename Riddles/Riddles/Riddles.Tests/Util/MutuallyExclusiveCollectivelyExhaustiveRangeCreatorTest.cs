using NUnit.Framework;
using Riddles.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Tests.Util
{
    public class MutuallyExclusiveCollectivelyExhaustiveRangeCreatorTest
    {
        private double Epsilon = 0.00000001;

        public Dictionary<int, (
            List<List<(double, double)>>,
            List<List<(double, double, bool)>>
        )> _testCaseDictionary = new Dictionary<int, (
            List<List<(double, double)>>,
            List<List<(double, double, bool)>>
        )>
        {
            {1, (
                new List<List<(double, double)>> {
                    new List<(double, double)> {
                        (0, 1), (2, 4), (8, 12)
                    },
                    new List<(double, double)> {
                        (1, 5), (6, 10), (11, 12)
                    }
                },
                new List<List<(double, double, bool)>> {
                    new List<(double, double, bool)> {
                        (0, 1, true), (1, 2, false), (2, 4, true), (4, 5, false),
                        (5, 6, false), (6, 8, false), (8, 10, true),
                        (10, 11, true), (11, 12, true)
                    },
                    new List<(double, double, bool)> {
                        (0, 1, false), (1, 2, true), (2, 4, true), (4, 5, true),
                        (5, 6, false), (6, 8, true), (8, 10, true),
                        (10, 11, false), (11, 12, true)
                    }
                })
            },
            {2, (
                new List<List<(double, double)>> {
                    new List<(double, double)> {
                        (0, 1), (2, 4), (8, 12)
                    }
                },
                new List<List<(double, double, bool)>> {
                    new List<(double, double, bool)> {
                        (0, 1, true), (1, 2, false), (2, 4, true),
                        (4, 8, false), (8, 12, true)
                    }
                })
            },
            {3, (
                new List<List<(double, double)>> {
                    new List<(double, double)> {
                        (0, 1), (2, 4), (8, 12)
                    },
                    new List<(double, double)>
                    {
                        (0, 12)
                    }
                },
                new List<List<(double, double, bool)>> {
                    new List<(double, double, bool)> {
                        (0, 1, true), (1, 2, false), (2, 4, true),
                        (4, 8, false), (8, 12, true)
                    },
                    new List<(double, double, bool)> {
                        (0, 1, true), (1, 2, true), (2, 4, true),
                        (4, 8, true), (8, 12, true)
                    }
                })
            },
            {4, (
                new List<List<(double, double)>> {
                    new List<(double, double)> {
                        (0, 1), (2, 4)
                    },
                    new List<(double, double)>
                    {
                        (0, 4), (6, 11)
                    },
                    new List<(double, double)>
                    {
                        (2, 3), (7, 10)
                    }
                },
                new List<List<(double, double, bool)>> {
                    new List<(double, double, bool)> {
                        (0, 1, true), (1, 2, false), (2, 3, true), (3, 4, true),
                        (4, 6, false), (6, 7, false), (7, 10, false), (10, 11, false)
                    },
                    new List<(double, double, bool)> {
                        (0, 1, true), (1, 2, true), (2, 3, true), (3, 4, true),
                        (4, 6, false), (6, 7, true), (7, 10, true), (10, 11, true)
                    },
                    new List<(double, double, bool)> {
                        (0, 1, false), (1, 2, false), (2, 3, true), (3, 4, false),
                        (4, 6, false), (6, 7, false), (7, 10, true), (10, 11, false)
                    }
                })
            },
        };

        private Dictionary<int, ((double, double), List<(double, double)>, List<(double, double)>)>
            _createComplementaryRangeTestCaseDictionary = new Dictionary<int, ((double, double), List<(double, double)>, List<(double, double)>)>
            {
                { 1, ((0, 1), new List<(double, double)> { (0.1, 0.2), (0.3, 0.4), (0.5, 0.6), (0.7, 0.8), (0.9, 1) }, new List<(double, double)> { (0, 0.1), (0.2, 0.3), (0.4, 0.5), (0.6, 0.7), (0.8, 0.9) }) },
                { 2, ((0, 1), new List<(double, double)> { (0, 0.1), (0.2, 0.3), (0.4, 0.5), (0.6, 0.7), (0.8, 0.9) }, new List<(double, double)> { (0.1, 0.2), (0.3, 0.4), (0.5, 0.6), (0.7, 0.8), (0.9, 1) }) },
                { 3, ((5, 9), new List<(double, double)> { (5, 5.25), (7.25, 8.3), (8.6, 9) }, new List<(double, double)> { (5.25, 7.25), (8.3, 8.6) }) },
                { 4, ((5, 9), new List<(double, double)> { (5.25, 7.25), (8.3, 8.6) }, new List<(double, double)> { (5, 5.25), (7.25, 8.3), (8.6, 9) }) },
                { 5, ((1, 10), new List<(double, double)> { }, new List<(double, double)> { (1, 10) }) },
                { 6, ((0, 1), new List<(double, double)> { (0, 0.5) }, new List<(double, double)> { (0.5, 1) }) },
                { 7, ((0, 1), new List<(double, double)> { (0.5, 1) }, new List<(double, double)> { (0, 0.5) }) }
            };

        [TestCase(1)] // two non-trivial lists
        [TestCase(2)] // single list
        [TestCase(3)] // two lists, one trivial
        [TestCase(4)] // three lists
        public void TestCreateMeceRanges(int testCaseId)
        {
            var testCase = this._testCaseDictionary[testCaseId];
            var meceRangeCreator = new MutuallyExclusiveCollectivelyExhaustiveRangeCreator();
            var meceRanges = meceRangeCreator.CreateMeceRanges(
                testCase.Item1);
            var expected = testCase.Item2;
            Assert.AreEqual(expected.Count, meceRanges.Count);
            for(int i=0; i<expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Count, meceRanges[i].Count);
                for(int j=0; j < expected[i].Count; j++)
                {
                    Assert.AreEqual(
                        expected[i][j].Item1,
                        meceRanges[i][j].Item1
                    );
                    Assert.AreEqual(
                        expected[i][j].Item2,
                        meceRanges[i][j].Item2
                    );
                    Assert.AreEqual(
                        expected[i][j].Item3,
                        meceRanges[i][j].Item3
                    );
                }
            }
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        public void TestCreateComplementaryRanges(int testCaseId)
        {
            var meceRangeCalculator = new MutuallyExclusiveCollectivelyExhaustiveRangeCreator();
            var testCase = this._createComplementaryRangeTestCaseDictionary[testCaseId];
            var fullRange = testCase.Item1;
            var range = testCase.Item2;
            var expected = testCase.Item3;
            var actual = meceRangeCalculator.CreateComplementaryRanges(fullRange, range);

            Assert.LessOrEqual(Math.Abs((fullRange.Item2 - fullRange.Item1) -
                (range.Sum(r => r.Item2 - r.Item1) + actual.Sum(r => r.Item2 - r.Item1))), this.Epsilon);

            Assert.AreEqual(expected.Count, actual.Count);

            for(int i=0; i<expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Item1, actual[i].Item1);
                Assert.AreEqual(expected[i].Item2, actual[i].Item2);
            }

        }
    }
}
