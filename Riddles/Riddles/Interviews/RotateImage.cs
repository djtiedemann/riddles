using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews
{
	public class RotateImage
	{
		public void Rotate(int[,] image)
		{
			for(int i=0; i<image.GetLength(0)/2; i++)
			{
				for(int j=i; j<image.GetLength(0) - 1 - i; j++)
				{
					var temp = image[image.GetLength(0) - 1 - j, i];
					image[image.GetLength(0) - 1 - j, i] = image[image.GetLength(0) - 1 - i, image.GetLength(0) - 1 - j];
					image[image.GetLength(0) - 1 - i, image.GetLength(0) - 1 - j] = image[j, image.GetLength(0) - 1 - i];
					image[j, image.GetLength(0) - 1 - i] = image[i, j];
					image[i, j] = temp;
				}
			}
		}
	}
}
