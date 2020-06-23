# https://fivethirtyeight.com/features/can-you-flip-the-magic-coin/

# the general idea is to break the spheres up into 3 groups and make sure their weights are the same.
# say we have n spheres. we'll create an array of size n that's made up of 1, 2, 3. each index represents the nth sphere and the value represents the grouping:
# so say we have the following
# sphere number: 123456
#                122322
# 
# this represents the following division:
# 1: 1
# 2: 2, 3, 5, 6
# 3: 4
#
# this is a valid solution if 1^3 = 2^3 + 3^3 + 5^3 + 6^3 = 4^3
#
#
# so for each number of spheres, we generate all valid arrays of size n made up of 1,2,3. we then test each of those arrays to see if the combination creates a split where the sum
# is equal. if so, that's the right number
class SphereSolver:
	def divideSpheres(self, numSpheres):
		print('checking: ' + str(numSpheres))
		grouping = []
		for i in range(0, numSpheres):
			grouping.append(1)
		isLastGrouping = False
		while(not isLastGrouping):
			isValid = self.validateGrouping(grouping)
			if(isValid):
				print('winner')
			(grouping, isLastGrouping) = self.generateNextGrouping(grouping)
			
		
	# generate the next valid grouping. basically we search from the right to find the first value that isn't a 3, increment that value by 1 and set all values to the right equal to 1
	# returns false if it isn't the last grouping and true if it is
	def generateNextGrouping(self, grouping):
		#print(grouping)
		# we can always assign the first sphere to group 1 without changing the problem, so we can go to 0 instead of -1
		for index in range(len(grouping) - 1, 0, -1):
			if(grouping[index] == 3):
				continue
			if(grouping[index] == 2):
				grouping[index] = 3
			if(grouping[index] == 1):
				grouping[index] = 2
			for i in range(index+1, len(grouping)):
				grouping[i] = 1
			return (grouping, False)
		return (grouping, True)
		
	# a grouping is valid if all of the spheres in each grouping have the same weight
	def validateGrouping(self, grouping):
		sum1 = 0
		sum2 = 0
		sum3 = 0
		for i in range(0, len(grouping)):
			if(grouping[i] == 1):
				sum1 = sum1 + (i+1)**3
			if(grouping[i] == 2):
				sum2 = sum2 + (i+1)**3
			if(grouping[i] == 3):
				sum3 = sum3 + (i+1)**3
		#print('checking ' + str(grouping))
		#print('sum 1: ' + str(sum1))
		#print('sum 2: ' + str(sum2))
		#print('sum 3: ' + str(sum3))
		return sum1 == sum2 and sum2 == sum3
		


sum = 0
solver = SphereSolver()
for i in range (18, 19):
	# the weight of the spheres is proportional to the volume which is proportional to the radius cubed
	sum = sum + i**3
	if sum % 3 == 0:
		solver.divideSpheres(i)