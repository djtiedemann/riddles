# https://fivethirtyeight.com/features/can-you-connect-the-dots/s
class CityGridSolver:
	def turnNTimes(self, currentDirection, transition, n):
		nextDirection = currentDirection
		for i in range(0, n):
			nextDirection = self.matrixMultiply(nextDirection, transition)
		return nextDirection

	def matrixMultiply(self, currentDirection, transition):
		nextDirection = []
		for i in range (0, len(currentDirection)):
			cumulativeProbability = 0
			for j in range (0, len(currentDirection)):
				cumulativeProbability = cumulativeProbability + transition[i][j]*currentDirection[j]
			nextDirection.append(cumulativeProbability)
		return nextDirection		
# transitionProbabilities[i][j] is the probability that you transition from direction i to direction j
# direction 1 = north
# direction 2 = east
# direction 3 = west
# direction 4 = south			
transitionProbabilities = [
[0, 0.5, 0, 0.5], 
[0.5, 0, 0.5, 0],
[0, 0.5, 0, 0.5],
[0.5, 0, 0.5, 0]]

initialProbabilities = [1, 0, 0, 0]
cityGridSolver = CityGridSolver()
oddsGoingNorthAfter10TurnsWith50TurnProbability = cityGridSolver.turnNTimes(initialProbabilities, transitionProbabilities, 10)[0]
print('part 1: ' + str(oddsGoingNorthAfter10TurnsWith50TurnProbability))

transitionProbabilities2 = [
	[1.0/3.0, 1.0/3.0, 0, 1.0/3.0],
	[1.0/3.0, 1.0/3.0, 1.0/3.0, 0],
	[0, 1.0/3.0, 1.0/3.0, 1.0/3.0],
	[1.0/3.0, 0, 1.0/3.0, 1.0/3.0],
]

initialProbabilities = [1, 0, 0, 0]
oddsAfterGoing10Turns = cityGridSolver.turnNTimes(initialProbabilities, transitionProbabilities2, 10)
print('part 2: ' + str(oddsAfterGoing10Turns))