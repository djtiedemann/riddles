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
		private MarkovChainSolver _markovChainSolver;
		public RandomBallPainter()
		{
			this._numBallsToProbabilityDictionary = new Dictionary<int, double>();
			this._markovChainSolver = new MarkovChainSolver();
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
			var initialStateBallDistribution = new Dictionary<int, int> { { 1, numBalls } };
			var initialState = new RandomBallPainterState(initialStateBallDistribution, numBalls);

			return this._markovChainSolver.GetExpectedValueOfNumTurnsToReachTerminalState(initialState, GetStateTransitions);			
		}

		/// <summary>
		/// returns a tuple of (state, probability transition to this state)
		/// </summary>
		/// <returns></returns>
		public Dictionary<RandomBallPainterState, double> GetStateTransitions(RandomBallPainterState state)
		{
			Dictionary<RandomBallPainterState, double> stateProbabilityTransition = new Dictionary<RandomBallPainterState, double>();
			foreach (var key1 in state.BallDistributionForState.Keys)
			{
				var probabilityFirstTypeOfBallDrawn = ((double)key1 * (double)state.BallDistributionForState[key1]) / (double)state.NumBalls;
				foreach (var key2 in state.BallDistributionForState.Keys)
				{
					if (key1 == key2)
					{
						// if we've drawn a second ball that has the same number of balls of the same color, there is a chance that
						// we've drawn the same colored ball twice if we're looking at a color with > 1 ball in it
						var probabilityOfDrawingSameBallTwice = (key1 - 1) / ((double)state.NumBalls - 1);
						var currentStateClone = new RandomBallPainterState(state.DeepCloneBallDistribution(), state.NumBalls);
						stateProbabilityTransition[currentStateClone]
							= (stateProbabilityTransition.ContainsKey(currentStateClone) ? stateProbabilityTransition[currentStateClone] : 0)
							+ probabilityFirstTypeOfBallDrawn * probabilityOfDrawingSameBallTwice;

						if (state.BallDistributionForState[key2] == 1)
						{
							continue;
						}
						// it's also possible we drew a ball of a different color that happened to have the same number of balls of the same color
						// as the first ball that was drawn
						var probabilityOfDrawingDifferentBall
							= (double)key2 * ((double)state.BallDistributionForState[key2] - 1) / ((double)state.NumBalls - 1);
						var nextStateDistribution = state.DeepCloneBallDistribution();
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
						var nextState = new RandomBallPainterState(nextStateDistribution, state.NumBalls);
						stateProbabilityTransition[nextState]
							= (stateProbabilityTransition.ContainsKey(nextState) ? stateProbabilityTransition[nextState] : 0)
							+ probabilityFirstTypeOfBallDrawn * probabilityOfDrawingDifferentBall;
					}
					// if we drew a second type of ball
					else
					{
						var probabilityDrawingSecond
							= ((double)key2 * (double)state.BallDistributionForState[key2]) / ((double)state.NumBalls - 1);
						// in this case, the first ball's and second ball's groups will lose 1
						// the first ball's group + 1 will gain 1 and the second ball's group - 1 will gain 1
						var nextStateDistribution = state.DeepCloneBallDistribution();
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
						var nextState = new RandomBallPainterState(nextStateDistribution, state.NumBalls);
						stateProbabilityTransition[nextState]
							= (stateProbabilityTransition.ContainsKey(nextState) ? stateProbabilityTransition[nextState] : 0)
							+ probabilityFirstTypeOfBallDrawn * probabilityDrawingSecond;
					}
				}
			}
			return stateProbabilityTransition;
		}

		public class RandomBallPainterState : MarkovChainState
		{
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

			public Dictionary<int, int> BallDistributionForState { get { return this._ballDistributionForState; } }			

			public bool IsStateTerminalState()
			{
				// we've reached the final state when the only key is numBalls
				return this.BallDistributionForState.ContainsKey(this.NumBalls);
			}

			public Dictionary<int, int> DeepCloneBallDistribution()
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
