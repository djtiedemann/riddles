using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Probability.Application
{
    public class SockMatcherSolver
    {
        // (numSocksOnChair, numEmptySpotsOnChair, numReadyToMatchSocks, numUnreadyToMatchSocks)
        private Dictionary<(int, int, int, int), double> _cache 
            = new Dictionary<(int, int, int, int), double>();

        public double CalculateProbabilityOfMatchingSocksSuccessfully(
            int numPairs, 
            int numSpotsOnChair
        )
        {
            return this.CalculateProbabilityOfMatchingSocksSuccessfully(
                0,
                numSpotsOnChair,
                0,
                numPairs*2
            );
        }

        private double CalculateProbabilityOfMatchingSocksSuccessfully(
            int numSocksOnChair,
            int numEmptySpotsOnChair,
            int numReadyToMatchSocks,
            int numUnreadyToMatchSocks
        )
        {
            if (!this._cache.ContainsKey((
                numSocksOnChair, 
                numEmptySpotsOnChair,
                numReadyToMatchSocks,
                numUnreadyToMatchSocks))){

                // if there are no socks to match, you are successful
                if(numReadyToMatchSocks + numUnreadyToMatchSocks == 0)
                {
                    this._cache[(numSocksOnChair,
                        numEmptySpotsOnChair,
                        numReadyToMatchSocks,
                        numUnreadyToMatchSocks)] = 1;
                }
                else
                {
                    var probabilityOfMatchingASock =
                        (double)numReadyToMatchSocks /
                            (numReadyToMatchSocks + numUnreadyToMatchSocks);
                    var probabilityOfNotMatchingASock =
                        1 - probabilityOfMatchingASock;

                    var oddsOfWinningIfMatchingASock = 
                        numSocksOnChair > 0 && numReadyToMatchSocks > 0 ? 
                            this.CalculateProbabilityOfMatchingSocksSuccessfully(
                                numSocksOnChair - 1,
                                numEmptySpotsOnChair + 1,
                                numReadyToMatchSocks - 1,
                                numUnreadyToMatchSocks
                            ) : 0;

                    var oddsOfWinningIfNotMatchingASock =
                        numEmptySpotsOnChair > 0 && numUnreadyToMatchSocks > 0 ?
                            this.CalculateProbabilityOfMatchingSocksSuccessfully(
                                numSocksOnChair + 1,
                                numEmptySpotsOnChair - 1,
                                numReadyToMatchSocks + 1,
                                numUnreadyToMatchSocks - 2
                            ) : 0;

                    var probability =
                        probabilityOfMatchingASock * oddsOfWinningIfMatchingASock +
                        probabilityOfNotMatchingASock * oddsOfWinningIfNotMatchingASock;

                    this._cache[(numSocksOnChair,
                        numEmptySpotsOnChair,
                        numReadyToMatchSocks,
                        numUnreadyToMatchSocks)] = probability;
                }

            }
            return this._cache[(numSocksOnChair,
                numEmptySpotsOnChair,
                numReadyToMatchSocks,
                numUnreadyToMatchSocks)];
        }
    }
}
