using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews
{
	public class BalancedParenthesesSolver
	{
		public int FindLongestBalancedParenthesisLength(string parentheses)
		{
			Stack<int> stackOfLeftParentheses = new Stack<int> { };
			var bestScore = 0;
			for(int i=0; i<parentheses.Length; i++)
			{
				if (parentheses[i] == '(')
				{
					stackOfLeftParentheses.Push(i);
				}
				else if(parentheses[i] == ')')
				{
					if(stackOfLeftParentheses.Count > 0)
					{
						var startingIndex = stackOfLeftParentheses.Pop();
						var score = i - startingIndex + 1;
						if(score > bestScore)
						{
							bestScore = score;
						}
					}
				}
				else { stackOfLeftParentheses = new Stack<int> { }; }
			}
			return bestScore;
		}

		public int FindMatchedParenthesesScore(string parentheses)
		{
			var currentScore = 0;
			Stack<int> scores = new Stack<int> { };
			for(int i=1; i<parentheses.Length; i++)
			{
				if (parentheses[i] == '(')
				{
					if(parentheses[i-1] == '(') {
						scores.Push(currentScore);
						currentScore = 0;
					}					
				}
				if(parentheses[i] == ')')
				{
					if(parentheses[i-1] == '(') {
						currentScore++;
					}
					
					if(parentheses[i-1] == ')'){
						currentScore = currentScore * 2 + scores.Pop();
					}
				}
			}
			return currentScore;
		}
	}
}
