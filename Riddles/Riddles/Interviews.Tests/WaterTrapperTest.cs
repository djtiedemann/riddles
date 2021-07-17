using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Interviews;

namespace Interviews.Tests
{
	public class WaterTrapperTest
	{
		[Test]
		public void TestTrap()
		{
			var height = new int[] { 0, 1, 0, 2, 1, 0, 1, 3, 2, 1, 2, 1 };
			var waterTrapper = new WaterTrapper();
			var waterTrapped = waterTrapper.Trap(height);
		}
	}
}
