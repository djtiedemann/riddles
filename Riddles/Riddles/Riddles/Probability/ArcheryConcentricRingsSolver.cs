using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Probability
{
	public class ArcheryConcentricRingsSolver
	{
		public ArcheryConcentricRingsSolver()
		{

		}

		/// <summary>
		/// The idea is that there is a target with n concentric rings. The archer is guaranteed to hit the target, but will hit a random
		/// point on the target, each point with equal probability.
		/// 
		/// The archer fires arrows until they fire one that's not in a ring that's closer to the target than the previous one. What's the
		/// expected number of arrows fired: https://fivethirtyeight.com/features/can-you-cut-the-square-into-more-squares/
		/// </summary>
		/// <param name="numRings"></param>
		/// <returns></returns>
		public double CalculateExpectedValueOfPointsForNRings(int numRings)
		{
			if(numRings <= 0)
			{
				throw new Exception("Expected an integer greater than zero number of rings");
			}
			if(numRings == 1)
			{
				// if there's only 1 ring, the first arrow will hit the target. The second arrow will also hit the target, ending the game
				return 2;
			}
			Dictionary<int, double> probabilityArrowFiredIntoEachRingDictionary = new Dictionary<int, double> { };
			double overallArea = Math.Pow(numRings, 2);
			for(int i=1; i<=numRings; i++)
			{
				// for a ring i, the area of that ring = (i)^2 - (i-1)^2 = 2i - 1
				// we divide that by the total area to get the probability an arrow is fired into that ring
				probabilityArrowFiredIntoEachRingDictionary[i] = (2 * i - 1) / overallArea;
			}

			Dictionary<int, double> ringNumberOfPreviousArrowToRemainingExpectedValueDictionary = new Dictionary<int, double> { };
			// if an arrow is fired within the centermost ring, that must be the last arrow
			ringNumberOfPreviousArrowToRemainingExpectedValueDictionary[1] = 0;
			for(int ring=2; ring<=numRings; ring++)
			{
				double probabilityOfHittingSmallerRing = 0.0;
				double incrementalExpectedValueFromFutureArrows = 0.0;
				for(int smallerRing=1; smallerRing<ring; smallerRing++)
				{
					probabilityOfHittingSmallerRing += probabilityArrowFiredIntoEachRingDictionary[smallerRing];
					incrementalExpectedValueFromFutureArrows +=
						probabilityArrowFiredIntoEachRingDictionary[smallerRing] * ringNumberOfPreviousArrowToRemainingExpectedValueDictionary[smallerRing];
				}
				var expectedScoreFromAdditionalArrows = probabilityOfHittingSmallerRing + incrementalExpectedValueFromFutureArrows;
				ringNumberOfPreviousArrowToRemainingExpectedValueDictionary[ring] = expectedScoreFromAdditionalArrows;
			}

			var runningExpectedValue = 0.0;
			for(int ring=1; ring<=numRings; ring++)
			{
				runningExpectedValue 
					+= probabilityArrowFiredIntoEachRingDictionary[ring] * ringNumberOfPreviousArrowToRemainingExpectedValueDictionary[ring];
			}
			return 2 /* the first two arrows will always be fired */ + runningExpectedValue;
		}
	}
}
