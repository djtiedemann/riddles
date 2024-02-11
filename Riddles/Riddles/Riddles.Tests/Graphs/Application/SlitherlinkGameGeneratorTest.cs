using NUnit.Framework;
using Riddles.Graphs.Application;
using Riddles.Graphs.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Tests.Graphs.Application
{
    public class SlitherlinkGameGeneratorTest
    {
        private Dictionary<int, (int, int, int[], int[])>
            _testEdgeCountForCycleTestCases = new Dictionary<int, (int, int, int[], int[])> {
                { 1, (3, 3, new int[] { 0, 1, 4, 3, 0 }, new int[] { 4, 1, 1, 0 })},
                { 2, (3, 3, new int[] { 1, 2, 5, 4, 1 }, new int[] { 1, 4, 0, 1 })},
                { 3, (3, 3, new int[] { 3, 4, 7, 6, 3 }, new int[] { 1, 0, 4, 1 })},
                { 4, (3, 3, new int[] { 4, 5, 8, 7, 4 }, new int[] { 0, 1, 1, 4 })},
                { 5, (3, 3, new int[] { 0, 1, 2, 5, 4, 3, 0 }, new int[] { 3, 3, 1, 1 })},
                { 6, (3, 3, new int[] { 3, 4, 5, 8, 7, 6, 3 }, new int[] { 1, 1, 3, 3 })},
                { 7, (3, 3, new int[] { 0, 1, 4, 7, 6, 3, 0 }, new int[] { 3, 1, 3, 1 })},
                { 8, (3, 3, new int[] { 1, 2, 5, 8, 7, 4, 1 }, new int[] { 1, 3, 1, 3 })},
                { 9, (3, 3, new int[] { 0, 1, 2, 5, 8, 7, 4, 3, 0 }, new int[] { 3, 2, 2, 3 })},
                { 10, (3, 3, new int[] { 1, 2, 5, 8, 7, 6, 3, 4, 1 }, new int[] { 2, 3, 3, 2 })},
                { 11, (3, 3, new int[] { 0, 1, 4, 5, 8, 7, 6, 3, 0 }, new int[] { 3, 2, 2, 3 })},
                { 12, (3, 3, new int[] { 0, 1, 2, 5, 4, 7, 6, 3, 0 }, new int[] { 2, 3, 3, 2 })},
                { 13, (3, 3, new int[] { 0, 1, 2, 5, 8, 7, 6, 3, 0 }, new int[] { 2, 2, 2, 2 })},
            };

        private Dictionary<int, (int[], List<string>, List<string>)> _testGenerateCanonicalPuzzleRepresentationTestCaseDictionary =
            new Dictionary<int, (int[], List<string>, List<string>)>
            {
                { 1, (new int[] { 4, 1, 1, 0 }, 
                    new List<string> { 
                        "0000", "0001", "0010", "0011", "0100", "0101", "0110", "0111",
                        "1000", "1001", "1010", "1011", "1100", "1101", "1110", "1111",
                    },
                    new List<string>{
                        "____", "___0", "__1_", "__10", "_1__", "_1_0", "_11_", "_110",
                        "4___", "4__0", "4_1_", "4_10", "41__", "41_0", "411_", "4110",
                    })
                }
            };

        [TestCase(3, 3, 93)]
        [TestCase(4, 4, 41433)]
        public void TestGenerateNumValidSlitherlinkGames(int length, int width, int expected)
        {
            var slitherlinkGameGenerator = new SlitherlinkGameGenerator();
            var actual = slitherlinkGameGenerator.GenerateNumValidSlitherlinkGames(length, width);
            Assert.AreEqual(expected, actual);
        }

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
        public void TestEdgeCountForCycle(int testCaseId)
        {
            var testCase = this._testEdgeCountForCycleTestCases[testCaseId];
            var slitherlinkGameGenerator = new SlitherlinkGameGenerator();
            var cycle = testCase.Item3.Select(
                c => new TwoDimensionalRectangularGrid.Location(id: c, length: testCase.Item1, width: testCase.Item2)
            );
            var actual = slitherlinkGameGenerator.GetEdgeCountForCycle(
                testCase.Item1,
                testCase.Item2,
                cycle.ToList()
            );
            Assert.AreEqual(actual.Length, testCase.Item4.Length);
            for(int i=0; i<actual.Length; i++)
            {
                Assert.AreEqual(actual[i], testCase.Item4[i]);
            }
        }

        [TestCase(1, 0, 0, 0, 1, 3, 3, new int[] { 0 })]
        [TestCase(2, 0, 1, 0, 2, 3, 3, new int[] { 1 })]
        [TestCase(3, 2, 0, 2, 1, 3, 3, new int[] { 2 })]
        [TestCase(4, 2, 1, 2, 2, 3, 3, new int[] { 3 })]
        [TestCase(5, 1, 0, 1, 1, 3, 3, new int[] { 0, 2 })]
        [TestCase(6, 1, 1, 1, 2, 3, 3, new int[] { 1, 3 })]
        [TestCase(7, 0, 0, 1, 0, 3, 3, new int[] { 0 })]
        [TestCase(8, 1, 0, 2, 0, 3, 3, new int[] { 2 })]
        [TestCase(9, 0, 2, 1, 2, 3, 3, new int[] { 1 })]
        [TestCase(10, 1, 2, 2, 2, 3, 3, new int[] { 3 })]
        [TestCase(11, 0, 1, 1, 1, 3, 3, new int[] { 0, 1 })]
        [TestCase(12, 1, 1, 2, 1, 3, 3, new int[] { 2, 3 })]
        public void TestGetIndexesForEdge(
            int testCaseId,
            int row1,
            int col1,
            int row2,
            int col2,
            int length,
            int width,
            int[] expected)
        {
            var location1 = new TwoDimensionalRectangularGrid.Location(
                row: row1,
                col: col1,
                length: length,
                width: width
            );
            var location2 = new TwoDimensionalRectangularGrid.Location(
                row: row2,
                col: col2,
                length: length,
                width: width
            );
            var slitherlinkGameGenerator = new SlitherlinkGameGenerator();
            var actual = slitherlinkGameGenerator.GetIndexesForEdge(
                location1: location1,
                location2: location2,
                length: length,
                width: width
            ).OrderBy(x => x).ToArray();
            Assert.AreEqual(actual.Length, expected.Length);
            for(int i=0; i<actual.Length; i++)
            {
                Assert.AreEqual(actual[i], expected[i]);
            }
        }

        [TestCase(1)]
        public void TestGenerateCanonicalPuzzleRepresentation(int testCaseId)
        {
            var testCase = this._testGenerateCanonicalPuzzleRepresentationTestCaseDictionary[testCaseId];
            var slitherlinkGameGenerator = new SlitherlinkGameGenerator();
            var actual = slitherlinkGameGenerator.GenerateCanonicalPuzzleRepresentation(
                testCase.Item1,
                testCase.Item2
            );
            Assert.AreEqual(actual.Count, testCase.Item3.Count);
            for(int i=0; i<actual.Count; i++)
            {
                Assert.AreEqual(actual.ElementAt(i), testCase.Item3.ElementAt(i));
            }
        }
    }
}
