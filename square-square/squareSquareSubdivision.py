import math
solutionSpace = {}
solutionSpaceList = []
# in order to find out how to break up a square into 
for i in range(1, 14):
	for j in range(1, i+1):
		if(i % j == 0):
			# if j fits evenly into i, then the solution is the number of j*j squares that can be formed
			# for instance, a 3x9 rectange can be split into 3 3x3 squares
			solution = math.floor(i/j)
			solutionSpace[(j, i)] = solution
			solutionSpaceList.append(((j, i), solution))
			continue

		# in the solution, the top left corner must be a part of a square
		# we iterate over all possible sizes of that square, and find the solution of the remaining problem space
		# for simplicity, we're making the assumption that the optimal solution splits the rectangle into the top left square and 2 smaller rectangles to solve for
		# either you can cut across the i=k line, creating 2 rectangle of sizes (i-k, j) and (j-k, k)
		# or you can cut across the j=k line, creating 2 rectangles of sizes (j-k, i) and (i-k, k)
		solution = i*j
		# k is the size of the proposed top left rectange
		# i is the longer edge of the rectangle we're solving for
		# j is the shorter edge of the rectange we're solving for
		for k in range(1, j+1):
			solution1 = i*j
			solution2 = i*j			
			if k == j:
				remainingRectangle = (i-j, j) if i-j < j else (j, i-j)
				solution1 = 1 + solutionSpace[remainingRectangle]
				solution2 = 1 + solutionSpace[remainingRectangle]
				
			else:	
				solution1Rectangle1 = (i-k, j) if i-k < j else (j, i-k)
				solution1Rectangle2 = (j-k, k) if j-k < k else (k, j-k)
				solution2Rectangle1 = (j-k, i) if j-k < i else (i, j-k)
				solution2Rectangle2 = (i-k, k) if i-k < k else (k, i-k)
				solution1 = 1 + solutionSpace[solution1Rectangle1] + solutionSpace[solution1Rectangle2]
				solution2 = 1 + solutionSpace[solution2Rectangle1] + solutionSpace[solution2Rectangle2]
				
			if solution1 < solution:
				solution = solution1
			if solution2 < solution:
				solution = solution2
			
		solutionSpace[(j, i)] = solution
		solutionSpaceList.append(((j, i), solution))

for solution in solutionSpaceList:
	print('(' + str(solution[0][0]) + ',' + str(solution[0][1]) + '): ' + str(solution[1]))
	
	
# the top left corner must be either a (1,1), (2,2), ... (13,13) square
# test each of those divisions to find the one with the smallest number of squares
sizeOfSquare = 13
for i in range(1, sizeOfSquare):
	# break square up into 3 sections
	# one (i,i), one ((13-i), 13), one ((13-i), i)
	# the number of squares in this solution is 1 + solutionSpace[(13-i), 13] + solutionSpace [i, 13-i]
	# the idea is that the top left corner must be a part of a square somewhere between a (1,1) and a (12,12)
	# if you remove that square, you're left with a shape that can be cut along the x = i axis to form 2 rectangles
	# we can find the optimal way to subdivide each of those rectangles and then the solution is the total number of squares.
	#
	# this solution has 2 assumptions i'm having a hard time proving, but seem reasonable
	# 1) for any rectangle, if you take the largest square in the rectangle every time, you'll get an optimal result - this builds the solution map above
	# 2) the optimal solution for the square will not contain a rectangle that goes across the x = i line that we've drawn between the 2 rectangles
	leftCornerSquare = (i, i)
	rightCornerRectangle = (i, 13-i) if i<7 else (13-i, i)
	bottomRectangle = (13-i, 13)
	print('solution for (' + str(leftCornerSquare[0]) + ', ' + str(leftCornerSquare[1]) + ')')
	#print('(' + str(rightCornerRectangle[0]) + ', ' + str(rightCornerRectangle[1]) + ')')
	#print('(' + str(bottomRectangle[0]) + ', ' + str(bottomRectangle[1]) + ')')
	numSquares = 1 + solutionSpace[rightCornerRectangle] + solutionSpace[bottomRectangle]
	print(str(numSquares))