using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Riddles.Probability.DistributionGenerators;
using Riddles.Probability.ExpectedValue;

namespace Riddles.Optimization
{
	/// <summary>
	/// You want to minimize the variance of rolling 2 dice by weighting the sides of the dice
	/// https://fivethirtyeight.com/features/can-you-catch-the-cricket/
	/// </summary>
	public class DiceWeighter
	{
		private const int _numTimesDiceRolled = 2;
		private Random _random;
		private CumulativeSumOfIndependentTrialsDistributionGenerator _distributionGenerator;
		private VarianceCalculator _varianceCalculator;
		public DiceWeighter(){
			this._random = new Random();
			this._distributionGenerator = new CumulativeSumOfIndependentTrialsDistributionGenerator();
			this._varianceCalculator = new VarianceCalculator();
		}
		public DiceWeighterVariance RunRandomTrials(int numFacesOnDice, int numIterations, int numFittest)
		{
			var populationSize = numFittest * numFittest;
			var population = this.GenerateInitialPopulation(numFacesOnDice, populationSize).ToList();
			DiceWeighterVariance mostFitInidividual = this.CalculateFitness(new List<double[]> { population[0] }).First();
			for(int i=0; i<numIterations; i++)
			{
				var populationFitness = this.CalculateFitness(population).OrderBy(v => v.Variance).ToArray();
				Console.WriteLine($"Iteration {i}:");
				Console.WriteLine($"Most Fit: {populationFitness[0].Variance}");
				Console.WriteLine($"Average Fitness: {populationFitness.Average(p => p.Variance)}");
				Console.WriteLine($"Least Fit: {populationFitness[populationSize - 1].Variance}");

				var mostFit = populationFitness.Take(numFittest).ToList();
				if (mostFit[0].Variance < mostFitInidividual.Variance) {
					mostFitInidividual = mostFit[0];
				}
				population = this.MutatePopulation(mostFit.Select(p => p.Weights).ToList()).ToList();
			}
			return mostFitInidividual;
		}

		private IEnumerable<double[]> GenerateInitialPopulation(int numFacesOnDice, int populationSize)
		{
			for (int individual = 0; individual < populationSize; individual++)
			{
				double[] randomSegments;
				if (numFacesOnDice % 2 == 1) {
					randomSegments = new List<double> { 0, 1 }.Concat(
						Enumerable.Range(0, numFacesOnDice - 1).Select(x => this._random.NextDouble())
					).OrderBy(x => x).ToArray();
				} else
				{
					var numRandomValuesToGenerate = (numFacesOnDice / 2) - 1;
					randomSegments = new List<double> { 0, 0.5, 1 }.ToArray();
					for (int i = 0; i < numRandomValuesToGenerate; i++)
					{
						var randomValue = this._random.NextDouble() * 0.5;
						var mirroredValue = 1 - randomValue;
						randomSegments = randomSegments.Concat(new List<double> { randomValue, mirroredValue }).ToArray();
					}
					randomSegments = randomSegments.OrderBy(b => b).ToArray();
				}					
				double[] weights = new double[numFacesOnDice];
				for (int i = 0; i < weights.Length; i++)
				{
					weights[i] = randomSegments[i + 1] - randomSegments[i];
				}
				yield return weights;
			}
		}

		private IEnumerable<double[]> MutatePopulation(List<double[]> initialPopulation)
		{
			foreach(var prob1 in initialPopulation)
			{
				foreach(var prob2 in initialPopulation)
				{
					double[] newProbabilities = new double[prob1.Length];
					for(int i=0; i<prob1.Length; i++)
					{
						newProbabilities[i] = (prob1[i] + prob2[i]) / 2;						
					}
					yield return newProbabilities;
				}
			}
		}

		public IEnumerable<DiceWeighterVariance> CalculateFitness(IEnumerable<double[]> population)
		{
			foreach (var individual in population)
			{
				var weightDictionary = individual.Select((v, i) => new { probability = v, value = i + 1 })
					.ToDictionary(w => w.value, w => w.probability);
				var distribution = this._distributionGenerator
					.GetOutcomeProbabilityDistribution(weightDictionary, _numTimesDiceRolled);
				var variance = this._varianceCalculator.CalculateVarianceOfProbabilityDistribution(distribution.Values.ToArray());
				yield return new DiceWeighterVariance
				{
					Weights = individual,
					OutcomeDistribution = distribution,
					Variance = variance
				};
			}
		}

		public class DiceWeighterVariance {
			public double[] Weights { get; set; }
			public Dictionary<int, double> OutcomeDistribution { get; set; }
			public double? Variance { get; set; }
		}
	}
}
