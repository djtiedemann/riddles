using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Riddles.Tests.Simulations
{
	public class ClevernessSimulatorTest
	{
		//[TestCase(0.9, 1.0, 100_000)]
		public void TestSimulation(double clevernessLowerBound, double clevernessUpperBound, int numSimulations) {
			double runningSum = 0;
			for (int i = 0; i < numSimulations; i++) {
				var randomCleverness = clevernessLowerBound + (clevernessUpperBound - clevernessLowerBound) * (new Random()).NextDouble();
				runningSum += Math.Pow(randomCleverness, 9);
			}
			var result = runningSum / (double)numSimulations;
		}
	}
}
