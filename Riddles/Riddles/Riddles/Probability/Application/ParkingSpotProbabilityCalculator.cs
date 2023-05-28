using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Probability.Application
{
    // https://fivethirtyeight.com/features/can-you-game-the-currency-exchange/
    // Riddler Classic
    public class ParkingSpotProbabilityCalculator
    {
        private Dictionary<(int, int, int), double> _cache; 
        public ParkingSpotProbabilityCalculator()
        {
            this._cache = new Dictionary<(int, int, int), double>();
        }

        public double CalculateProbabilityOfFindingSpot(
            int numSpotsAvailable,
            int numCarsThatWillLeave,
            int numCarsThatWillArrive
        )
        {
            // if no cars arrive, no one will get a spot
            if(numCarsThatWillArrive <= 0)
            {
                return 0;
            }
            var expectedNumberOfSpacesFilledEventually
                = this.CalculateExpectedNumberOfSpacesFilledEventually(
                    numSpotsAvailable,
                    numCarsThatWillLeave,
                    numCarsThatWillArrive
                );
            return expectedNumberOfSpacesFilledEventually / (double)numCarsThatWillArrive;
        }

        private double CalculateExpectedNumberOfSpacesFilledEventually(
            int numSpotsAvailable,
            int numCarsThatWillLeave,
            int numCarsThatWillArrive
        )
        {
            if (!this._cache.ContainsKey((numSpotsAvailable, numCarsThatWillLeave, numCarsThatWillArrive)))
            {
                double cacheValue;
                if (numSpotsAvailable < 0 || numCarsThatWillArrive < 0 || numCarsThatWillLeave < 0)
                {
                    throw new InvalidOperationException("Can't have a negative input value");
                }
                else if(numSpotsAvailable >= numCarsThatWillArrive 
                    || numCarsThatWillLeave == 0)
                {
                    cacheValue = Math.Min(numSpotsAvailable, numCarsThatWillArrive);
                }
                else
                {
                    var numSpotsFilledIfNextCarArrives = 
                        (numSpotsAvailable > 0 ? 1 : 0);
                    var expectedNumSpotsIfNextCarArrives =
                         + numSpotsFilledIfNextCarArrives + 
                         (numCarsThatWillArrive > 0 ? 
                        this.CalculateExpectedNumberOfSpacesFilledEventually(
                           Math.Max(numSpotsAvailable - numSpotsFilledIfNextCarArrives, 0),
                           numCarsThatWillLeave,
                           numCarsThatWillArrive - 1
                        ) : 0);
                    var expectedNumSpotsIfNextCarLeaves =
                        (numCarsThatWillLeave > 0 ?
                            this.CalculateExpectedNumberOfSpacesFilledEventually(
                                numSpotsAvailable + 1,
                                numCarsThatWillLeave - 1,
                                numCarsThatWillArrive
                            ) : 0
                        );
                    var probabilityOfCarArriving = (double)numCarsThatWillArrive / 
                        ((double)numCarsThatWillArrive + (double)numCarsThatWillLeave);
                    var probabilityOfCarLeaving = 1 - probabilityOfCarArriving;
                    cacheValue = probabilityOfCarArriving * expectedNumSpotsIfNextCarArrives
                        + probabilityOfCarLeaving * expectedNumSpotsIfNextCarLeaves;
                }
                this._cache[(numSpotsAvailable, numCarsThatWillLeave, numCarsThatWillArrive)] = cacheValue; ;
            }
            return this._cache[(numSpotsAvailable, numCarsThatWillLeave, numCarsThatWillArrive)];
        }
    }
}
