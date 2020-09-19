using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Probability;

namespace Riddles.Tests.Probability
{
	public class CentrifugeBalancerTest
	{
		[TestCase(4, 0, 1, Description = "trivially true")]
		[TestCase(4, 2, 2, Description = "2 valid configurations")]
		[TestCase(4, 4, 1, Description = "Only 1 configuration, but it's valid")]
		[TestCase(12, 2, 6, Description = "Only 6 distinct ways")]
		[TestCase(12, 1, 0, Description = "Impossible")]
		[TestCase(12, 7, 12, Description = "Only 1 valid configuration, but 12 ways to rotate them")]
		public void TestFindConfigurationThatBalancesCentrifuge(int numSlots, int numSlotsFilled, int expectedNumBalancedConfigurations)
		{
			var centrifugeBalancer = new CentrifugeBalancer();
			var balancedResults = centrifugeBalancer.FindConfigurationThatBalancesCentrifuge(numSlots, numSlotsFilled);
			Assert.AreEqual(expectedNumBalancedConfigurations, balancedResults.Count);
		}
	}
}
