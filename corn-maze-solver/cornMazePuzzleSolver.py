# solves https://fivethirtyeight.com/features/can-you-break-the-riddler-bank/
class Maze:
	def __init__(self, maze, startingLocation, endingLocation):
		self.maze = maze
		self.startingLocation = startingLocation
		self.endingLocation = endingLocation
		self.solutionCache = {}
		self.solutionCache[startingLocation] = (None, 0)
		
	def print(self):
		print(self.maze)
		
	def solve(self):
		cells = [(self.startingLocation, 0)]
		while(len(cells) != 0):
			currentCellInfo = cells[0]
			cellLocation = currentCellInfo[0]
			distance = currentCellInfo[1]
			cellValue = int(self.maze[cellLocation[0]][cellLocation[1]])
			
			possibleNextLocations = [
				(cellLocation[0]-cellValue, cellLocation[1]),
				(cellLocation[0]+cellValue, cellLocation[1]),
				(cellLocation[0], cellLocation[1]-cellValue),
				(cellLocation[0], cellLocation[1]+cellValue)
			]
			validNextLocations = []
			for nextLocation in possibleNextLocations:
				if(nextLocation[0] >= 0 and nextLocation[0] < len(self.maze) and nextLocation[1] >= 0 and nextLocation[1] < len(self.maze[0])):
					if nextLocation not in self.solutionCache:
						validNextLocations.append(nextLocation)
						
			for location in validNextLocations:
				self.solutionCache[location] = (cellLocation, distance + 1)
				cells.append((location, distance +  1))
			
			cells.pop(0)
			
	def printSolutionForCell(self, location):
		steps = []
		currentLocation = location
		while currentLocation is not None:				
			previousLocation = self.solutionCache[currentLocation][0]
			steps.insert(0, currentLocation)
			currentLocation = previousLocation
		print(str(location) + ': ' + str(self.solutionCache[location][1]) + ', ' + str(steps))
		
	def printFullSolution(self):
		for row in range(0, len(self.maze)):
			for col in range(0, len(self.maze[0])):
				self.printSolutionForCell((row, col))

file = open('cornMazeConfig.txt', 'r')
mazeInitializationMatrix = []
startingLocationString = file.readline().rstrip().split(',')
startingLocation = (int(startingLocationString[0]), int(startingLocationString[1]))
endingLocationString = file.readline().rstrip().split(',')
endingLocation = (int(endingLocationString[0]), int(endingLocationString[1]))

line = file.readline().rstrip()
while line:
	mazeInitializationMatrix.append(list(line))
	line = file.readline().rstrip()
maze = Maze(mazeInitializationMatrix, startingLocation, endingLocation)
maze.solve()
maze.printFullSolution()