import math

# gives too many false positives to be useful
class SquareSquareSolver:
	def __init__ (self, size):
		self.size = size
		self.validGridSizeContainingNSquares = {}
		
	def solve(self, numSquaresRequested):
		initialSquares = []
		for numSquares in range(1, self.size):
			initialSquares.append(numSquares**2)
		self.validGridSizeContainingNSquares[1] = initialSquares
		
		for numSquares in range(2, numSquaresRequested + 1):
			print('solving: ' + str(numSquares))
			self.solve_internal(numSquares)
			
		print(self.validGridSizeContainingNSquares)
			
	# assumes that you have populated solutions for all n < numSquares
	def solve_internal(self, numSquares):
		solutions = []
		# we're putting together solutions of size n by combining solutions of smaller sizes. Take the case of n = 4
		# any grid that can fit 4 squares is the sum of a grid that fits 1 square and a grid that fits 3 squares as well as a grid that fits 2 squares and 2 squares
		# so, we want to search from 1 to n/2+1 (exclusive) and sum up solutions of i and validGridSizeContainingNSquares - i
		for i in range(1, math.floor(numSquares/2)+1):
			print('trying ' + str(i) + ', ' + str(numSquares - i))
			solutionSet1 = self.validGridSizeContainingNSquares[i]
			solutionSet2 = self.validGridSizeContainingNSquares[numSquares - i]
			for v1 in solutionSet1:
				for v2 in solutionSet2:
					if (v1 + v2) not in solutions and (v1 + v2) <= self.size**2:
						solutions.append(v1 + v2)
					if (v1 + v2) == self.size**2:
						print(str(v1) + ', ' + str(v2))
			solutions.sort()
			self.validGridSizeContainingNSquares[numSquares] = solutions
	
squareSquareSolver = SquareSquareSolver(13)
squareSquareSolver.solve(3)