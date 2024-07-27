using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Probability.Application;

namespace Riddles.Tests.Probability.Application
{
    public class ThueMorseSequenceProbabilitySolverTest
    {
        private double epsilon = 0.00000000001;
        private Dictionary<int, (
            ThueMorseSequenceProbabilitySolver.Strategy,
            double,
            double)
        > _testCaseDictionary = new Dictionary<int, (
            ThueMorseSequenceProbabilitySolver.Strategy,
            double,
            double)>
            {
                { 1, (
                        ThueMorseSequenceProbabilitySolver.Strategy.Alternating, 
                        1.0/6.0,
                        0.54545454545006478
                    )
                },
                { 2, (
                        ThueMorseSequenceProbabilitySolver.Strategy.Snake,
                        1.0/6.0,
                        0.50819672130565718
                    )
                },
                { 3, (
                        ThueMorseSequenceProbabilitySolver.Strategy.ThueMorse,
                        1.0/6.0,
                        0.50159033919854623
                    )
                },
            };

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void TestCalculateOddsOfFirstPersonWinning(
            int testCaseId
        )
        {
            var (strategy, oddsOfSuccess, expected) =
                this._testCaseDictionary[testCaseId];
            var sequenceProbabilitySolver =
                new ThueMorseSequenceProbabilitySolver();
            var actual = sequenceProbabilitySolver
                .CalculateOddsOfFirstPersonWinning(
                    strategy,
                    oddsOfSuccess,
                    epsilon
                );
            Assert.LessOrEqual(Math.Abs(expected - actual), epsilon);
        }
    }
}
