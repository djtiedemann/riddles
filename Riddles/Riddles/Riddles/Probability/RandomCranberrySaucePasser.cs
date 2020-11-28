using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Probability.RandomWalk;

namespace Riddles.Probability
{
	public class RandomCranberrySaucePasser
	{
		private Dictionary<string, double[]> stateToProbabilityDistributionDictionary;
		private RandomWalkCalculator2D randomWalkCalculator;
		// https://fivethirtyeight.com/features/can-you-pass-the-cranberry-sauce/
		public RandomCranberrySaucePasser()
		{
			stateToProbabilityDistributionDictionary = new Dictionary<string, double[]>();
			randomWalkCalculator = new RandomWalkCalculator2D();
		}

		public double[] FindProbabilitiesOfEachPersonBeingLastToReceiveCranberrySauce(int numPeopleAtTable)
		{
			var startingSeatWithCranberrySauce = 0;
			var initialState = new State(
				currentSeat: startingSeatWithCranberrySauce,
				nextSeatWithoutCranberrySauceToLeft: (startingSeatWithCranberrySauce - 1) >= 0 
					? (startingSeatWithCranberrySauce - 1)
					: (startingSeatWithCranberrySauce - 1) + numPeopleAtTable,
				nextSeatWithoutCranberrySauceToRight: (startingSeatWithCranberrySauce + 1) < numPeopleAtTable
					? (startingSeatWithCranberrySauce + 1)
					: (startingSeatWithCranberrySauce + 1) - numPeopleAtTable,
				numSeatsAtTable: numPeopleAtTable
			);
			return this.FindProbabilitiesOfEachPersonBeingLastToReceiveCranberrySauceAtCurrentState(initialState, numPeopleAtTable);
		}

		public double[] FindProbabilitiesOfEachPersonBeingLastToReceiveCranberrySauceAtCurrentState(State currentState, int numSeatsAtTable)
		{
			if (this.stateToProbabilityDistributionDictionary.ContainsKey(currentState.MemoizationKey))
			{
				return this.stateToProbabilityDistributionDictionary[currentState.MemoizationKey];
			}
			var probabilities = new double[numSeatsAtTable];
			if (currentState.IsTermialState)
			{
				// if there is only 1 seat left, the person in that seat has a 100% chance of getting it last				
				probabilities[currentState.NextSeatWithoutCranberrySauceToLeft] = 1.0;
				this.stateToProbabilityDistributionDictionary[currentState.MemoizationKey] = probabilities;
				return probabilities;
			}
			var numSeatsWithCranberrySauce = currentState.NextSeatWithoutCranberrySauceToLeft < currentState.NextSeatWithoutCranberrySauceToRight
				? currentState.NextSeatWithoutCranberrySauceToRight - currentState.NextSeatWithoutCranberrySauceToLeft - 1
				: currentState.NextSeatWithoutCranberrySauceToRight - (currentState.NextSeatWithoutCranberrySauceToLeft - numSeatsAtTable) - 1;
			// for all states, the current seat is either next to the left seat without cranberry sauce or the right seat without cranberry sauce
			// the RandomWalkCalculator2D component calculates the odds we will pass left, so if we're closest to the left seat the odds will be
			// the first index in the array. if it's right we need to subtract that probability from 1
			var isCurrentSeatWithCranberrySauceNextToLeftSeatWithoutCranberrySauce =
				currentState.CurrentSeat > currentState.NextSeatWithoutCranberrySauceToLeft
					? currentState.CurrentSeat - currentState.NextSeatWithoutCranberrySauceToLeft == 1
					: currentState.CurrentSeat - (currentState.NextSeatWithoutCranberrySauceToLeft - numSeatsAtTable) == 1;
			var oddsDistributionBasedOnCurrentNumberOfSeatsWithCranberrySauce
				= this.randomWalkCalculator.GetProbabilityDistributionForARangeOfNSteps(numSeatsWithCranberrySauce);
			var oddsOfPassingLeft = isCurrentSeatWithCranberrySauceNextToLeftSeatWithoutCranberrySauce
				? oddsDistributionBasedOnCurrentNumberOfSeatsWithCranberrySauce[0]
				: 1 - oddsDistributionBasedOnCurrentNumberOfSeatsWithCranberrySauce[0];
			var oddsOfPassingRight = 1 - oddsOfPassingLeft;

			var nextStateIfCranberrySauceIsPassedLeft = currentState.GetNextStatePassingCranberrySauceToLeft();
			var nextStateIfCranberrySauceIsPassedRight = currentState.GetNextStatePassingCranberrySauceToRight();

			var probabilityDistributionForPassingLeft =
				this.FindProbabilitiesOfEachPersonBeingLastToReceiveCranberrySauceAtCurrentState(
					nextStateIfCranberrySauceIsPassedLeft,
					numSeatsAtTable);

			var probabilityDistributionForPassingRight =
				this.FindProbabilitiesOfEachPersonBeingLastToReceiveCranberrySauceAtCurrentState(
					nextStateIfCranberrySauceIsPassedRight,
					numSeatsAtTable);
			
			for(int i=0; i<numSeatsAtTable; i++)
			{
				probabilities[i] = oddsOfPassingLeft * probabilityDistributionForPassingLeft[i]
					+ oddsOfPassingRight * probabilityDistributionForPassingRight[i];
			}
			this.stateToProbabilityDistributionDictionary[currentState.MemoizationKey] = probabilities;
			return probabilities;
		}

		public class State
		{
			public State(int currentSeat, int nextSeatWithoutCranberrySauceToLeft, int nextSeatWithoutCranberrySauceToRight, int numSeatsAtTable)
			{
				this.CurrentSeat = currentSeat;
				this.NextSeatWithoutCranberrySauceToLeft = nextSeatWithoutCranberrySauceToLeft;
				this.NextSeatWithoutCranberrySauceToRight = nextSeatWithoutCranberrySauceToRight;
				this.NumSeatsAtTable = numSeatsAtTable;
			}

			public int CurrentSeat { get; }
			public int NextSeatWithoutCranberrySauceToLeft { get; }
			public int NextSeatWithoutCranberrySauceToRight { get; }
			public int NumSeatsAtTable { get;  }
			public string MemoizationKey { get { 
				return $"{CurrentSeat}:{NextSeatWithoutCranberrySauceToLeft}:{NextSeatWithoutCranberrySauceToRight}:{NumSeatsAtTable}"; 
			} }
			public bool IsTermialState { get { return NextSeatWithoutCranberrySauceToLeft == NextSeatWithoutCranberrySauceToRight; } }
			public State GetNextStatePassingCranberrySauceToLeft()
			{
				if (this.IsTermialState)
				{
					return null;
				}
				return new State(
					currentSeat: this.NextSeatWithoutCranberrySauceToLeft,
					nextSeatWithoutCranberrySauceToLeft: (this.NextSeatWithoutCranberrySauceToLeft - 1) >= 0
						? (this.NextSeatWithoutCranberrySauceToLeft - 1)
						: (this.NextSeatWithoutCranberrySauceToLeft - 1) + this.NumSeatsAtTable,
					nextSeatWithoutCranberrySauceToRight: this.NextSeatWithoutCranberrySauceToRight,
					numSeatsAtTable: this.NumSeatsAtTable
				);
			}
			public State GetNextStatePassingCranberrySauceToRight()
			{
				if (this.IsTermialState)
				{
					return null;
				}
				return new State(
					currentSeat: this.NextSeatWithoutCranberrySauceToRight,
					nextSeatWithoutCranberrySauceToLeft: this.NextSeatWithoutCranberrySauceToLeft,
					nextSeatWithoutCranberrySauceToRight: (this.NextSeatWithoutCranberrySauceToRight + 1) < this.NumSeatsAtTable
						? (this.NextSeatWithoutCranberrySauceToRight + 1)
						: (this.NextSeatWithoutCranberrySauceToRight + 1) - this.NumSeatsAtTable,
					numSeatsAtTable: this.NumSeatsAtTable
				);
			}
		}
	}
}
