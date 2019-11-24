# solves https://fivethirtyeight.com/features/can-you-help-dakota-jones-raid-the-lost-arc/
import sys

class StateSolver:
	def __init__(self):
		self.count = 0

	def solve(self, allPossibleStates, hash):
		biggestString = ''
		for state in allPossibleStates:
			newString = self.getBiggestString(state, hash)
			print(newString)
			sys.stdout.flush()
			if(len(newString) > len(biggestString)):
				biggestString = newString
		return biggestString
		
	def getBiggestString(self, previousStates, hash):
		biggestString = previousStates
		mostRecentState = previousStates[-2:]
		possibleNextStates = hash[mostRecentState]
		for nextState in possibleNextStates:
			if(nextState not in previousStates):
				newString = previousStates + nextState[1]
				newBiggestString = self.getBiggestString(newString, hash)				
				if len(newBiggestString) > len(biggestString):
					biggestString = newBiggestString
		return biggestString
		
	def buildHash(self, allPossibleStates):
		hash = {}
		for state in allPossibleStates:
			validPairs = []
			for secondState in allPossibleStates:
				if(secondState[0] == state[1] and secondState != state):
					validPairs.append(secondState)
			hash[state] = validPairs
		#print(hash)
		return hash
					
					
		
file = open('stateInput.txt', 'r')
line = file.readline()
states = []
while line:
	states.append(line.rstrip())
	line = file.readline()
solver = StateSolver()
hash = solver.buildHash(states)
biggestString = solver.solve(states, hash)
print('---')
print(biggestString)

##
solution = 'FMNVIDCALAKSCOHINCTNMPWVARIASDE'