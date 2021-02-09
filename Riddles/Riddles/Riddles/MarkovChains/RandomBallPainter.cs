using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Riddles.LinearAlgebra;

namespace Riddles.MarkovChains
{
	/// <summary>
	/// The idea here is that you have n balls of different colors. Each turn you draw 2 balls. You paint the second ball to be the same color as the first
	/// one. How many turns does it take to paint all of the balls
	/// </summary>
	public class RandomBallPainter
	{
		private Dictionary<int, double> _numBallsToProbabilityDictionary;
		private MatrixUtilities _matrixUtilities;
		public RandomBallPainter()
		{
			this._numBallsToProbabilityDictionary = new Dictionary<int, double>();
			this._matrixUtilities = new MatrixUtilities();
		}

		public double? GetExpectedNumberOfTurnsToPaintBalls(int numBalls)
		{
			if (this._numBallsToProbabilityDictionary.ContainsKey(numBalls))
			{
				return this._numBallsToProbabilityDictionary[numBalls];
			}
			if(numBalls <= 0)
			{
				return null;
			}
			if(numBalls == 1)
			{
				return 0;
			}
			var statesToProcess = new List<RandomBallPainterState>();
			var stateTransitionDictionary = new Dictionary<RandomBallPainterState, Dictionary<RandomBallPainterState, double>>();
			
			var initialStateBallDistribution = new Dictionary<int, int> { { 1, numBalls } };
			var initialState = new RandomBallPainterState(initialStateBallDistribution, numBalls);
			statesToProcess.Add(initialState);

			// while there are states that we haven't yet processed, calculate the transitions for those states
			// add any new states that we've found as states to process
			while(statesToProcess.Any())
			{
				var stateToProcess = statesToProcess.First();
				var stateTransitions = stateToProcess.GetStateTransitions();
				stateTransitionDictionary.Add(stateToProcess, stateTransitions);
				statesToProcess.Remove(stateToProcess);

				foreach(var transitionState in stateTransitions.Keys)
				{
					if(!statesToProcess.Contains(transitionState) && !stateTransitionDictionary.ContainsKey(transitionState))
					{
						statesToProcess.Add(transitionState);
					}
				}
			}

			// once we've calculated all of the state transitions and found all possible states, turn those state transitions into linear equations

			// first assign each state an id which represents the column it will be in the matrix representation
			var termDictionary = new Dictionary<RandomBallPainterState, int>();			
			var nextStateId = 0;
			foreach (var state in stateTransitionDictionary.Keys)
			{
				termDictionary[state] = nextStateId;
				nextStateId++;
			}

			// use those variableIds in order to construct the linear equations
			var equationsToSolve = new List<LinearEquation>();
			var constants = new List<double>();
			foreach(var state in stateTransitionDictionary.Keys)
			{
				// basically a state transition means that 1 turn after the current state we will be in a set of states probabilistically
				// this means that the expected value to finish is 1 turn more than the equation formed by each of the states
				// so we can construct the linear equation:
				// vi = 1 + va*pa + vb*pb + ... +
				// where vi is the state we're looking at and va, vb, ..., are the variables in the state transition
				// if the state is a terminal state, then we know the number of turns left to complete the game is zero, so we can hard-code that equation
				// 
				// if we bring all the variables to the left hand side and the constant to the right hand side we get
				// 1) the coefficient for vi = 1 - pi (the probability we transition to the same state)
				// 2) the coefficient for any variable va that's in the state transition dictionary is -pa
				// 3) the constant coefficient is always 1, expect for the terminal state where it is 0
				if(state.IsStateTerminalState())
				{
					equationsToSolve.Add(new LinearEquation(
						terms: new List<LinearTerm> { new LinearTerm(coefficient: 1, variableId: termDictionary[state]) },
						constant: 0
					));
					continue;
				}

				var terms = new List<LinearTerm>();
				var termForThisState = new LinearTerm(
					coefficient: 1 - (stateTransitionDictionary[state].ContainsKey(state) ? stateTransitionDictionary[state][state] : 0),
					variableId: termDictionary[state]
				);
				terms.Add(termForThisState);

				foreach(var transitionState in stateTransitionDictionary[state].Keys)
				{
					if (!transitionState.Equals(state))
					{
						terms.Add(new LinearTerm(
							coefficient: stateTransitionDictionary[state][transitionState]*-1,
							variableId: termDictionary[transitionState]
						));
					}
				}
				equationsToSolve.Add(new LinearEquation(terms: terms, constant: 1));
			}

			var expectedValuesForEachState = this._matrixUtilities.SolveLinearSystemOfEquations(equationsToSolve);
			return expectedValuesForEachState[termDictionary[initialState]];
		}

		public class RandomBallPainterState {
			/// <summary>
			/// This keeps track of the current state by tracking how many balls there are of similar colors.
			/// The key to this dictionary is the number of balls of the same color, and the value is the number of colors who have that many balls
			/// 
			/// So for instance if there were 4 green balls, 2 red balls, 2 blue balls, 1 yellow ball, 1 purple ball, and 1 black ball, you'd have the
			/// following dictionary:
			/// { 4: 1 /* green */, 2: 2 /* red, blue */, 1: 3 /* yellow, purple, black */ }
			/// 
			/// The initial state for n balls is { 1: n }, and the final state for n balls is { n: 1 }
			/// </summary>
			private readonly Dictionary<int, int> _ballDistributionForState;
			private string _stateKey;
			public RandomBallPainterState(Dictionary<int, int> ballDistributionForState, int numBalls)
			{
				this._ballDistributionForState = ballDistributionForState;
				this.NumBalls = numBalls;
			}	
			
			private Dictionary<int, int> BallDistributionForState { get { return this._ballDistributionForState; } }

			/// <summary>
			/// returns a tuple of (state, probability transition to this state)
			/// </summary>
			/// <returns></returns>
			public Dictionary<RandomBallPainterState, double> GetStateTransitions()
			{
				Dictionary<RandomBallPainterState, double> stateProbabilityTransition = new Dictionary<RandomBallPainterState, double>();
				foreach (var key1 in this.BallDistributionForState.Keys)
				{
					var probabilityFirstTypeOfBallDrawn = ((double)key1 * (double)this.BallDistributionForState[key1]) / (double)this.NumBalls;
					foreach (var key2 in this.BallDistributionForState.Keys)
					{
						if (key1 == key2)
						{
							// if we've drawn a second ball that has the same number of balls of the same color, there is a chance that
							// we've drawn the same colored ball twice if we're looking at a color with > 1 ball in it
							var probabilityOfDrawingSameBallTwice = (key1 - 1) / ((double)this.NumBalls - 1);
							var currentStateClone = new RandomBallPainterState(this.DeepCloneBallDistribution(), this.NumBalls);
							stateProbabilityTransition[currentStateClone]
								= (stateProbabilityTransition.ContainsKey(currentStateClone) ? stateProbabilityTransition[currentStateClone] : 0)
								+ probabilityFirstTypeOfBallDrawn * probabilityOfDrawingSameBallTwice;

							if (this.BallDistributionForState[key2] == 1)
							{
								continue;
							}
							// it's also possible we drew a ball of a different color that happened to have the same number of balls of the same color
							// as the first ball that was drawn
							var probabilityOfDrawingDifferentBall
								= (double)key2 * ((double)this.BallDistributionForState[key2] - 1) / ((double)this.NumBalls - 1);
							var nextStateDistribution = this.DeepCloneBallDistribution();
							// if this happened, the second ball's group will lose 1 ball, and the first ball's group will gain 1 ball
							// so we increment the value for key1+1 by 1, increment the value for key1-1 by 1, decrement the value for key1 by 2
							nextStateDistribution[key1 + 1] = (nextStateDistribution.ContainsKey(key1 + 1) ? nextStateDistribution[key1 + 1] : 0) + 1;
							nextStateDistribution[key1] = nextStateDistribution[key1] - 2;
							// if there are no longer values for this key, remove it
							if (nextStateDistribution[key1] == 0)
							{
								nextStateDistribution.Remove(key1);
							}
							if (key1 - 1 > 0)
							{
								nextStateDistribution[key1 - 1] = (nextStateDistribution.ContainsKey(key1 - 1) ? nextStateDistribution[key1 - 1] : 0) + 1;
							}
							var nextState = new RandomBallPainterState(nextStateDistribution, this.NumBalls);
							stateProbabilityTransition[nextState]
								= (stateProbabilityTransition.ContainsKey(nextState) ? stateProbabilityTransition[nextState] : 0)
								+ probabilityFirstTypeOfBallDrawn * probabilityOfDrawingDifferentBall;
						}
						// if we drew a second type of ball
						else
						{
							var probabilityDrawingSecond
								= ((double)key2 * (double)this.BallDistributionForState[key2]) / ((double)this.NumBalls - 1);
							// in this case, the first ball's and second ball's groups will lose 1
							// the first ball's group + 1 will gain 1 and the second ball's group - 1 will gain 1
							var nextStateDistribution = this.DeepCloneBallDistribution();
							nextStateDistribution[key1] = nextStateDistribution[key1] - 1;
							if (nextStateDistribution[key1] == 0)
							{
								nextStateDistribution.Remove(key1);
							}
							nextStateDistribution[key2] = nextStateDistribution[key2] - 1;
							if (nextStateDistribution[key2] == 0)
							{
								nextStateDistribution.Remove(key2);
							}
							nextStateDistribution[key1 + 1] = (nextStateDistribution.ContainsKey(key1 + 1) ? nextStateDistribution[key1 + 1] : 0) + 1;
							if (key2 - 1 > 0)
							{
								nextStateDistribution[key2 - 1] = (nextStateDistribution.ContainsKey(key2 - 1) ? nextStateDistribution[key2 - 1] : 0) + 1;
							}
							var nextState = new RandomBallPainterState(nextStateDistribution, this.NumBalls);
							stateProbabilityTransition[nextState]
								= (stateProbabilityTransition.ContainsKey(nextState) ? stateProbabilityTransition[nextState] : 0)
								+ probabilityFirstTypeOfBallDrawn * probabilityDrawingSecond;
						}
					}
				}
				return stateProbabilityTransition;
			}

			public bool IsStateTerminalState()
			{
				// we've reached the final state when the only key is numBalls
				return this.BallDistributionForState.ContainsKey(this.NumBalls);
			}

			private Dictionary<int, int> DeepCloneBallDistribution()
			{
				var clone = new Dictionary<int, int>();
				foreach (var key in this.BallDistributionForState.Keys)
				{
					clone[key] = this.BallDistributionForState[key];
				}
				return clone;
			}

			private string StringRepresentation()
			{
				if(this._stateKey == null)
				{
					this._stateKey = 
						$"({this._ballDistributionForState.Keys.OrderBy(k => k).Select(k => $"{k}:{this._ballDistributionForState[k]}").Aggregate((i, j) => $"{i},{j}")})";
				}
				return this._stateKey;
			}

			public override bool Equals(object obj)
			{
				if(!(obj is RandomBallPainterState))
				{
					return false;
				}
				return ((RandomBallPainterState)obj).StringRepresentation() == this.StringRepresentation();
			}

			public override int GetHashCode()
			{
				return this.StringRepresentation().GetHashCode();
			}	
			
			public int NumBalls { get;  }
		}
	}	
}
