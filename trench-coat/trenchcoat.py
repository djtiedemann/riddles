# https://fivethirtyeight.com/features/can-you-best-the-mysterious-man-in-the-trench-coat/
# a person in a trench coat has a number between $1 and $1000 and will give you the money if you guess the number
# you get 9 guesses, after each one, you'll get higher or lower
class Trenchcoat:
	def __init__(self):
		self.lookupMatrix = []
		
	def guess(self, guess, currentMin, currentMax, numGuessesRemainingAfterThisGuess):
		numValuesThatCouldHaveBeenGuessed = (currentMax - currentMin + 1)
		pGuessIsCorrect = 1.0 / numValuesThatCouldHaveBeenGuessed
		pGuessIsLow = (guess - currentMin) / numValuesThatCouldHaveBeenGuessed
		pGuessIsHigh = (currentMax - guess) / numValuesThatCouldHaveBeenGuessed
		
		if (numGuessesRemainingAfterThisGuess == 0):
			return guess * pGuessIsCorrect
		
trenchcoat = Trenchcoat()
expectedValue = trenchcoat.guess(10, 1, 10, 0)
print(expectedValue)
