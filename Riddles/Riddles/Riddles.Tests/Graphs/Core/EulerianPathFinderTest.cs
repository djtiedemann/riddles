using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Graphs.Core;
using NUnit.Framework;
using System.Linq;

namespace Riddles.Tests.Graphs.Core
{
    public class EulerianPathFinderTest
    {
        [Test]
        public void TestGetRockPaperScissorsPaths()
        {
            var rockPaperScissorsAdjacencies =
                new Dictionary<string, HashSet<string>>
                {
                    { "Rock", new HashSet<string> { "Scissors" } },
                    { "Scissors", new HashSet<string> { "Paper" } },
                    { "Paper", new HashSet<string> { "Rock" } },
                };

            var eulerianPathFinder = new EulerianPathFinder();
            var paths = eulerianPathFinder.FindPaths(
                rockPaperScissorsAdjacencies
            );
            this.VerifyUniqueness(paths);
            Assert.AreEqual(3, paths.Count);
        }

        [Test]
        public void TestGetRockPaperScissorsLizardSpockPaths()
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

            var eulerianPathFinder = new EulerianPathFinder();
            var paths = eulerianPathFinder.FindPaths(
                rockPaperScissorsAdjacencies
            );
            this.VerifyUniqueness(paths);
            Assert.AreEqual(110, paths.Count);
        }

        private void VerifyUniqueness(List<List<string>> paths)
        {
            var pathStrings = paths.Select(
                p => p.Aggregate("", (agg, y) => $"{agg}{y}")
            ).Distinct();
            Assert.AreEqual(paths.Count, pathStrings.Count());
        }
    }
}
