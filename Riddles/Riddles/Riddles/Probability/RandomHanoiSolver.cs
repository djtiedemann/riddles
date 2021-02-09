using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Riddles.LinearAlgebra;

namespace Riddles.Probability
{
	public class RandomHanoiSolver
	{
		private MatrixUtilities _matrixUtilities;
		public RandomHanoiSolver()
		{
			this._matrixUtilities = new MatrixUtilities();
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
			HashSet<HanoiState> statesAlreadyProcessed = new HashSet<HanoiState>();
			Dictionary<HanoiState, int> termDictionary = new Dictionary<HanoiState, int>();
			termDictionary.Add(initialState, 0);
			Dictionary<HanoiState, List<HanoiStateTransition>> stateTransitionDictionary 
				= new Dictionary<HanoiState, List<HanoiStateTransition>>();
			List<HanoiState> statesToProcess = new List<HanoiState> { initialState };
			
			int nextTermId = 1;
			while(statesToProcess.Count() > 0)
			{
				var currentState = statesToProcess[0];
				statesToProcess.RemoveAt(0);
				if (currentState.IsTerminalState())
				{
					stateTransitionDictionary[currentState] = new List<HanoiStateTransition>();
				}
				else
				{
					var stateTransitions = currentState.GetStateTransitionDictionary();
					stateTransitionDictionary[currentState] = stateTransitions;
					foreach(var stateTransition in stateTransitions)
					{
						if (!termDictionary.ContainsKey(stateTransition.State))
						{
							termDictionary[stateTransition.State] = nextTermId;
							nextTermId++;
							statesToProcess.Add(stateTransition.State);
						}
					}
				}
				statesAlreadyProcessed.Add(currentState);
			}

			List<LinearEquation> linearEquations = new List<LinearEquation>();
			foreach(var state in statesAlreadyProcessed)
			{
				var terms = new List<LinearTerm> { new LinearTerm(1, termDictionary[state]) };
				if (state.IsTerminalState())
				{
					linearEquations.Add(new LinearEquation(terms, 0));
				} 
				else
				{
					foreach(var transitionState in stateTransitionDictionary[state])
					{
						terms.Add(new LinearTerm(transitionState.Probability * -1, termDictionary[transitionState.State]));
					}
					linearEquations.Add(new LinearEquation(terms, 1));
				}
			}
			var expectedValuesForEachState = this._matrixUtilities.SolveLinearSystemOfEquations(linearEquations);
			return expectedValuesForEachState[termDictionary[initialState]];
		}


		public class HanoiState
		{
			public List<int> Tower1 { get; set; }
			public List<int> Tower2 { get; set; }
			public List<int> Tower3 { get; set; }

			public List<HanoiStateTransition> GetStateTransitionDictionary()
			{
				var nextStates = new List<HanoiState>();
				for(int i=0; i<3; i++)
				{
					for(int j=0; j<3; j++)
					{
						if(i == j)
						{
							continue;
						}
						var nextState = this.MoveElementBetweenTowers(new List<List<int>> { Tower1, Tower2, Tower3 }, i, j);
						if(nextState != null)
						{
							nextStates.Add(nextState);
						}
					}
				}
				var numPossibleStateTransitions = nextStates.Count();
				var stateTransitions = nextStates.Select(s =>
					new HanoiStateTransition { State = s, Probability = 1.0 / (double)numPossibleStateTransitions });

				return stateTransitions.GroupBy(s => s.State).Select(g => new HanoiStateTransition {
					State = g.Key,
					Probability = g.Sum(x => x.Probability)
				}).ToList();
			}

			public bool IsTerminalState()
			{
				return Tower1.Count() == 0 && Tower3.Count() == 0;
			}

			private HanoiState MoveElementBetweenTowers(IReadOnlyList<IReadOnlyList<int>> towers, int toIndex, int fromIndex)
			{
				var towersCopy = towers.Select(t => t.ToList()).ToList();
				if(towers[fromIndex].Count() > 0 && towers[fromIndex][0] < towers[toIndex][0]) {
					towersCopy[toIndex].Insert(0, towersCopy[fromIndex][0]);
					towersCopy.RemoveAt(0);
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

			public override bool Equals(object obj)
			{
				if (!(obj is HanoiState))
				{
					return false;
				}
				var otherObj = (HanoiState)obj;
				return this.Tower1.SequenceEqual(otherObj.Tower1)
					&& this.Tower2.SequenceEqual(otherObj.Tower2)
					&& this.Tower3.SequenceEqual(otherObj.Tower3);
			}

			public override int GetHashCode()
			{
				int hash = 17;
				hash = hash * 23 + Tower1.GetHashCode();
				hash = hash * 23 + Tower2.GetHashCode();
				hash = hash * 23 + Tower3.GetHashCode();
				return hash;
			}
		}

		public class HanoiStateTransition{
			public HanoiState State { get; set; }
			public double Probability { get; set; }
		}
	}
}
