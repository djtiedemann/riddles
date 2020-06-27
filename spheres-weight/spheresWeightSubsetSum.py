# https://fivethirtyeight.com/features/can-you-flip-the-magic-coin/
#
# if we're splitting n spheres which have weight proportional to 1^3 + 2^3 + 3^3 + ... + n^3 = w overall weight
# we need to find k sets of weight w/k
#
# we'll start by seeing if there is a subset of size w/k. if so, we will exclude those values and try and find remaining subsets of size w/k
class SphereSolver:
	def checkIfSetOfSpheresCanBeDividedEvenly(self, numSpheres, numPartitions):
		spheres = []
		desiredWeight = 0
		for i in range(1, numSpheres + 1):
			spheres.append(i**3)
			desiredWeight = desiredWeight + i**3
		if desiredWeight % numPartitions != 0:
			return []
		desiredWeight = desiredWeight / numPartitions
		#print(spheres)
		#print(desiredWeight)
		correctSubsets = []
		subset = self.findSubsetOfCertainWeight(spheres, desiredWeight, numPartitions, correctSubsets)
		return correctSubsets

	def findSubsetOfCertainWeight(self, spheres, desiredWeight, numSubsetsNeeded, correctSubsets):
		subset = []
		for i in range(0, len(spheres)):
			subset.append(0)
		hasNewSubset = True
		while(hasNewSubset):
			#print(subset)
			(subset, hasNewSubset) = self.generateNextSubset(subset)
			if(self.checkIfSubsetOfWeight(spheres, desiredWeight, subset)):
				spheresInSubset = self.getSpheresFromSubset(spheres, subset)
				remainingSubset = self.getRemainingSpheres(spheres, subset)
				if(numSubsetsNeeded == 1):
					correctSubsets.append(spheresInSubset)
					return True
				isValidPartition = self.findSubsetOfCertainWeight(remainingSubset, desiredWeight, numSubsetsNeeded - 1, correctSubsets)
				if(isValidPartition):
					correctSubsets.append(spheresInSubset)
					return True
		return False
		
	def checkIfSubsetOfWeight(self, spheres, desiredWeight, subset):
		sum = 0
		for i in range(0, len(subset)):
			if(subset[i] == 1):
				sum = sum + spheres[i]
		#print(spheres)
		#print(subset)
		#print(sum)
		return sum == desiredWeight
		
	def getRemainingSpheres(self, spheres, subset):
		remainingSpheres = []
		for i in range(0, len(subset)):
			if(subset[i] == 0):
				remainingSpheres.append(spheres[i])
		return remainingSpheres
		
	def getSpheresFromSubset(self, spheres, subset):
		spheresInSubset = []
		for i in range(0, len(subset)):
			if(subset[i] == 1):
				spheresInSubset.append(spheres[i])
		return spheresInSubset
		
	# the idea here is that we're generating a bitstring that indiciates which elements are included in the subset
	# given a subset, we generate the next one by finding the rightmost 0, turning it into a 1, and turning all values to the right of that to a 0
	def generateNextSubset(self, subset):
		for index in range(len(subset) - 1, -1, -1):
			if(subset[index] == 1):
				continue
			if(subset[index] == 0):
				subset[index] = 1
			for i in range(index+1, len(subset)):
				subset[i] = 0
			return (subset, True)
		return (subset, False)
		
solver = SphereSolver()
print(solver.checkIfSetOfSpheresCanBeDividedEvenly(23, 3))
