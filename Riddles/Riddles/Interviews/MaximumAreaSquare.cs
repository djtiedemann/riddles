using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Interviews
{
	public class MaximumAreaSquare
	{
		public int FindMaximumAreaSquare(int[,] matrix)
		{
			int[,] largestSquares = new int[matrix.GetLength(0), matrix.GetLength(1)];
			int largestSquareSeen = 0;
			for(int i=0; i<matrix.GetLength(0); i++)
			{
				for(int j=0; j<matrix.GetLength(1); j++)
				{
					if(matrix[i,j] == 0)
					{
						largestSquares[i, j] = 0;
						continue;
					}
					var largestSquareForIJ = 1;
					if(i-1>=0 && j-1>=0)
					{
						var largestSquareToLeft = 1 +  
							new List<int> { largestSquares[i - 1, j], largestSquares[i, j - 1], largestSquares[i - 1, j - 1] }.Min();
						if (largestSquareToLeft > largestSquareForIJ) { largestSquareForIJ = largestSquareToLeft; }
					}
					largestSquares[i, j] = largestSquareForIJ;
					if(largestSquareForIJ > largestSquareSeen) { largestSquareSeen = largestSquareForIJ; }
				}
			}
			return largestSquareSeen;
		}
	}
}
