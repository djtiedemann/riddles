import random
from enum import Enum
class Color(Enum):
	Red = 1,
	Green = 2,
	Blue = 3,
	Yellow = 4

class ColoredBallSimulation:
	def __init__(self):
		self.randomSelectionTwoBalls = {1: [0, 1], 2: [0, 2], 3: [0, 3],
			4: [1, 0], 5: [1, 2], 6: [1, 3],
			7: [2, 0], 8: [2, 1], 9: [2, 3],
			10: [3, 0], 11: [3, 1], 12: [3, 2]}
		
	def runSimulation(self):
		self.balls = [Color.Red, Color.Green, Color.Blue, Color.Yellow]
		numSteps = 0
		while(self.isSimulationFinished() == False):
			numSteps = numSteps + 1
			selection = self.randomSelectionTwoBalls[random.randint(1, 12)]
			#print(selection)
			self.balls[selection[1]] = self.balls[selection[0]]
			#print('iteration ' + str(numSteps) + ': ' + str(self.balls))
		return numSteps
		
	def isSimulationFinished(self):
		return self.balls[0] == self.balls[1] and self.balls[1] == self.balls[2] and self.balls[2] == self.balls[3]

	
simulator = ColoredBallSimulation()
numSimulations = 0
numSteps = 0
for i in range(1, 1000000):
	result = simulator.runSimulation()
	numSimulations = numSimulations + 1
	numSteps = numSteps + result
expectedValue = numSteps / numSimulations
print('Expected Value: ' + str(expectedValue))