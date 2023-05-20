using NUnit.Framework;
using Riddles.Optimization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Optimization
{
    public class MoneyFromBagOptimizerTest
    {
        private double epsilon = 0.00000000001;

        //[TestCase(1, 5, 100_000_000)]
        // forms recurrence x[i] = x[i-1] + (x[i-1]+1)/2
        public void GetSimulatedNumItemsInBag(int numInitialValues, int numDraws, int numIterations)
        {
            var moneyFromBagOptimizer = new MoneyFromBagOptimizer();
            double sum = 0.0;
            for(int i=0; i<numIterations; i++)
            {
                sum += moneyFromBagOptimizer
                    .GetSimulatedNumberOfValuesInFinalBag(numInitialValues, numDraws);
            }
            var result = sum / numIterations;
        }

        [TestCase(0, 1.0)]
        [TestCase(1, 2.0)]
        [TestCase(2, 3.5)]
        [TestCase(3, 5.75)]
        [TestCase(4, 9.125)]
        [TestCase(5, 14.1875)]
        [TestCase(6, 21.78125)]
        [TestCase(7, 33.171875)]
        [TestCase(8, 50.2578125)]
        public void TestGetExpectedItemsInBagAfterNAdds(int numAdds, double expected)
        {
            var moneyFromBagOptimizer = new MoneyFromBagOptimizer();
            var actual = moneyFromBagOptimizer.GetExpectedItemsInBagAfterNAdds(numAdds);
            Assert.LessOrEqual(Math.Abs(expected - actual) / expected, double.Epsilon);
        }

        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 3)]
        [TestCase(4, 4.5)]
        [TestCase(5, 6.75)]
        [TestCase(6, 10.125)]
        [TestCase(7, 15.1875)]
        [TestCase(8, 22.78125)]
        [TestCase(9, 34.171875)]
        [TestCase(10, 51.2578125)]
        [TestCase(100, 3.613877133646359E+17)]
        public void TestGetMaximumMoneyFromHat(int numDraws, double expected)
        {
            var moneyFromBagOptimizer = new MoneyFromBagOptimizer();
            var actual = moneyFromBagOptimizer.GetMaximumMoneyFromHat(numDraws);
            Assert.LessOrEqual(Math.Abs(expected - actual) / expected, double.Epsilon);
        }
    }
}
