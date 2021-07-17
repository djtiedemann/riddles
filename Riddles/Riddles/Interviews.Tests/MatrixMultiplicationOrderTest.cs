using System;
using System.Collections.Generic;
using System.Text;
using Interviews;
using NUnit.Framework;

namespace Interviews.Tests
{
	public class MatrixMultiplicationOrderTest
	{
		[Test]
		public void TestGetMultiplicationOrder()
		{
			var matrixMultiplicationOrders = new Tuple<int, int>[]
			{
				Tuple.Create(1, 3),
				Tuple.Create(3, 1),
				Tuple.Create(1, 3),
				Tuple.Create(3, 1),
				Tuple.Create(1, 3)
			};
			var matrixMultiplicationOrder = new MatrixMultiplicationOrder();
			var result = matrixMultiplicationOrder.GetMultiplicationOrder(matrixMultiplicationOrders);
		}
	}
}
