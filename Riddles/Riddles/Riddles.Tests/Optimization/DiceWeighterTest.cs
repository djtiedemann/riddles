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
		const double Epsilon = 0.000001;

		[Test]
		public void WeightDiceOptimally()
		{
			var diceWeighter = new DiceWeighter();
			var numTrials = 10;
			var numFittestIndividuals = 100;
			var mostFitIndividual = diceWeighter.RunRandomTrials(6, numTrials, numFittestIndividuals);
			Console.WriteLine($"Most Fit Individual: {mostFitIndividual.Variance}");
		}

		[TestCase(0.244, 0.137, 0.119, 0.001217611)]
		public void TestCalculateFitness(double p1, double p2, double p3, double expectedVariance)
		{
			var diceWeighter = new DiceWeighter();
			var probabilities = new List<double[]> { new double[] { p1, p2, p3, p3, p2, p1 } };
			var varianceInfo = diceWeighter.CalculateFitness(probabilities).ToList();
			Assert.LessOrEqual(Math.Abs(expectedVariance - varianceInfo.Single().Variance.Value), Epsilon);
		}
	}
}
