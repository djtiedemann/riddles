using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.Probability.DistributionGenerators
{
	public class CumulativeSumOfIndependentTrialsDistributionGenerator
	{
		public CumulativeSumOfIndependentTrialsDistributionGenerator()
		{

		}

		public Dictionary<int, double> GetOutcomeProbabilityDistribution(Dictionary<int, double> outcomes, int numTrials)
		{
			if (numTrials <= 0) { return new Dictionary<int, double>(); }

			Dictionary<int, double> currentOutcomeDistribution = new Dictionary<int, double>();
			foreach(var key in outcomes.Keys)
			{
				currentOutcomeDistribution[key] = outcomes[key];
			}
			for(int i=2; i<=numTrials; i++)
			{
				Dictionary<int, double> nextOutcomeDistribution = new Dictionary<int, double>();
				foreach(var outcome1 in outcomes.Keys)
				{
					foreach(var outcome2 in currentOutcomeDistribution.Keys)
					{
						nextOutcomeDistribution[outcome1 + outcome2] =
							nextOutcomeDistribution.ContainsKey(outcome1 + outcome2)
							? nextOutcomeDistribution[outcome1 + outcome2] + (outcomes[outcome1] * currentOutcomeDistribution[outcome2])
							: outcomes[outcome1] * currentOutcomeDistribution[outcome2];
					}
				}
				currentOutcomeDistribution = nextOutcomeDistribution;
			}
			return currentOutcomeDistribution;
		}
	}
}
