using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Riddles.Tests.Simulations
{
    public class ArcherySimulatorTest
    {
        [TestCase(1000000)]
        public void CalculateExpectedValue(int numIterations)
		{
            var random = new Random();
            long numPoints = 0;
            for(int i=0; i<numIterations; i++) {
                double bestDistanceFromCenter = 1.1;
                int numArrows = 1;
                double currentArrowDistanceFromCenter = random.NextDouble();
                while(currentArrowDistanceFromCenter < bestDistanceFromCenter)
				{
                    bestDistanceFromCenter = currentArrowDistanceFromCenter;
                    numArrows++;
                    currentArrowDistanceFromCenter = random.NextDouble();
				}
                numPoints += numArrows;
            }
            var expectedValue = numPoints / (double)numIterations;
		}
    }
}
