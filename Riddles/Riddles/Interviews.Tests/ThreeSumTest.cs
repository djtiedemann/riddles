using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Interviews;

namespace Interviews.Tests
{
	public class ThreeSumTest
	{
		[Test]
		public void TestThreeSum()
		{
			var threeSumSolver = new ThreeSum();
			int[] nums = new int[] { -1, 0, 1, 2, -1, -4 };
			var result = threeSumSolver.CalculateThreeSum(nums);
		}
	}
}
