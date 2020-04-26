# https://fivethirtyeight.com/features/can-you-best-the-mysterious-man-in-the-trench-coat/
# a person in a trench coat has a number between $1 and $1000 and will give you the money if you guess the number
# you get 9 guesses, after each one, you'll get higher or lower
class Trenchcoat:
	def __init__(self):
		self.lookupMatrix = []
		
	def guess(self, currentMin, currentMax, numGuessesRemainingAfterThisGuess):
		numValuesThatCouldHaveBeenGuessed = (currentMax - currentMin + 1)
		bestExpectedValue = -1.0
		bestGuess = -1.0
		for guess in range(currentMin, currentMax+1):
			pGuessIsCorrect = (1.0 if guess >= currentMin and guess <= currentMax else 0.0) / numValuesThatCouldHaveBeenGuessed
			pGuessIsLow = (guess - currentMin) / numValuesThatCouldHaveBeenGuessed if guess >= currentMin and guess <= currentMax else (1.0 if guess < currentMin else 0.0)
			pGuessIsHigh = (currentMax - guess) / numValuesThatCouldHaveBeenGuessed if guess >= currentMin and guess <= currentMax else (0.0 if guess < currentMin else 1.0)		
			expectedValue = guess * pGuessIsCorrect
			if (expectedValue > bestExpectedValue):
				bestExpectedValue = expectedValue
				bestGuess = guess
		return (bestGuess, bestExpectedValue)
		
trenchcoat = Trenchcoat()
(bestGuess, bestExpectedValue) = trenchcoat.guess(1, 10, 0)
print(bestGuess)
print(bestExpectedValue)