# https://fivethirtyeight.com/features/can-you-best-the-mysterious-man-in-the-trench-coat/
# a person in a trench coat has a number between $1 and $1000 and will give you the money if you guess the number
# you get 9 guesses, after each one, you'll get higher or lower
# this was code enough to get a general pattern
# with n guesses, you are able to guess 2^n-1 options. If there are fewer than 2^n-1 options, you will win the game with certainty and get the expected value of the range
# if there are more than 2^n-1 options, you should focus on the highest of the 2^n-1 options, and you will win (2^n-1)/total amount of the expected value of the top 2^n-1 options
class Trenchcoat:
	def __init__(self):
		self.lookupMatrix = {}
		
	def printLookupMatrix(self):
		print(self.lookupMatrix)
		
	def guess(self, currentMin, currentMax, numGuessesRemainingAfterThisGuess):
		numValuesThatCouldHaveBeenGuessed = (currentMax - currentMin + 1)
		bestExpectedValue = -1.0
		bestGuess = -1.0
		if (currentMin, currentMax, numGuessesRemainingAfterThisGuess) in self.lookupMatrix:
			return self.lookupMatrix[(currentMin, currentMax, numGuessesRemainingAfterThisGuess)]
		for guess in range(currentMin, currentMax+1):
			pGuessIsCorrect = (1.0 if guess >= currentMin and guess <= currentMax else 0.0) / numValuesThatCouldHaveBeenGuessed
			pGuessIsLow = (guess - currentMin) / numValuesThatCouldHaveBeenGuessed if guess >= currentMin and guess <= currentMax else (1.0 if guess < currentMin else 0.0)
			pGuessIsHigh = (currentMax - guess) / numValuesThatCouldHaveBeenGuessed if guess >= currentMin and guess <= currentMax else (0.0 if guess < currentMin else 1.0)
			
			expectedValue = guess * pGuessIsCorrect
			if (numGuessesRemainingAfterThisGuess > 0):
				outcomeForLowGuess = (self.guess(currentMin, guess-1,numGuessesRemainingAfterThisGuess-1)[1] if guess > currentMin else 0) * pGuessIsLow
				outcomeForHighGuess = (self.guess(guess+1, currentMax, numGuessesRemainingAfterThisGuess-1)[1] if guess < currentMax else 0) * pGuessIsHigh
				expectedValue = outcomeForLowGuess + outcomeForHighGuess + expectedValue
			
			if (expectedValue > bestExpectedValue):
				bestExpectedValue = expectedValue
				bestGuess = guess
		self.lookupMatrix[(currentMin, currentMax, numGuessesRemainingAfterThisGuess)] = (bestGuess, bestExpectedValue)
		return (bestGuess, bestExpectedValue)
		
trenchcoat = Trenchcoat()
(bestGuess, bestExpectedValue) = trenchcoat.guess(1, 100, 3)
print(bestGuess)
print(bestExpectedValue)