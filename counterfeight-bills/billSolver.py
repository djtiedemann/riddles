# https://fivethirtyeight.com/features/can-you-fool-the-bank-with-your-counterfeit-bills/
# you have 25 real $100 bills, totalling $2,500 dollars. You want to add counterfeit bills to maximize your expected value
# the counterfeight bills are also $100 dollars. The bank will look at 5% of your bills (rounding up).
# If they find a single fake bill, you get nothing. If they look at a counterfeight bill, they have a 25% chance of detecting that it is counterfeight
import math

class Solver:
	def __init__(self):
		self.cache = {}

	def solve(self, numTotalBills, numCounterfeightBills, numBillsToBeDrawn):
		oddsOfCounterfeightBillBeingMissed = .75
	
		cacheKey = (numTotalBills, numCounterfeightBills, numBillsToBeDrawn)
		if(cacheKey in self.cache):
			return self.cache[cacheKey]
		
		probabilityOfCounterfeightBillBeingDrawn = numCounterfeightBills/numTotalBills
		probabilityOfRealBillBeingDrawn = 1-probabilityOfCounterfeightBillBeingDrawn
		if(numBillsToBeDrawn == 0):
			probability = 1
			self.cache[cacheKey] = probability
			return self.cache[cacheKey]
			
		probabilityOfSuccessGivenRealBillChecked = self.solve(numTotalBills-1, numCounterfeightBills, numBillsToBeDrawn-1) * probabilityOfRealBillBeingDrawn
		probabilityOfSuccessGivenCounterfeightBillBeingChecked = oddsOfCounterfeightBillBeingMissed*probabilityOfCounterfeightBillBeingDrawn * self.solve(numTotalBills-1, max(numCounterfeightBills-1, 0), numBillsToBeDrawn-1)
		
		probability = probabilityOfSuccessGivenRealBillChecked + probabilityOfSuccessGivenCounterfeightBillBeingChecked
		self.cache[cacheKey] = probability
		return self.cache[cacheKey]
		
solver = Solver()
numRealBills = 25
ratioOfBillsToCheck = 0.05
numCounterfitBillsToTest = [x for x in range(0,500)]
maxExpectedValue = 0
bestNumCounterfeightBills = 0
bestProbability = 0
for numCounterfitBills in numCounterfitBillsToTest:
	numTotalBills = numRealBills + numCounterfitBills
	numBillsToCheck = math.ceil(numTotalBills*ratioOfBillsToCheck)
	probability = solver.solve(numTotalBills, numCounterfitBills, numBillsToCheck)
	expectedValue = probability*numTotalBills
	if(expectedValue > maxExpectedValue):
		maxExpectedValue = expectedValue
		bestNumCounterfeightBills = numCounterfitBills
		bestProbability = probability
print('Number of counterfit bills: ' + str(bestNumCounterfeightBills) + ", Probability: " + str(bestProbability) + ", " + 'Expected Value: ' + str(maxExpectedValue))
print('Expected gain ' + str((maxExpectedValue - numRealBills)*100))
