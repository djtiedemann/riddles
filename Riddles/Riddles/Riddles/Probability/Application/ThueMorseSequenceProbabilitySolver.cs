using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Probability.Application
{
    // https://thefiddler.substack.com/p/can-you-even-the-odds
    // you and your friend are taking turns rolling a dice to see
    // who is the first to roll a 5. What are your odds of winning
    // 1) if you alternate turns
    // 2) if you use a snake draft
    // 3) if you use Thue-Morse
    public class ThueMorseSequenceProbabilitySolver
    {
        public enum Strategy
        {
            Alternating,
            Snake,
            ThueMorse
        }

        public double CalculateOddsOfFirstPersonWinning(
            Strategy strategy,
            double oddsOfSuccess,
            double epsilon
        )
        {
            double oddsOfPlayerOneWinning = 0.0;
            double oddsOfPlayerTwoWinning = 0.0;
            List<bool> playerRollingThueMorse = new List<bool> { true };

            int numRolls = 0;
            List<bool> playersRolling = new List<bool>();
            while(oddsOfPlayerOneWinning + oddsOfPlayerTwoWinning < 
                1.0 - epsilon)
            {
                bool playerRolling = false;
                (oddsOfPlayerOneWinning, oddsOfPlayerTwoWinning, playerRolling) 
                    = this.UpdateOddsForNextRoll(
                        strategy,
                        oddsOfSuccess,
                        oddsOfPlayerOneWinning,
                        oddsOfPlayerTwoWinning,
                        numRolls,
                        playerRollingThueMorse
                    );
                playersRolling.Add( playerRolling );
                numRolls++;
            }
            return oddsOfPlayerOneWinning;
        }

        private (double, double, bool) UpdateOddsForNextRoll(
            Strategy strategy,
            double oddsOfSuccess,
            double cumulativeOddsOfPlayerOneWinning,
            double cumulativeOddsOfPlayerTwoWinning,
            int numPreviousRolls,
            List<bool> playerRollingThueMorse
        )
        {
            double oddsOfWinningThisRoll =
                Math.Pow(1 - oddsOfSuccess, numPreviousRolls) * oddsOfSuccess;
            bool isPlayerOneRolling = strategy
                switch 
                {
                    Strategy.Alternating => 
                        numPreviousRolls % 2 == 0,
                    Strategy.Snake => 
                        numPreviousRolls % 4 == 0
                        || numPreviousRolls %4 == 3,
                    Strategy.ThueMorse => 
                        this.GetPlayerRollingThueMorse(
                            numPreviousRolls,
                            playerRollingThueMorse
                        )
                };
            if (isPlayerOneRolling)
            {
                cumulativeOddsOfPlayerOneWinning += oddsOfWinningThisRoll;
            }
            else
            {
                cumulativeOddsOfPlayerTwoWinning += oddsOfWinningThisRoll;
            }
            return (
                cumulativeOddsOfPlayerOneWinning, 
                cumulativeOddsOfPlayerTwoWinning,
                isPlayerOneRolling
            );
        }

        private bool GetPlayerRollingThueMorse(
            int numPreviousRolls,
            List<bool> playerRollingThueMorse
        )
        {
            if(numPreviousRolls >= playerRollingThueMorse.Count)
            {
                var copy = playerRollingThueMorse
                .Select(x => !x)
                .ToList();
                playerRollingThueMorse.AddRange(copy);
            }
            return playerRollingThueMorse[numPreviousRolls];
        }
    }
}
