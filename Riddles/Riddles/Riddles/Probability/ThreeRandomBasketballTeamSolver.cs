using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.Probability
{
	public class ThreeRandomBasketballTeamSolver
	{
		// we have 3 towns. each town has N people. for each town the probability that i=1, ... n people show up is 1/n (equal distribution)
		// what are the odds that 2 towns can play the 3rd town with both teams having the same number of people
		public ThreeRandomBasketballTeamSolver()
		{

		}


		public double FindProbabilityOfEvenTeamsUsingEqualPopulationsEqualProbabilities(int population)
		{
			var town1Distribution = Enumerable.Range(1, population).Select(numPlayers => new NumPlayersWithProbability
			{
				NumPlayers = numPlayers,
				ProbabiltyThisManyPlayersShowUp = 1.0 / (double)population
			}).ToList();
			var town2Distribution = Enumerable.Range(1, population).Select(numPlayers => new NumPlayersWithProbability
			{
				NumPlayers = numPlayers,
				ProbabiltyThisManyPlayersShowUp = 1.0 / (double)population
			}).ToList();
			var town3Distribution = Enumerable.Range(1, population).Select(numPlayers => new NumPlayersWithProbability
			{
				NumPlayers = numPlayers,
				ProbabiltyThisManyPlayersShowUp = 1.0 / (double)population
			}).ToList();
			return this.FindProbabilityOfEvenTeamsFromDistribution(town1Distribution, town2Distribution, town3Distribution);
		}

		public double FindProbabilityOfEvenTeamsFromDistribution(List<NumPlayersWithProbability> town1Distribution,
			List<NumPlayersWithProbability> town2Distribution,
			List<NumPlayersWithProbability> town3Distribution)
		{
			var cumulativeProbability = 0.0;
			foreach(var town1Outcome in town1Distribution)
			{
				foreach(var town2Outcome in town2Distribution)
				{
					foreach(var town3Outcome in town3Distribution)
					{
						if(town1Outcome.NumPlayers == (town2Outcome.NumPlayers + town3Outcome.NumPlayers) ||
							town2Outcome.NumPlayers == (town1Outcome.NumPlayers + town3Outcome.NumPlayers) ||
							town3Outcome.NumPlayers == (town1Outcome.NumPlayers + town2Outcome.NumPlayers))
						{
							cumulativeProbability += town1Outcome.ProbabiltyThisManyPlayersShowUp * town2Outcome.ProbabiltyThisManyPlayersShowUp
								* town3Outcome.ProbabiltyThisManyPlayersShowUp;
						}
					}
				}
			}
			return cumulativeProbability;
		}
	}

	public class NumPlayersWithProbability
	{
		public int NumPlayers { get; set; }
		public double ProbabiltyThisManyPlayersShowUp { get; set; }
	}
}
