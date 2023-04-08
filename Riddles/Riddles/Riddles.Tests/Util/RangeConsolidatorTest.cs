using NUnit.Framework;
using Riddles.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Util
{
    public class RangeConsolidatorTest
    {
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

        [TestCase(1)] // two non-trivial lists
        [TestCase(2)] // single list
        [TestCase(3)] // two lists, one trivial
        [TestCase(4)] // three lists
        public void TestConsolidateRanges(int testCaseId)
        {
            var testCase = this._testCaseDictionary[testCaseId];
            var rangeConsolidator = new RangeConsolidator();
            var consolidatedRanges = rangeConsolidator.ConsolidateRanges(
                testCase.Item1);
            var expected = testCase.Item2;
            Assert.AreEqual(expected.Count, consolidatedRanges.Count);
            for(int i=0; i<expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Count, consolidatedRanges[i].Count);
                for(int j=0; j < expected[i].Count; j++)
                {
                    Assert.AreEqual(
                        expected[i][j].Item1,
                        consolidatedRanges[i][j].Item1
                    );
                    Assert.AreEqual(
                        expected[i][j].Item2,
                        consolidatedRanges[i][j].Item2
                    );
                    Assert.AreEqual(
                        expected[i][j].Item3,
                        consolidatedRanges[i][j].Item3
                    );
                }
            }
        }
    }
}
