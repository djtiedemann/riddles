using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews
{
	public class FractionToRecurringDecimal
	{
		public string GetRecurringDecimalRepresentation(int numerator, int denominator)
		{
			var answer = numerator * denominator >= 0 ? "" : "-";
			numerator = Math.Abs(numerator);
			denominator = Math.Abs(denominator);
			if (denominator == 0) { throw new InvalidOperationException("Denominator cannot be 0"); }
			if (numerator == 0) { return "0"; }

			var answerBeforeDecimalPoint = numerator / denominator;
			answer += answerBeforeDecimalPoint.ToString();
			var initialRemainder = numerator % denominator;
			if (initialRemainder == 0) { return answer; }
			answer += ".";
			numerator = numerator - denominator * answerBeforeDecimalPoint;
			numerator = numerator * 10;

			Dictionary<int, int> firstTimeNumeratorSeen = new Dictionary<int, int> { };
			while (true) {
				firstTimeNumeratorSeen[numerator] = answer.Length;
				var nextDigitToAdd = numerator / denominator;
				var nextRemainder = numerator % denominator;
				answer += nextDigitToAdd;
				if (nextRemainder == 0) {
					return answer;
				}
				numerator = numerator - nextDigitToAdd * denominator;
				numerator = numerator * 10;
				if (firstTimeNumeratorSeen.ContainsKey(numerator)) {
					answer = answer.Insert(firstTimeNumeratorSeen[numerator], "(");
					answer += ")";
					return answer;
				}
			}
		}
	}
}
