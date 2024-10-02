using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Graphs.Core;
using NUnit.Framework;
using System.Linq;

namespace Riddles.Tests.Graphs.Core
{
    public class EulerianCircuitFinderTest
    {
        [Test]
        public void TestGetRockPaperScissorsCircuits()
        {
            var rockPaperScissorsAdjacencies =
                new Dictionary<string, HashSet<string>>
                {
                    { "Rock", new HashSet<string> { "Scissors" } },
                    { "Scissors", new HashSet<string> { "Paper" } },
                    { "Paper", new HashSet<string> { "Rock" } },
                };

            var eulerianCircuitFinder = new EulerianCircuitFinder();
            var circuits = eulerianCircuitFinder.FindCircuits(
                rockPaperScissorsAdjacencies
            );
            this.VerifyUniqueness(circuits);
            Assert.AreEqual(3, circuits.Count);
        }

        [Test]
        public void TestGetRockPaperScissorsLizardSpockCircuits()
        {
            var rockPaperScissorsAdjacencies =
                new Dictionary<string, HashSet<string>>
                {
                    { "Rock", new HashSet<string> { "Scissors", "Lizard" } },
                    { "Scissors", new HashSet<string> { "Paper", "Lizard" } },
                    { "Paper", new HashSet<string> { "Rock", "Spock" } },
                    { "Lizard", new HashSet<string> { "Paper", "Spock" } },
                    { "Spock", new HashSet<string> { "Scissors", "Rock" } },
                };

            var eulerianCircuitFinder = new EulerianCircuitFinder();
            var circuits = eulerianCircuitFinder.FindCircuits(
                rockPaperScissorsAdjacencies
            );
            this.VerifyUniqueness(circuits);
            Assert.AreEqual(110, circuits.Count);
        }

        private void VerifyUniqueness(List<List<string>> circuits)
        {
            var circuitsStrings = circuits.Select(
                p => p.Aggregate("", (agg, y) => $"{agg}{y}")
            ).Distinct();
            Assert.AreEqual(circuits.Count, circuitsStrings.Count());
        }
    }
}
