using Riddles.Probability.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Probability
{
    /**
     * A game consists of a certain number of rounds.
     * In each case team A has a certain probability of winning
     * the point. If they don't team B gets the point. In
     * the standard case each team has a 50/50 chance of winning
     * each point.
     * 
     * You know ahead of time that one team has a X% chance of winning.
     * What are the odds the other team pulls off the comeback?
     */
    public class BasketballComebackProbabilityCalculator
    {
        private BernoulliTrialOutcomeDistributionCalculator
            _bernoulliTrialCalculator;
        public BasketballComebackProbabilityCalculator() { 
            this._bernoulliTrialCalculator = 
                new BernoulliTrialOutcomeDistributionCalculator();
        }

        /*
         * Need to determine all of the possible ways a team can reach an
         * X% (say 90% chance) of winning.
         * 
         * To avoid double counting, we look at the first time in the game
         * that team reaches a probability of 90%. We count the number
         * of ways to get to a state where a team has a 90% chance of winning,
         * and then multiply that by the number of possible games after that point.
         * This becomes the denominator
         * 
         * Then we find the number of games won by that team to get the numerator.
         * 
         * Then we sum all the numerators and denominators to properly
         * weight each game and dividing those two numbers gives the final answer
         */
        public double CalculateProbabilityOfComeback(
            int numRounds,
            double probabilityOfWinningRound,
            double chancesOfWinningAtSomePointInGame
        )
        {
           var waysToReachStateWithoutExceedingThreshold =
                new Dictionary<int, Dictionary<(int, int), double>> {
                    { 0, new Dictionary<(int, int), double> {{(0, 0), 1.0 }} } 
                };
            var gamesExceedingThresholdInfo =
                new Dictionary<(int, int), (double, double)> { };
            // using i<numRounds because we need to reach the threshold
            // before the final buzzer
            for(int i=1; i<numRounds; i++)
            {
                var gameStatesFromPreviousRounds =
                    waysToReachStateWithoutExceedingThreshold[i - 1]
                        .Keys;
                waysToReachStateWithoutExceedingThreshold[i] =
                    new Dictionary<(int, int), double>();
                foreach(var gameState in gameStatesFromPreviousRounds)
                {
                    var numWins = gameState.Item1;
                    var numLosses = gameState.Item2;
                    // from this game state, the team could either win or 
                    // lose the next game. Take into account both cases.
                    this.UpdateStateForNewRound(
                        waysToReachStateWithoutExceedingThreshold[i - 1][gameState],
                        numWins + 1,
                        numLosses,
                        numRounds,
                        probabilityOfWinningRound,
                        chancesOfWinningAtSomePointInGame,
                        waysToReachStateWithoutExceedingThreshold,
                        gamesExceedingThresholdInfo
                    );
                    this.UpdateStateForNewRound(
                        waysToReachStateWithoutExceedingThreshold[i - 1][gameState],
                        numWins,
                        numLosses + 1,
                        numRounds,
                        probabilityOfWinningRound,
                        chancesOfWinningAtSomePointInGame,
                        waysToReachStateWithoutExceedingThreshold,
                        gamesExceedingThresholdInfo
                    );
                }
            }

            var numerator = 0.0;
            var denominator = 0.0;
            foreach(var odds in gamesExceedingThresholdInfo.Values)
            {
                numerator += odds.Item1 * odds.Item2;
                denominator += odds.Item1;
            }
            // calculating odds of a comeback, not of a win
            var result = 1.0 - (numerator / denominator);
            return result;
        }

        public void UpdateStateForNewRound(
            double numWaysToReachThisState,
            int numWins,
            int numLosses,
            int numRounds,
            double probabilityOfWinningRound,
            double chancesOfWinningAtSomePointInGame,
            Dictionary<int, Dictionary<(int, int), double>> 
                waysToReachStateWithoutExceedingThreshold,
            Dictionary<(int, int), (double, double)>
                gamesExceedingThresholdInfo
        )
        {
            var numRemainingRounds = numRounds - numLosses - numWins;
            var numNecessarySuccesses = (int)Math.Ceiling(numRounds / 2.0) - numWins;
            var probabilityOfWinning = this._bernoulliTrialCalculator
                .CalculateOddsOfAnOutcomeOfAtLeastKSuccesses(
                    numRemainingRounds,
                    numNecessarySuccesses,
                    probabilityOfWinningRound
                );
            // if the team has exceeded the threshold, calculate the appropriate
            // numerator and denominator, but don't continue past that point.
            // Continuing from this state would lead to double counting
            if(probabilityOfWinning >= chancesOfWinningAtSomePointInGame)
            {
                if(!gamesExceedingThresholdInfo.ContainsKey((numWins, numLosses))){
                    gamesExceedingThresholdInfo[(numWins, numLosses)] = (0, probabilityOfWinning);
                }
                gamesExceedingThresholdInfo[(numWins, numLosses)]
                    = (gamesExceedingThresholdInfo[(numWins, numLosses)].Item1 
                        + numWaysToReachThisState*(Math.Pow(2, numRemainingRounds)),
                    gamesExceedingThresholdInfo[(numWins, numLosses)].Item2);
                    
            }
            else
            {
                if (!waysToReachStateWithoutExceedingThreshold[(numWins + numLosses)]
                    .ContainsKey((numWins, numLosses)))
                {
                    waysToReachStateWithoutExceedingThreshold[numWins + numLosses]
                        [(numWins, numLosses)] = 0;
                }
                waysToReachStateWithoutExceedingThreshold[numWins + numLosses][(numWins, numLosses)]
                    = waysToReachStateWithoutExceedingThreshold[numWins + numLosses][(numWins, numLosses)] + numWaysToReachThisState;
            }
        }
    }
}
