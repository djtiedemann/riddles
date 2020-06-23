# https://fivethirtyeight.com/features/can-you-flip-the-magic-coin/
#
# if we're splitting n spheres which have weight proportional to 1^3 + 2^3 + 3^3 + ... + n^3 = w overall weight
# we need to find k sets of weight w/k
#
# we'll start by seeing if there is a subset of size w/k. if so, we will exclude those values and try and find remaining subsets of size w/k
class SphereSolver:
	def checkIfSetOfSpheresCanBeDividedEvenly(self, numSpheres):
		spheres = []
		desiredWeight = 0
		for i in range(1, numSpheres + 1):
			spheres.append(i**3)
			desiredWeight = desiredWeight + i**3
		if desiredWeight % 3 != 0:
			return None
		desiredWeight = desiredWeight / 3
		#print(spheres)
		#print(desiredWeight)
		subset = self.findSubsetOfCertainWeight(spheres, desiredWeight)
		return subset

	def findSubsetOfCertainWeight(self, spheres, desiredWeight):
		subset = []
		for i in range(0, len(spheres)):
			subset.append(0)
		hasNewSubset = True
		while(hasNewSubset):
			#print(subset)
			(subset, hasNewSubset) = self.generateNextSubset(subset)
			if(self.checkIfSubsetOfWeight(spheres, desiredWeight, subset)):
				remainingSubset = self.getRemainingSubset(spheres, subset)
				# need to call this recursively
				return subset
		return None
		
	def checkIfSubsetOfWeight(self, spheres, desiredWeight, subset):
		sum = 0
		for i in range(0, len(subset)):
			if(subset[i] == 1):
				sum = sum + spheres[i]
		#print(spheres)
		#print(subset)
		#print(sum)
		return sum == desiredWeight
		
	def getRemainingSubset(self, spheres, subset):
		remainingSubset = []
		for i in range(0, len(subset)):
			if(subset[i] == 0):
				remainingSubset.append(spheres[i])
		return remainingSubset
		
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
for i in range(3, 25):
	print(str(i) + ': ' + str(solver.checkIfSetOfSpheresCanBeDividedEvenly(i)))
