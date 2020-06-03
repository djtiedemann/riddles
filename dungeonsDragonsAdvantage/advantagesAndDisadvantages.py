sumForSingleRoll = 0
sumForAdvantage = 0
sumForDisadvantage = 0
sumForAdvantageOfDisadvantages = 0
sumForDisadvantageOfAdvantages = 0
numCombinations = 20**4
singleRollDistributionVsCheck = [0 for i in range(1,21)]
advantageRollDistributionVsCheck = [0 for i in range(1,21)]
disadvantageRollDistributionVsCheck = [0 for i in range(1,21)]
advantageOfDisadvantagesRollDistributionVsCheck = [0 for i in range(1,21)]
disadvantageOfAdvantagesRollDistributionVsCheck = [0 for i in range(1,21)]
for roll_1 in range(1, 21):
	for roll_2 in range(1, 21):
		for roll_3 in range(1, 21):
			for roll_4 in range(1, 21):
				singleRoll = roll_1
				disadvantageRoll = min(roll_1, roll_2)
				advantageRoll = max(roll_1, roll_2)
				advantageOfDisadvantagesRoll = max(min(roll_1, roll_2), min(roll_3, roll_4))
				disadvantageOfAdvantagesRoll = min(max(roll_1, roll_2), max(roll_3, roll_4))
				
				sumForSingleRoll = sumForSingleRoll + singleRoll
				sumForDisadvantage = sumForDisadvantage + disadvantageRoll
				sumForAdvantage = sumForAdvantage + advantageRoll
				sumForAdvantageOfDisadvantages = sumForAdvantageOfDisadvantages + advantageOfDisadvantagesRoll
				sumForDisadvantageOfAdvantages = sumForDisadvantageOfAdvantages + disadvantageOfAdvantagesRoll
				for check in range(0, 20):
					if(singleRoll >= check + 1):
						singleRollDistributionVsCheck[check] = singleRollDistributionVsCheck[check] + 1
					if(disadvantageRoll >= check + 1):
						disadvantageRollDistributionVsCheck[check] = disadvantageRollDistributionVsCheck[check] + 1
					if(advantageRoll >= check + 1):
						advantageRollDistributionVsCheck[check] = advantageRollDistributionVsCheck[check] + 1
					if(advantageOfDisadvantagesRoll >= check + 1):
						advantageOfDisadvantagesRollDistributionVsCheck[check] = advantageOfDisadvantagesRollDistributionVsCheck[check] + 1
					if(disadvantageOfAdvantagesRoll >= check + 1):
						disadvantageOfAdvantagesRollDistributionVsCheck[check] = disadvantageOfAdvantagesRollDistributionVsCheck[check] + 1
						
for i in range(0, 20):
	singleRollDistributionVsCheck[i] = (singleRollDistributionVsCheck[i] / numCombinations)
	advantageRollDistributionVsCheck[i] = (advantageRollDistributionVsCheck[i] / numCombinations)
	disadvantageRollDistributionVsCheck[i] = (disadvantageRollDistributionVsCheck[i] / numCombinations)
	advantageOfDisadvantagesRollDistributionVsCheck[i] = (advantageOfDisadvantagesRollDistributionVsCheck[i] / numCombinations)
	disadvantageOfAdvantagesRollDistributionVsCheck[i] = (disadvantageOfAdvantagesRollDistributionVsCheck[i] / numCombinations)
				 
expectedValueSingleRoll = sumForSingleRoll / numCombinations
expectedValueAdvantage = sumForAdvantage / numCombinations
expectedValueDisadvantage = sumForDisadvantage / numCombinations
expectedValueAdvantageOfDisadvantages = sumForAdvantageOfDisadvantages / numCombinations
expectedValueDisadvantageOfAdvantages = sumForDisadvantageOfAdvantages / numCombinations

print('single roll - expected value: ' + str(expectedValueSingleRoll))
print('advantage roll - expected value: ' + str(expectedValueAdvantage))
print('disadvantage roll - expected value: ' + str(expectedValueDisadvantage))
print('advantage of disadvantages: ' + str(expectedValueAdvantageOfDisadvantages))
print('disadvantage of advantages: ' + str(expectedValueDisadvantageOfAdvantages))

print('single roll distribution: ' + str(singleRollDistributionVsCheck))
print('advantage roll distribution: ' + str(advantageRollDistributionVsCheck))
print('disadvantage roll distribution: ' + str(disadvantageRollDistributionVsCheck))
print('advantage of disadvantages roll distribution: ' + str(advantageOfDisadvantagesRollDistributionVsCheck))
print('disadvantage of advantages roll distribution: ' + str(disadvantageOfAdvantagesRollDistributionVsCheck))