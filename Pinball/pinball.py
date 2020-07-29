from enum import Enum
import math

# https://fivethirtyeight.com/features/are-you-a-pinball-wizard/
class Direction(Enum):
	CounterClockwise = 1,
	Clockwise = 2
	
class PinballSolver:
	def findOptimalTarget(self, startingPosition, minValue, maxValue, increment):
		target = minValue
		while(target<=maxValue):
			collisionPoints = self.findCollisionPoints(startingPosition, (target, -2))
			print(str(target) + ': ' + str(len(collisionPoints)))
			target = target + increment
			
	def findCollisionPoints(self, positionOffWall, targetLocationOnWall):
		collisionPoints = [targetLocationOnWall]
		nextCollisionPointOnBall = self.findCollisionPointOnBall(positionOffWall, targetLocationOnWall)
		if (nextCollisionPointOnBall is None):
			return collisionPoints
		collisionPoints.append(nextCollisionPointOnBall)
		directionBouncingOffWall = self.findDirectionBallWillBounceWhenHittingBall(targetLocationOnWall, nextCollisionPointOnBall)
		nextCollisionPointOnWall = self.findCollisionPointOnWall(nextCollisionPointOnBall, directionBouncingOffWall)
		if (nextCollisionPointOnWall is None):
			return collisionPoints
		remainingCollisionPoints = self.findCollisionPoints(nextCollisionPointOnBall, nextCollisionPointOnWall)
		collisionPoints.extend(remainingCollisionPoints)
		return collisionPoints

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
		if(x1 is None):
			return None
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
		
	def findPointOnBottomPartOfBall(self, xCoordinate):
		if (xCoordinate**2 > 1):
			return None
		return -1*math.sqrt(1-xCoordinate**2)
		
	# initialPoint is the point where the pinball was launched from (either from the initial launch or a collision on the ball)
	# targetPosition is the point along the y = -2 line where the ball will collide with the wall
	# we need to find the slope of that line, invert it to get the bounce, and then find the equation for the resulting line (using the point and the slope)
	# we'll find the intersection between that point and the ball if it exists. If not, it misses the ball and the game is over
	def findCollisionPointOnBall(self, launchPoint, targetPosition):
		if(launchPoint[0] == targetPosition[0]):
			yCoordinate = self.findPointOnBottomPartOfBall(launchPoint[0])
			if(yCoordinate is None):
				return None
			return (launchPoint[0], yCoordinate)
		slopeOfLineTowardsTargetPosition = self.findSlopeInterceptOfLineFromTwoPoints(launchPoint, targetPosition)[0]
		newSlope = -slopeOfLineTowardsTargetPosition
		yIntercept = self.findInterceptFromPointAndSlope(targetPosition, newSlope)
		collisionPointOnBall = self.findIntersectionOfLineAndUnitCircle(newSlope, yIntercept)
		return collisionPointOnBall
		
	def findCollisionPointOnWall(self, collisionPointOnBall, vectorTravelingAwayFromBall):
		# if it hits the top of the ball, it must bounce upward given that the trajectory will be upward
		if(collisionPointOnBall[1] > 0):
			return None
		# if ball is traveling upwards, it will not re-connect
		if(vectorTravelingAwayFromBall[1] >= 0):
			return None
		if(vectorTravelingAwayFromBall[0] == 0):
			return (collisionPointOnBall[0], -2)
		slope = vectorTravelingAwayFromBall[1]/vectorTravelingAwayFromBall[0]
		yIntercept = self.findInterceptFromPointAndSlope(collisionPointOnBall, slope)
		# we know y = -2, so find x using y = mx+b, x = (y-b)/m
		xValue = (-2-yIntercept)/slope
		return (xValue, -2)
		
			
	# tested, seems good
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
		# need to take into account rounding errors
		if (cosTheta < -1):
			cosTheta = -1
		if (cosTheta > 1):
			cosTheta = 1
		return math.acos(cosTheta)
	
	# tested, seems good
	def rotateVectorByAngle(self, vector, theta, direction):
		if(direction == Direction.CounterClockwise):
			return [vector[0]*math.cos(theta) - vector[1]*math.sin(theta), vector[0]*math.sin(theta) + vector[1]*math.cos(theta)]
		return [vector[0]*math.cos(theta) + vector[1]*math.sin(theta), -1*vector[0]*math.sin(theta) + vector[1]*math.cos(theta)]
	
	# tested, seems good
	def findDirectionToRotate(self, vector1, vector2):
		# if the vector in the opposite direction of the vector of impact (vector 1)
		# and the vector from the center of the circle to the point of impact (vector 2)
		# are on the same side of the x-axis, then if the vector 1 has a more positive slope than vector 2 - then we want to rotate in the clockwise direction
		# otherwise, rotate in the counterclockwise direction
		if(vector1[0]*vector2[0] > 0):
			slopeVector1 = vector1[1]/vector1[0]
			slopeVector2 = vector2[1]/vector2[0]
			if (slopeVector1 > slopeVector2):
				return Direction.Clockwise
			return Direction.CounterClockwise
			
		# if the vectors are in opposite side of the x-axis, then we want to do the opposite
		if(vector1[0]*vector2[0] < 0):
			slopeVector1 = vector1[1]/vector1[0]
			slopeVector2 = vector2[1]/vector2[0]
			if (slopeVector1 > slopeVector2):
				return Direction.CounterClockwise
			return Direction.Clockwise
			
		# if the first vector is along the x-axis and the second vector is in quadrant 3, rotate clockwise. if it's in quadrant 4, rotate counterclockwise
		if(vector1[0] == 0):
			if(vector2[0]<0):
				return Direction.Clockwise
			else:
				return Direction.CounterClockwise
		# if the second vector is along the x-axis, do the opposite as above
		if(vector2[0] == 0):
			if(vector1[0]<0):
				return Direction.CounterClockwise
			else:
				return Direction.Clockwise
		
	def findDirectionBallWillBounceWhenHittingBall(self, collisionPointWithWall, collisionPointOnBall):
		vector1 = [collisionPointWithWall[0]-collisionPointOnBall[0], collisionPointWithWall[1]-collisionPointOnBall[1]]
		vector2 = [collisionPointOnBall[0], collisionPointOnBall[1]]
		angleToRotate = 2*self.findAngleBetweenVectors(vector1, vector2)		
		directionToRotate = self.findDirectionToRotate(vector1, vector2)
		resultingVector = self.rotateVectorByAngle(vector1, angleToRotate, directionToRotate)
		return resultingVector
	
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

print(pinballSolver.findCollisionPoints(startingPosition, (0.8224863249400002, -2)))
# 0.8224863249400002