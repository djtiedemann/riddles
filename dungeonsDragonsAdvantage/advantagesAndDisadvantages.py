sumForSingleRoll = 0
numCombinations = 20**4
for roll_1 in range(1, 21):
	for roll_2 in range(1, 21):
		for roll_3 in range(1, 21):
			for roll_4 in range(1:21):
				sumForSingleRoll = sumForSingleRoll + roll_1
expectedValueSingleRoll = sumForSingleRoll / numCombinations
print('single roll - expected value: ' + str(expectedValueSingleRoll))