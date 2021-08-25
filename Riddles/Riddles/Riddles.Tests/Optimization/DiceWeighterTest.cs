using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Optimization;
using System.Linq;
using NUnit.Framework;

namespace Riddles.Tests.Optimization
{
	public class DiceWeighterTest
	{
		[Test]
		public void WeightDiceOptimally()
		{
			var diceWeighter = new DiceWeighter();
			var numTrials = 10;
			var numFittestIndividuals = 100;
			var mostFitIndividual = diceWeighter.RunRandomTrials(6, numTrials, numFittestIndividuals);
			Console.WriteLine($"Most Fit Individual: {mostFitIndividual.Variance}");
		}
	}
}
