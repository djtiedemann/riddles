using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Riddles.LinearAlgebra;

namespace Riddles.MarkovChains
{
	public class RandomHanoiSolver
	{
		private MarkovChainSolver _markovChainSolver;
		public RandomHanoiSolver()
		{
			this._markovChainSolver = new MarkovChainSolver();
		}

		public double? GetExpectedNumMovesToSolveTowerOfHanoiRandomly(int numRings)
		{
			if(numRings <= 0)
			{
				throw new InvalidOperationException("There must be a non-zero number of rings");
			}
			var initialState = new HanoiState {
				Tower1 = Enumerable.Range(1, numRings).ToList(),
				Tower2 = new List<int> { },
				Tower3 = new List<int> { }
			};
			return this._markovChainSolver.GetExpectedValueOfNumTurnsToReachTerminalState(initialState, GetStateTransitionDictionary, (object)null);
		}

		public Dictionary<HanoiState, double> GetStateTransitionDictionary(HanoiState state, object additionalArguments)
		{
			var nextStates = new List<HanoiState>();
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					if (i == j)
					{
						continue;
					}
					var nextState = this.MoveElementBetweenTowers(new List<List<int>> { state.Tower1, state.Tower2, state.Tower3 }, i, j);
					if (nextState != null)
					{
						nextStates.Add(nextState);
					}
				}
			}
			var numPossibleStateTransitions = nextStates.Count();
			var stateTransitions = nextStates.Select(s =>
				new HanoiStateTransition { State = s, Probability = 1.0 / (double)numPossibleStateTransitions }).ToList();

			var stateTransitionsGroupings = stateTransitions.GroupBy(s => s.State.GetStringRepresentation()).Select(g => new HanoiStateTransition
			{
				State = g.First().State,
				Probability = g.Sum(x => x.Probability)
			}).ToList();

			return stateTransitionsGroupings.ToDictionary(d => d.State, d => d.Probability);
		}

		private HanoiState MoveElementBetweenTowers(IReadOnlyList<IReadOnlyList<int>> towers, int toIndex, int fromIndex)
		{
			var towersCopy = new List<List<int>> { towers[0].ToList(), towers[1].ToList(), towers[2].ToList() };
			if (towers[fromIndex].Count() > 0 && (towers[toIndex].Count() == 0 || towers[fromIndex][0] < towers[toIndex][0]))
			{
				towersCopy[toIndex].Insert(0, towersCopy[fromIndex][0]);
				towersCopy[fromIndex].RemoveAt(0);
				// in the towers of hanoi problem, final solution doesn't care if the rings are on tower 2 or tower 3
				// so states that are the same if you flip tower 2 and tower 3 are actually the same states.
				// therefore, we can swap tower 2 and tower 3 at any point.
				// we will set the precedent that tower 2 is never empty and it is the tower with the smallest ring on top
				var tower2Index = towersCopy[1].Count() > 0 && (towersCopy[2].Count == 0 || towersCopy[1][0] < towersCopy[2][0])
					? 1 : 2;
				return new HanoiState
				{
					Tower1 = towersCopy[0],
					Tower2 = towersCopy[tower2Index],
					Tower3 = towersCopy[tower2Index == 1 ? 2 : 1]
				};
			}
			return null;
		}

		public class HanoiState : IMarkovChainState
		{
			public List<int> Tower1 { get; set; }
			public List<int> Tower2 { get; set; }
			public List<int> Tower3 { get; set; }

			public bool IsStateTerminalState()
			{
				return Tower1.Count() == 0 && Tower3.Count() == 0;
			}			

			public override bool Equals(object obj)
			{
				if (!(obj is HanoiState))
				{
					return false;
				}
				var otherObj = (HanoiState)obj;
				return this.GetStringRepresentation() == otherObj.GetStringRepresentation();
			}

			public override int GetHashCode()
			{
				return this.GetStringRepresentation().GetHashCode();
			}

			public string GetStringRepresentation()
			{
				var representation = $@"T1:{Tower1.Select(t => t).Aggregate("", (agg, r) => $"{agg}{r}")}," 
					+ $@"T2:{Tower2.Select(t => t).Aggregate("", (agg, r) => $"{agg}{r}")},"
					+ $@"T3:{Tower3.Select(t => t).Aggregate("", (agg, r) => $"{agg}{r}")}";
				return representation;
			}
		}

		public class HanoiStateTransition{
			public HanoiState State { get; set; }
			public double Probability { get; set; }
		}
	}
}
