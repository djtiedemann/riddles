import math  

# https://fivethirtyeight.com/features/are-you-a-pinball-wizard/
class PinballSolver:		
	def findSlopeInterceptOfLineFromTwoPoints(self, initialPoint, finalPoint):
		deltaY = finalPoint[1] - initialPoint[1]
		deltaX = finalPoint[0] - initialPoint[0]
		slope = deltaY/deltaX
		yIntercept = self.findInterceptFromPointAndSlope(initialPoint, slope)
		return (slope, yIntercept)
		
	def findInterceptFromPointAndSlope(self, point, slope):
		# y = mx + b
		# b = y - mx
		return point[1] - slope*point[0]
		
	def findIntersectionOfLineAndUnitCircle(self, slope, yIntercept):
		#ball: x^2 + y^2 = 1, y = sqrt(1 - x^2)
		#line: y = mx + b
		
		# mx + b = sqrt(1 - x^2)
		# (mx + b)^2 = (1 - x^2)
		# m^2x^2 + 2mbx + b^2 = 1 - x^2
		# (m^2 + 1)x^2 + 2mbx = b^2 - 1
		
		# solve quadratic forumala with
		a = slope**2 + 1
		b = 2*slope*yIntercept
		c = yIntercept**2-1
		# we know with this problem, we are taking the smaller solution
		(x1, x2) = self.quadraticFormula(a, b, c)
		y1 = slope*x1 + yIntercept
		y2 = slope*x2 + yIntercept
		if(y1 > y2):
			return (x2, y2)
		return (x1, y1)
		
	def quadraticFormula(self, a, b, c):
		# the answer will be imaginary, and we're not interested in imaginary solutions
		if(4*a*c > b**2):
			return (None, None)
			
		minYValue = (-1*b - math.sqrt(b**2 - 4*a*c)) / (2*a)
		maxYValue = (-1*b + math.sqrt(b**2 - 4*a*c)) / (2*a)
		return (minYValue, maxYValue)

# assume ball is at (0, 0) for simplicity. assume radius is 1 for simplicity
	
# in order to solve, we'll take increments in between the minimum and maximum range where the ball bounces off the wall initially and hits the exact edge of the ball
# the points that we're hitting are (1, 2) and (-1, 2). if we're starting at (2, 2) and bouncing it off the ball once - the point at which this happens must be at the X value
# directly in the middle of the initial X value and the final X value. we know this is true because the ball starts at the same Y value as both sides of the ball
minX = -1/2
maxX = 1/2
initialTargetPosition = ((minX + maxX)/2, 0)

pinballSolver = PinballSolver()

point1 = (math.sqrt(3)/2, -1/2)

point2 = (0, -9/6)
(slope, yIntercept) = pinballSolver.findSlopeInterceptOfLineFromTwoPoints(point1, point2)
solutionPoint = pinballSolver.findIntersectionOfLineAndUnitCircle(slope, yIntercept)
print(point1)
print(solutionPoint)
# determine if point1 is on circle
print(point1[0]**2 + point1[1]**2)
# determine if point1 is on line
print(point1[0]*slope + yIntercept)
# determine if solutionPoint is on circle
print(solutionPoint[0]**2 + solutionPoint[1]**2)
# determine if solutionPoint is on line
print(solutionPoint[0]*slope + yIntercept)

