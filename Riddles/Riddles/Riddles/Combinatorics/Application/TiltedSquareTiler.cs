using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Combinatorics.Application
{
    // https://thefiddler.substack.com/p/how-many-ways-can-you-tile-the-tilted
    public class TiltedSquareTiler
    {
        private Dictionary<int, float> _numWaysToTileStepsCache;
        private Dictionary<int, float> _numWaysToTilePyramidCache;
        private Dictionary<int, float> _numWaysToTileTiltedSquareCache;
        public TiltedSquareTiler()
        {
            this._numWaysToTileStepsCache = new Dictionary<int, float>();
            this._numWaysToTilePyramidCache = new Dictionary<int, float>();
            this._numWaysToTileTiltedSquareCache = new Dictionary<int, float>();
        }

        public float CountNumberOfUniqueWaysToTileTiltedSquare(int distanceFromOrigin)
        {
            if (!this._numWaysToTileTiltedSquareCache
                .ContainsKey(distanceFromOrigin))
            {
                if(distanceFromOrigin <= 0)
                {
                    this._numWaysToTileTiltedSquareCache[distanceFromOrigin] 
                        = 0;
                }
                else if(distanceFromOrigin == 1)
                {
                    this._numWaysToTileTiltedSquareCache[distanceFromOrigin]
                        = 1;
                }
                else
                {
                    var numWaysToTile = 2 * 
                        this.CountNumberOfUniqueWaysToTilePyramid(
                            distanceFromOrigin - 1
                        ) * this.CountNumberOfUniqueWaysToTilePyramid(
                            distanceFromOrigin - 1
                        );
                    for(int i=1; i<distanceFromOrigin - 1; i++)
                    {
                        numWaysToTile +=
                            this.CountNumberOfUniqueWaysToTilePyramid(
                                i
                            ) *
                            this.CountNumberOfUniqueWaysToTilePyramid(
                                i
                            ) *
                            this.CountNumberOfUniqueWaysToTilePyramid(
                                distanceFromOrigin - 1 - i
                            ) *
                            this.CountNumberOfUniqueWaysToTilePyramid(
                                distanceFromOrigin - 1 - i
                            );
                    }
                    this._numWaysToTileTiltedSquareCache[distanceFromOrigin] = 
                        numWaysToTile;
                }
            }
            return this._numWaysToTileTiltedSquareCache[distanceFromOrigin];   
        }

        public float CountNumberOfUniqueWaysToTilePyramid(int height)
        {
            if (!this._numWaysToTilePyramidCache.ContainsKey(height))
            {
                if(height <= 0)
                {
                    this._numWaysToTilePyramidCache[height] = 0;
                }
                else if (height == 1)
                {
                    this._numWaysToTilePyramidCache[height] = 1;
                }
                else
                {
                    // if you pick the middle of the pyramid
                    // you split it into two sets of steps
                    float numWaysToTilePyramid = 
                        this.CountNumberOfUniqueWaysToTileSteps(height - 1)
                        * this.CountNumberOfUniqueWaysToTileSteps(height - 1);

                    // if you pick the bottom row, then it's a smaller
                    // pyramid
                    numWaysToTilePyramid +=
                        this.CountNumberOfUniqueWaysToTilePyramid(height - 1);

                    // if you pick anything else, it's 2 steps and 1 pyramid
                    for(int row = 1; row < height - 1; row++)
                    {
                        numWaysToTilePyramid +=
                            this.CountNumberOfUniqueWaysToTilePyramid(
                                height - 1 - row
                            ) *
                            this.CountNumberOfUniqueWaysToTileSteps(
                                row
                            ) *
                            this.CountNumberOfUniqueWaysToTileSteps(
                                row
                            );
                    }
                    this._numWaysToTilePyramidCache[height] 
                        = numWaysToTilePyramid; ;
                }
            }
            return this._numWaysToTilePyramidCache[height];
        }

        public float CountNumberOfUniqueWaysToTileSteps(int numSteps)
        {
            if (!this._numWaysToTileStepsCache.ContainsKey(numSteps))
            {
                if (numSteps <= 0)
                {
                    this._numWaysToTileStepsCache[numSteps] = 0;
                }
                else if (numSteps == 1)
                {
                    this._numWaysToTileStepsCache[numSteps] = 1;
                }
                else
                {
                    float numWaysToSumSteps = 0;
                    for (int i = 1; i < numSteps - 1; i++)
                    {
                        numWaysToSumSteps
                            += this.CountNumberOfUniqueWaysToTileSteps(i) *
                            this.CountNumberOfUniqueWaysToTileSteps(numSteps - 1 - i);
                    }
                    numWaysToSumSteps += 2 * this.CountNumberOfUniqueWaysToTileSteps(
                        numSteps - 1
                    );
                    this._numWaysToTileStepsCache[numSteps] = numWaysToSumSteps;
                }
            }
            return this._numWaysToTileStepsCache[numSteps];
        }
    }
}
