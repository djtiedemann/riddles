using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Probability.Application
{
    public class CityBlockCarPursuitProbabilitySolver
    {
        private Dictionary<(int, int), double> _cache;
        public CityBlockCarPursuitProbabilitySolver()
        {
            _cache = new Dictionary<(int, int), double>();
        }


        public double CalculateOddsOfEscaping(
            int numCityBlocks,
            int sizeOfHeadStart,
            double oddsOfOutpacing,
            double oddsOfReceeding
        )
        {
            if (!_cache.ContainsKey((numCityBlocks, sizeOfHeadStart)))
            {
                if (sizeOfHeadStart <= 0)
                {
                    _cache[(numCityBlocks, sizeOfHeadStart)] = 0.0;
                }
                else if (numCityBlocks < sizeOfHeadStart)
                {
                    _cache[(numCityBlocks, sizeOfHeadStart)] = 1.0;
                }
                else
                {
                    _cache[(numCityBlocks, sizeOfHeadStart)] =
                        oddsOfOutpacing * CalculateOddsOfEscaping(
                            numCityBlocks - 1,
                            sizeOfHeadStart + 1,
                            oddsOfOutpacing,
                            oddsOfReceeding
                        )
                        + (1 - oddsOfOutpacing - oddsOfReceeding) *
                            CalculateOddsOfEscaping(
                                numCityBlocks - 1,
                                sizeOfHeadStart,
                                oddsOfOutpacing,
                                oddsOfReceeding
                            )
                        + oddsOfReceeding * CalculateOddsOfEscaping(
                                numCityBlocks - 1,
                                sizeOfHeadStart - 1,
                                oddsOfOutpacing,
                                oddsOfReceeding
                            );
                }
            }
            return _cache[(numCityBlocks, sizeOfHeadStart)];
        }
    }
}
