using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Probability
{
	public class SkiSlopeSolver
	{
		public SkiSlopeSolver()
		{

		}

		// https://fivethirtyeight.com/features/can-you-skillfully-ski-the-slopes/
		// You are racing against n opponents. Your times between 2 runs will be summed and whoever has the faster overall time wins
		// You are in first place after the first run. What are the odds that you win.
		//
		// You win against an opponent by either
		// 1) winning against them in the second round
		// 2) losing against them in the second round, but losing by a smaller margin
		//
		// You have an equal chance of finishing in each of the n positions in the second round
		// If you finish in position i, then there were i-1 opponents who beat you in that round.
		// For each opponent who beat you, there's a 50% chance that the time gap was smaller than your victory in the first round
		// That's because each run is independent of each other, and there's an equal probability that the difference will be greater in the first or
		// second run. 
		//
		// You need to beat every opponent to claim victory, so if you finish in position i, your odds of winning are (1/2)^(i-1)
		//
		// So the overall odds of victory are sum from i=1 to n of (1/n)*(1/2)^(i-1)
		public double GetProbabilityOfVictory(int numSkiers)
		{
			var totalProbability = 0.0;
			for(int i=1; i<=numSkiers; i++)
			{
				double probabilityOfWinningInPositionI = Math.Pow(0.5, i - 1);
				totalProbability += (1.0 / (double)numSkiers) * probabilityOfWinningInPositionI;
			}
			return totalProbability;
		}
	}
}
