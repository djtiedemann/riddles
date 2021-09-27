using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Combinatorics.Application;

namespace Riddles.Tests.Combinatorics.Application
{
	public class TiltedStairStackerTest
	{
		[TestCase(1, 1)]
		[TestCase(2, 2)]
		[TestCase(3, 16)]
		[TestCase(4, 768)]
		public void TestGetNumberOfValidWaysToBuildStaircase(int numRows, int expectedNumValidWaysToBuildStaircase)
		{
			var tiltedStaircaseStacker = new TiltedStairStacker();
			var actualNumValidWaysToBuildStaircase = tiltedStaircaseStacker.GetNumberOfValidWaysToBuildStaircase(numRows);
			Assert.AreEqual(expectedNumValidWaysToBuildStaircase, actualNumValidWaysToBuildStaircase);
		}
	}
}
