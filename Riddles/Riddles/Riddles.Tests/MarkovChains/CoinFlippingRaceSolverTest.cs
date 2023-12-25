using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Riddles.MarkovChains;

namespace Riddles.Tests.MarkovChains
{
    public class CoinFlippingRaceSolverTest
    {
        private double epsilon = 0.00000000001;

        private Dictionary<int, (List<List<string>>, List<List<string>>, bool)>
            _testEqualityTestCaseDictionary = new Dictionary<int, (List<List<string>>, List<List<string>>, bool)>
            {
                { 1, (new List<List<string>>(), new List<List<string>>(), true) },
                { 2, (new List<List<string>>(),
                    new List<List<string>>{
                        new List<string> { },
                        new List<string> { },
                        new List<string> { },
                    },
                false) },
                { 3, (new List<List<string>>{
                        new List<string> { },
                        new List<string> { },
                    },
                    new List<List<string>>{
                        new List<string> { },
                        new List<string> { },
                        new List<string> { },
                    },
                false) },
                { 4, (new List<List<string>>{
                        new List<string> { },
                        new List<string> { },
                        new List<string> { },
                    },
                    new List<List<string>>{
                        new List<string> { },
                        new List<string> { },
                        new List<string> { },
                    },
                true) },
                { 5, (new List<List<string>>{
                        new List<string> { "H" },
                        new List<string> { "H" },
                        new List<string> { "H" },
                    },
                    new List<List<string>>{
                        new List<string> { "H" },
                        new List<string> { "H" },
                        new List<string> { },
                    },
                false) },
                { 6, (new List<List<string>>{
                        new List<string> { "H" },
                        new List<string> { "H" },
                        new List<string> { "H" },
                    },
                    new List<List<string>>{
                        new List<string> { "H" },
                        new List<string> { "H" },
                        new List<string> { "H" },
                    },
                true) },
                { 7, (new List<List<string>>{
                        new List<string> { "H" },
                        new List<string> { "H" },
                        new List<string> { "H" },
                    },
                    new List<List<string>>{
                        new List<string> { "H" },
                        new List<string> { "H" },
                        new List<string> { "T" },
                    },
                false) },
                { 8, (new List<List<string>>{
                        new List<string> { "T" },
                        new List<string> { "H" },
                        new List<string> { "H" },
                    },
                    new List<List<string>>{
                        new List<string> { "H" },
                        new List<string> { "T" },
                        new List<string> { "H" },
                    }, 
                false) },
                { 9, (new List<List<string>>{
                        new List<string> { "T", "H" },
                        new List<string> { "H" },
                        new List<string> { "H" },
                    },
                    new List<List<string>>{
                        new List<string> { "H", "T" },
                        new List<string> { "H" },
                        new List<string> { "H" },
                    },
                false) },
                 { 10, (new List<List<string>>{
                        new List<string> { "H", "T" },
                        new List<string> { "H", "H", "H" },
                        new List<string> { "H", "H" },
                    },
                    new List<List<string>>{
                        new List<string> { "H", "T" },
                        new List<string> { "H", "H", "H" },
                        new List<string> { "H", "H" },
                    },
                true) },
                 { 11, (new List<List<string>>{
                        new List<string> { "H", "T" },
                        new List<string> { "H", "H", "T" },
                        new List<string> { "H", "H" },
                    },
                    new List<List<string>>{
                        new List<string> { "H", "T" },
                        new List<string> { "H", "H", "H" },
                        new List<string> { "H", "H" },
                    },
                false) }
            };

        private Dictionary<int, (List<List<string>>, List<List<string>>, bool, string?)>
            _testTerminalStateTestCaseDictionary = new Dictionary<int, (List<List<string>>, List<List<string>>, bool, string?)>
            {
                { 1, (
                    new List<List<string>> { 
                        new List<string> { },
                        new List<string> { },
                        new List<string> { },
                    }, 
                    new List<List<string>>{
                        new List<string> { "H", "T" },
                        new List<string> { "T", "T" },
                        new List<string> { "H", "H", "H" },
                    }, 
                    false, 
                    null ) },
                { 2, (
                    new List<List<string>> {
                        new List<string> { "T", "T" },
                        new List<string> { "T", "H" },
                        new List<string> { "H", "H", "T"},
                    },
                    new List<List<string>>{
                        new List<string> { "H", "T" },
                        new List<string> { "T", "T" },
                        new List<string> { "H", "H", "H" },
                    },
                    false,
                    null ) }, // no one has won
                { 3, (
                    new List<List<string>> {
                        new List<string> { "T", "T" },
                        new List<string> { "T", "H" },
                        new List<string> { "H", "H", "T"},
                    },
                    new List<List<string>>{
                        new List<string> { "T", "T" },
                        new List<string> { "T", "T" },
                        new List<string> { "H", "H", "H" },
                    },
                    true,
                    "0" ) },
                { 4, (
                    new List<List<string>> {
                        new List<string> { "H", "T" },
                        new List<string> { "T", "T" },
                        new List<string> { "H", "H", "T"},
                    },
                    new List<List<string>>{
                        new List<string> { "T", "T" },
                        new List<string> { "T", "T" },
                        new List<string> { "H", "H", "H" },
                    },
                    true,
                    "1" ) },
                { 5, (
                    new List<List<string>> {
                        new List<string> { "H", "T" },
                        new List<string> { "T", "T" },
                        new List<string> { "H", "H", "T"},
                    },
                    new List<List<string>>{
                        new List<string> { "T", "T" },
                        new List<string> { "T", "H" },
                        new List<string> { "H", "H", "T" },
                    },
                    true,
                    "2" ) },
                { 6, (
                    new List<List<string>> {
                        new List<string> { "H", "T" },
                        new List<string> { "T", "T" },
                        new List<string> { "H", "H", "T"},
                    },
                    new List<List<string>>{
                        new List<string> { "H", "T" },
                        new List<string> { "T", "H" },
                        new List<string> { "H", "H", "T" },
                    },
                    false,
                    null ) }, // 2 winners
                { 7, (
                    new List<List<string>> {
                        new List<string> { "H", "T" },
                        new List<string> { "T", "T" },
                        new List<string> { "H", "H", "T"},
                    },
                    new List<List<string>>{
                        new List<string> { "H", "T" },
                        new List<string> { "T", "T" },
                        new List<string> { "H", "H", "T" },
                    },
                    false,
                    null ) }, // 3 winners
            };

        private Dictionary<int, (List<List<string>>, string, Dictionary<char, double>, double)> 
            _testCaseDictionary =
            new Dictionary<int, (List<List<string>>, string, Dictionary<char, double>, double)> {
                { 1, (new List<List<string>> { 
                    new List<string> { "H" },
                    new List<string> { "T" }
                }, "0", new Dictionary<char, double> { { 'H', 0.5}, { 'T', 0.5} }, 0.5)},
                { 2, (new List<List<string>> {
                    new List<string> { "T", "T", "H" },
                    new List<string> { "T", "T", "T" }
                }, "0", new Dictionary<char, double> { { 'H', 0.5}, { 'T', 0.5} }, 45_667.0/71_106.0)},
        };

        private Dictionary<int, (int, int, Dictionary<char, double>, Dictionary<string, double>)>
            _mostEvenTestCaseDictionary = new Dictionary<int, (int, int, Dictionary<char, double>, Dictionary<string, double>)>
            {
                { 1, (2, 3, new Dictionary<char, double> { { 'H', 0.5 }, { 'T', 0.5} }, new Dictionary<string, double> { { "THT", 0.49644519065771492 } })}
            };

        [TestCase(1)]
        [TestCase(2)]
        public void TestCalculateProbabilityOfCertainPlayerWinningCoinFlipRace(int testCaseId) { 
            var testCase = this._testCaseDictionary[testCaseId];
            var coinFlippingRaceSolver = new CoinFlippingRaceSolver();
            var actual = coinFlippingRaceSolver
                .CalculateProbabilityOfCertainPlayerWinningCoinFlipRace(
                    testCase.Item2,
                    testCase.Item3,
                    testCase.Item1
                );
            Assert.LessOrEqual(Math.Abs(actual - testCase.Item4), this.epsilon);
        }

        [TestCase(1)]
        public void TestFindMostEvenStartingPick(int testCaseId)
        {
            var coinFlippingRaceSolver = new CoinFlippingRaceSolver();
            var testCase = this._mostEvenTestCaseDictionary[testCaseId];
            var actual = coinFlippingRaceSolver.FindWinningLikelihoodBasedOnStartingPick(
                testCase.Item1,
                testCase.Item3,
                testCase.Item2
            );
            foreach(var key in testCase.Item4.Keys)
            {
                Assert.LessOrEqual(Math.Abs(testCase.Item4[key] - actual[key])
                    , epsilon);
            }
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
        public void TestGetEquals(int testCaseId)
        {
            var testCase = this._testEqualityTestCaseDictionary[testCaseId];
            var winningFlips = new List<List<string>>
            {
                new List<string> { "H", "H", "H" },
                new List<string> { "H", "H", "H" },
                new List<string> { "H", "H", "H" },
            };
            var coinFlippingRaceState1 = new CoinFlippingRaceSolver
                .CoinFlippingRaceState(testCase.Item1, winningFlips);
            var coinFlippingRaceState2 = new CoinFlippingRaceSolver
                .CoinFlippingRaceState(testCase.Item2, winningFlips);
            Assert.AreEqual(coinFlippingRaceState1.Equals(coinFlippingRaceState2), testCase.Item3);
            Assert.AreEqual(coinFlippingRaceState2.Equals(coinFlippingRaceState1), testCase.Item3);
            Assert.AreEqual(coinFlippingRaceState1.Equals(coinFlippingRaceState1), true);
            Assert.AreEqual(coinFlippingRaceState2.Equals(coinFlippingRaceState2), true);
        }
        
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        public void TestIsTerminalStateAndLabel(int testCaseId)
        {
            var testCase = this._testTerminalStateTestCaseDictionary[testCaseId];
            var coinFlippingRaceState = new CoinFlippingRaceSolver.CoinFlippingRaceState(
                testCase.Item1,
                testCase.Item2
            );
            Assert.AreEqual(coinFlippingRaceState.IsStateTerminalState(), testCase.Item3);
            Assert.AreEqual(coinFlippingRaceState.TerminalStateLabel(), testCase.Item4);
        }

        [Test]
        public void TestDeepClone()
        {
            var flips = new List<List<string>>
            {
                new List<string> { "H", "H", "H" },
                new List<string> { "H", "T", "H" },
                new List<string> { "H", "T", "H" },
            };
            var winningFlips = new List<List<string>>
            {
                new List<string> { "H", "H", "H" },
                new List<string> { "H", "H", "H" },
                new List<string> { "H", "H", "H" },
            };
            var initialState = new CoinFlippingRaceSolver.CoinFlippingRaceState(
                flips,
                winningFlips
            );
            var newCurrentFlips = initialState.DeepCloneCurrentFlips();
            var newState = new CoinFlippingRaceSolver.CoinFlippingRaceState(
                newCurrentFlips,
                winningFlips
            );
            Assert.AreEqual(newState.CurrentFlips, initialState.CurrentFlips);
            Assert.AreNotSame(newState.CurrentFlips, initialState.CurrentFlips);
            Assert.AreEqual(newState.CurrentFlips[0], initialState.CurrentFlips[0]);
            Assert.AreNotSame(newState.CurrentFlips[0], initialState.CurrentFlips[0]);
            Assert.AreEqual(newState.CurrentFlips[1], initialState.CurrentFlips[1]);
            Assert.AreNotSame(newState.CurrentFlips[1], initialState.CurrentFlips[1]);
            Assert.AreEqual(newState.CurrentFlips[2], initialState.CurrentFlips[2]);
            Assert.AreNotSame(newState.CurrentFlips[2], initialState.CurrentFlips[2]);
            newState.CurrentFlips[0][0] = "T";
            Assert.AreNotEqual(newState, initialState);
        }
    }
}
