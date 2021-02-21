using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Simulations;

namespace Riddles.Tests.Simulations
{
	public class JengaTowerSimulator2DTest
	{
		//[TestCase(100_000)] - around 7.11
		public void SimulateJenga(int numIterations)
		{
			var jengaSimulator = new JengaTowerSimulator2D();
			var result = jengaSimulator.Simulate(numIterations);
		}
	}
}
