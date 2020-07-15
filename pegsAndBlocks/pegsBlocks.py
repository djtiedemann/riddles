#https://fivethirtyeight.com/features/can-you-make-24/

class PegBlockSolver:	
	def __init__(self, numPegs):
		self.numPegs = numPegs
		
	def solve(self):
		subsets = self.findAllSubsets()
		validPermutations = []
		invalidPermutations = []
		for i in range(0, len(subsets)):
			subset = subsets[i]
			permutations = self.findAllPermutationsOfSubset(subset)
			for j in range(0, len(permutations)):
				permutation = permutations[j]
				if(self.isPermutationValid(permutation) == True):
					validPermutations.append(permutation)
				else:
					invalidPermutations.append(permutation)
		return validPermutations
			
	def isPermutationValid(self, permutation):
		# must have at least 1 block
		if(len(permutation) == 0):
			return False
			
		# a 1 cannot be placed on a space lower than 1, a 2 cannot be placed on a space lower than 2, and so on
		for i in range(0, len(permutation)):
			if(permutation[i] < i):
				return False
		return True
		
		
	def findAllSubsets(self):
		subsets = []
		nextSubset = [0]*self.numPegs
		while (nextSubset is not None):
			subsets.append(nextSubset)
			nextSubset = self.findNextSubset(nextSubset)
		return subsets
	
	def findNextSubset(self, currentSubset):
		for i in range(len(currentSubset)-1, -1, -1):
			if(currentSubset[i] == 0):
				nextSubset = currentSubset[0:i]
				nextSubset.append(1)
				nextSubset.extend([0]*(len(currentSubset)-1-i))
				return nextSubset
		return None
		
	def findAllPermutationsOfSubset(self, subset):
		blocks = []
		for i in range(0, len(subset)):
			if(subset[i] == 1):
				blocks.append(i)
		return self.findAllArrangementsOfBlocks(blocks)
		
	def findAllArrangementsOfBlocks(self, blocks):
		if(len(blocks) <= 1):
			return [blocks]
		arrangements = []
		for i in range(0, len(blocks)):
			blockChosenFirst = blocks[i]
			remainingBlocks = blocks[0:i]
			if(i != len(blocks) -1):
				remainingBlocks.extend(blocks[i+1:len(blocks)])
			arrangementsOfReminaingBlocks = self.findAllArrangementsOfBlocks(remainingBlocks)
			for j in range(0, len(arrangementsOfReminaingBlocks)):
				arrangement = arrangementsOfReminaingBlocks[j]
				nextArrangement = [blockChosenFirst]
				nextArrangement.extend(arrangement)
				arrangements.append(nextArrangement)
		return arrangements
numPegs = 5
pegBlockSolver = PegBlockSolver(numPegs)
solutions = pegBlockSolver.solve()
print(len(solutions))
print(solutions)
#print(pegBlockSolver.findAllPermutationsOfSubset([1, 1, 1, 1, 1]))