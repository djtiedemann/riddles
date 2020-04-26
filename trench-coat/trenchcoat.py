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
		print('sample size: ' + str(numValuesThatCouldHaveBeenGuessed))
		print('pGuessIsCorrect: ' + str(pGuessIsCorrect))
		print('pGuessIsLow: ' + str(pGuessIsLow))
		print('pGuessIsHigh: ' + str(pGuessIsHigh))
		
trenchcoat = Trenchcoat()
trenchcoat.guess(4, 1, 10, 0)