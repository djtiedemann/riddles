from enum import Enum
import math

# https://fivethirtyeight.com/features/are-you-a-pinball-wizard/
class Direction(Enum):
	CounterClockwise = 1,
	Clockwise = 2
	
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
		
	# initialPoint is the point where the pinball was launched from (either from the initial launch or a collision on the ball)
	# targetPosition is the point along the y = -2 line where the ball will collide with the wall
	# we need to find the slope of that line, invert it to get the bounce, and then find the equation for the resulting line (using the point and the slope)
	# we'll find the intersection between that point and the ball if it exists. If not, it misses the ball and the game is over
	def findCollisionPointOnBall(self, launchPoint, targetPosition):
		slopeOfLineTowardsTargetPosition = self.findSlopeInterceptOfLineFromTwoPoints(launchPoint, targetPosition)[0]
		newSlope = -slopeOfLineTowardsTargetPosition
		yIntercept = self.findInterceptFromPointAndSlope(targetPosition, newSlope)
		collisionPointOnBall = self.findIntersectionOfLineAndUnitCircle(newSlope, yIntercept)
		return collisionPointOnBall
		
	def findCollisionPointOnWall(self, collisionPointOnBall):
		if(collisionPointOnBall[1] > 0):
			return None
			
	def findAngleBetweenVectors(self, vector1, vector2):
		runningMagnitudeVector1 = 0
		runningMagnitudeVector2 = 0
		runningDotProduct = 0
		for i in range(0, len(vector1)):
			runningMagnitudeVector1 = runningMagnitudeVector1 + vector1[i]**2
			runningMagnitudeVector2 = runningMagnitudeVector2 + vector2[i]**2
			runningDotProduct = runningDotProduct + vector1[i]*vector2[i]
		dotProduct = runningDotProduct
		magnitudeVector1 = math.sqrt(runningMagnitudeVector1)
		magnitudeVector2 = math.sqrt(runningMagnitudeVector2)
		cosTheta = dotProduct/(magnitudeVector1*magnitudeVector2)
		return math.acos(cosTheta)
		
	def rotateVectorByAngle(self, vector, theta, direction):
		if(direction == Direction.CounterClockwise):
			return [vector[0]*math.cos(theta) - vector[1]*math.sin(theta), vector[0]*math.sin(theta) + vector[1]*math.cos(theta)]
		return [vector[0]*math.cos(theta) + vector[1]*math.sin(theta), -1*vector[0]*math.sin(theta) + vector[1]*math.cos(theta)]
	
		
	# if you are rotating by theta. x' = x*cos(theta) - y*sin(theta), y' = x*sin(theta) + y*cos(theta)
	# cos(theta) = a*b/||a|| ||b||

# assume ball is at (0, 0) for simplicity. assume radius is 1 for simplicity
pinballSolver = PinballSolver()
# in order to solve, we'll take increments in between the minimum and maximum range where the ball bounces off the wall initially and hits the exact edge of the ball
# the points that we're hitting are (1, 2) and (-1, 2). if we're starting at (2, 2) and bouncing it off the ball once - the point at which this happens must be at the X value
# directly in the middle of the initial X value and the final X value. we know this is true because the ball starts at the same Y value as both sides of the ball
leftSideOfBall = (-1, 0)
rightSideOfBall = (1, 0)
startingPosition = (2, 0)
minX = (startingPosition[0]+leftSideOfBall[0]) / 2
maxX = (startingPosition[0]+rightSideOfBall[0]) / 2
initialTargetPosition = ((minX + maxX)/2, -2)
collisionPoint = pinballSolver.findCollisionPointOnBall(startingPosition, initialTargetPosition)

print(pinballSolver.rotateVectorByAngle([1,0], math.pi/4, Direction.CounterClockwise))