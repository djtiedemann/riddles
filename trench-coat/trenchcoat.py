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
		
	def guess_efficient(self, currentMin, currentMax, numGuesses):
		sizeOfRange = (currentMax - currentMin) + 1
		guessFootprint = 2**(numGuesses) - 1
		probabilityOfCorrectGuess = guessFootprint / sizeOfRange
		minValueInGuessFootprint = currentMax - guessFootprint + 1
		averageValueInGuessFootprint = (currentMax + minValueInGuessFootprint) / 2
		expectedValue = averageValueInGuessFootprint * probabilityOfCorrectGuess
		print('sizeOfRange: ' + str(sizeOfRange))
		print('guessFootprint: ' + str(guessFootprint))
		print('probabilityOfCorrectGuess:' + str(probabilityOfCorrectGuess))
		print('minValueInGuessFootprint:' + str(minValueInGuessFootprint))
		print('averageValueInGuessFootprint:' + str(averageValueInGuessFootprint))
		print('expectedValue:' + str(expectedValue))
		
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
trenchcoat.guess_efficient(1, 1000, 9)