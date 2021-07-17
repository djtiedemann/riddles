using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Interviews;

namespace Interviews.Tests
{
	public class RotateImageTest
	{
		[Test]
		public void TestRotateImage()
		{
			var imageRotater = new RotateImage();
			//var matrix1 = new int[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
			//imageRotater.Rotate(matrix1);
			var matrix = new int[,] { { 5, 1, 9, 11 }, { 2, 4, 8, 10 }, { 13, 3, 6, 7 }, { 15, 14, 12, 16 } };
			imageRotater.Rotate(matrix);
		}
	}
}
