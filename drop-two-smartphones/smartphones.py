cache = {1: 1, 2: 1, 3: 1} # assume that it doesn't break when dropped from the lowest floor, it does break when dropped from the highest floor
bestFloorCache = {}
numFloors = 1000
for i in range(4, numFloors+1):
	bestFloorToDrop = 1
	bestNumAttempts = i
	floor = 1
	for j in range(2, i):
		numAttempts = max(j-1, 1 + cache[i - j + 1])
		if numAttempts <= bestNumAttempts:
			bestNumAttempts = numAttempts
			bestFloor = j
	cache[i] = bestNumAttempts
	bestFloorCache[i] = bestFloor
print('floor 100: ' + 'should drop on floor ' + str(bestFloorCache[100]) + '. it will take ' + str(cache[100]) + ' total attempts')
print('floor 1000: ' + 'should drop on floor ' + str(bestFloorCache[1000]) + '. it will take ' + str(cache[1000]) + ' total attempts')
	
# smallest number of drops for 100 = 14