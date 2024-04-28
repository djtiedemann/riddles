using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Riddles.Probability.Core;
using Riddles.Probability.Application;

namespace Riddles.Tests.Probability.Application
{
    public class DuplicateDeckShufflerAndDividerTest
    {
        private double epsilon = 0.00000000001;

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(52)]
        public void TestCalculateDistributionsWithProbability(int numCards)
        {
            var duplicateDeckShufflerAndDivider =
                new DuplicateDeckShufflerAndDivider();
            var result = duplicateDeckShufflerAndDivider
                .CalculateDistributionsWithProbability(numCards);
            var cumulativeProbability = result.Sum(r => r.Item2);
            Assert.LessOrEqual(Math.Abs(cumulativeProbability - 1), epsilon);
        }
    }
}
