# lottery puzzle: https://fivethirtyeight.com/features/can-you-decode-the-riddler-lottery/
import math

class Factor:
	def __init__(self):
		self.factors = {}		

	# gets prime factors of a value (excludes 1 because it behaves strangely)
	def getPrimeFactors(self, value):
		if value in self.factors:
			return self.factors[value]
		# find first pair of factors for value. if none exist, the value is prime
		# to do this, we check numbers from 2 to sqrt(value). we divide and ensure the remainder is 0
		
		maxPossibleSmallestFactor = math.floor(math.sqrt(value))
		
		for possibleFactor in range (2, maxPossibleSmallestFactor+1):
			if (value % possibleFactor == 0):
				factor1 = possibleFactor
				factor2 = (int) (value / possibleFactor)
				
				factorsForFactor1 = self.getPrimeFactors(factor1)
				factorsForFactor2 = self.getPrimeFactors(factor2)
				primeFactors = factorsForFactor1+factorsForFactor2
				self.factors[value] = primeFactors
				return primeFactors
				
		# if there are no factors, return a list containing the number
		self.factors[value] = [value]
		return [value]
		
	def getDistinctPrimeFactors(self, value):
		return list(set(self.getPrimeFactors(value)))


class LotteryGenerator:
	def generateValidCombinations(self):
		# in this case, each of the 5 people must pick different numbers, each of which multiply to the same number
		
		return None
		
	def generateAllPossibleSelectionsFromList(self, potentialValues, numValuesToChoose, valuesAlreadyChosen):
		potentialSelections = []
		for value in potentialValues:
			# make sure we are selecting values in ascending order to minimize duplication
			if(len(valuesAlreadyChosen) >= 1 and value < valuesAlreadyChosen[len(valuesAlreadyChosen) - 1]):
				continue
		
			newValuesChosen = valuesAlreadyChosen + [value]
			potentialValuesLeft = potentialValues.copy()
			potentialValuesLeft.remove(value)
			if (numValuesToChoose == 1):
				potentialSelections.append(newValuesChosen)
			else:
				possibleSelections = self.generateAllPossibleSelectionsFromList(potentialValuesLeft, numValuesToChoose - 1, newValuesChosen)
				potentialSelections = potentialSelections + possibleSelections
		return potentialSelections
			
minNumber = 2 # we can ignore the edge case of 1. it isn't composite and it doesn't have 2 distinct prime factors so it cannot be chose
maxNumber = 70

factor = Factor()
valuesWithAtLeast2DistinctPrimeFactors = []
for value in range(minNumber, maxNumber + 1):
	primeFactors = factor.getPrimeFactors(value)
	distinctPrimeFactors = factor.getDistinctPrimeFactors(value)
	if(len(distinctPrimeFactors) >= 2):
		valuesWithAtLeast2DistinctPrimeFactors.append(value)
lotteryGenerator = LotteryGenerator()
possibleSelections = lotteryGenerator.generateAllPossibleSelectionsFromList([1, 2, 3, 4, 5], 5, [])
for selection in possibleSelections:
	print(selection)