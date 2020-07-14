# https://fivethirtyeight.com/features/can-you-make-24/
class TwentyFour:
	orderingOfNumbers = []
	def __init__(self, numbers, signs, parenthesesOrders):
		self.numbers = numbers
		self.signs = signs
		self.parenthesesOrders = parenthesesOrders
		
	def generateAllEquations(self):
		numberOrderings = self.generateNumberOrderings()
		signOrderings = self.generateSignOrderings()
		equations = []
		for signOrderingIndex in range(0, len(signOrderings)):
			for numberOrderingIndex in range(0, len(numberOrderings)):
				for parenthesisIndex in range(0, len(self.parenthesesOrders)):
					signOrdering = signOrderings[signOrderingIndex]
					numberOrdering = numberOrderings[numberOrderingIndex]
					parenthesisOrdering = self.parenthesesOrders[parenthesisIndex]
					nextEquation = self.generatePrintableEquationFromSpecificOrdering(signOrdering, numberOrdering, parenthesisOrdering)
					nextValue = self.evaluateExpression(signOrdering, numberOrdering, parenthesisOrdering)
					if(nextValue == 24):
						print(nextEquation + str(': ') + str(nextValue))
	
	def generatePrintableEquationFromSpecificOrdering(self, signOrdering, numberOrdering, parenthesisOrdering):
		# if this equation is just a variable, return it
		if(len(signOrdering) == 0):
			return str(numberOrdering[0])
		# if there is only one sign, return (V1 S1 V2)
		if(len(signOrdering) == 1):
			return '(' + str(numberOrdering[0]) + ' ' + str(self.signs[signOrdering[0]]) + ' ' + str(numberOrdering[1]) + ')'
		# split it into two lists, one with numbers and signs to the left of the order that we're processing the signs in and one with numbers and signs to the right. 
		# combine them accordingly with (Exp1 SX Exp2)
		signPositionOfOutermostParenthesis = parenthesisOrdering[0]
		# note that the number includes the index of the parenthesis sign, the others do not
		signsInExpressionToLeft = signOrdering[0:signPositionOfOutermostParenthesis]
		numbersInExpressionToLeft = numberOrdering[0:signPositionOfOutermostParenthesis+1]
		parenthesesInExpressionToLeft = list(filter(lambda x: x < signPositionOfOutermostParenthesis, parenthesisOrdering))
		
		signsInExpressionToRight = signOrdering[signPositionOfOutermostParenthesis+1:len(signOrdering)]
		numbersInExpressionToRight = numberOrdering[signPositionOfOutermostParenthesis+1:len(numberOrdering)]
		parenthesesInExpressionToRight = list(filter(lambda x: x > signPositionOfOutermostParenthesis, parenthesisOrdering))
		parenthesesInExpressionToRight = list(map(lambda x: x - (signPositionOfOutermostParenthesis + 1), parenthesesInExpressionToRight))
		
		leftExpression = self.generatePrintableEquationFromSpecificOrdering(signsInExpressionToLeft, numbersInExpressionToLeft, parenthesesInExpressionToLeft)
		rightExpression = self.generatePrintableEquationFromSpecificOrdering(signsInExpressionToRight, numbersInExpressionToRight, parenthesesInExpressionToRight)
		return '(' + leftExpression + ' ' + str(self.signs[signOrdering[signPositionOfOutermostParenthesis]]) + ' ' + rightExpression + ')'
	
	def evaluateExpression(self, signOrdering, numberOrdering, parenthesisOrdering):
		# if this equation is just a variable, return it
		if(len(signOrdering) == 0):
			return numberOrdering[0]
		# if there is only one sign, return (V1 S1 V2)
		if(len(signOrdering) == 1):
			return self.evaluateSimpleExpression(numberOrdering[0], signOrdering[0], numberOrdering[1])
		# split it into two lists, one with numbers and signs to the left of the order that we're processing the signs in and one with numbers and signs to the right. 
		# combine them accordingly with (Exp1 SX Exp2)
		signPositionOfOutermostParenthesis = parenthesisOrdering[0]
		# note that the number includes the index of the parenthesis sign, the others do not
		signsInExpressionToLeft = signOrdering[0:signPositionOfOutermostParenthesis]
		numbersInExpressionToLeft = numberOrdering[0:signPositionOfOutermostParenthesis+1]
		parenthesesInExpressionToLeft = list(filter(lambda x: x < signPositionOfOutermostParenthesis, parenthesisOrdering))
		
		signsInExpressionToRight = signOrdering[signPositionOfOutermostParenthesis+1:len(signOrdering)]
		numbersInExpressionToRight = numberOrdering[signPositionOfOutermostParenthesis+1:len(numberOrdering)]
		parenthesesInExpressionToRight = list(filter(lambda x: x > signPositionOfOutermostParenthesis, parenthesisOrdering))
		parenthesesInExpressionToRight = list(map(lambda x: x - (signPositionOfOutermostParenthesis + 1), parenthesesInExpressionToRight))
		
		leftExpressionValue = self.evaluateExpression(signsInExpressionToLeft, numbersInExpressionToLeft, parenthesesInExpressionToLeft)
		rightExpressionValue = self.evaluateExpression(signsInExpressionToRight, numbersInExpressionToRight, parenthesesInExpressionToRight)
		return self.evaluateSimpleExpression(leftExpressionValue, signOrdering[signPositionOfOutermostParenthesis], rightExpressionValue)
	
	def evaluateSimpleExpression(self, leftValue, sign, rightValue):
		if(leftValue is None or rightValue is None):
			return None
		if (sign == 0):
			return leftValue + rightValue
		if (sign == 1):
			return leftValue - rightValue
		if (sign == 2):
			return leftValue * rightValue
		if (sign == 3):
			if(rightValue == 0):
				return None
			return leftValue / rightValue
		if (sign == 4):
			if (leftValue == 0 and rightValue < 0):
				return None
			return leftValue ** rightValue
		
	# generates all possible ordering of numbers for the variables in 24
	def generateNumberOrderings(self):
		possibleOrderings = self.generateNumberOrderingsInternal(self.numbers)
		return possibleOrderings
	
	def generateNumberOrderingsInternal(self, numbers):
		if(len(numbers) == 1):
			return [numbers]
		possibleSequences = []
		for i in range(0, len(numbers)):
			firstNumberInThisSequence = numbers[i]
			remainingNumbers = numbers[0:i]
			if(i != len(numbers) - 1):
				remainingNumbers.extend(numbers[i+1:len(numbers)])
			
			sequencesForRemainingNumbers = self.generateNumberOrderingsInternal(remainingNumbers)
			for j in range(0, len(sequencesForRemainingNumbers)):
				nextSequence = [firstNumberInThisSequence]
				nextSequence.extend(sequencesForRemainingNumbers[j])
				isDuplicateSequence = False
				for k in range(0, len(possibleSequences)):
					if(self.listEquality(nextSequence, possibleSequences[k])):
						isDuplicateSequence = True
				if not isDuplicateSequence:
					possibleSequences.append(nextSequence)
		return possibleSequences
			
	# generates all possible sign pairings for the equation for 24
	# it does this by starting with [0, 0, 0] and incrementing to [4, 4, 4]
	# the value in each index represents the sign
	def generateSignOrderings(self):
		signOrderings = []
		nextSign = [0]*(len(self.numbers)-1)
		print(nextSign)
		while(nextSign is not None):
			signOrderings.append(nextSign)
			nextSign = self.generateNextSign(nextSign)
		return signOrderings
	
	def generateNextSign(self, currentSign):
		minVal = 0
		maxVal = len(self.signs)-1
		for i in range(len(currentSign)-1, -1, -1):
			if(currentSign[i] != maxVal):
				nextSign = currentSign[0:i]
				nextSign.append(currentSign[i]+1)
				nextSign.extend([minVal]*(len(currentSign) - 1 - i))
				return nextSign
		return None
		
	def listEquality(self, list1, list2):
		if(len(list1) != len(list2)):
			return False
		for i in range(0, len(list1)):
			if(list1[i] != list2[i]):
				return False
		return True

numbers = [3, 3, 4, 2]
numbers.sort()
signs = ['+', '-', '*', '/', '^']
# for parentheses, we're hard-coding the possible orderings. basically, we have the possible equations:
# A S1 B S2 C S3 D
# the parentheses determine how the equation is evaluated. the one tricky thing about parentheses is that evaluating S1, S3, S2 is equivalent to evaluating S3, S2, S1
# all other sequences are unique, but those two sequences map down to (A S1 B) S2 (C S3 D)
# solving the general parenthesis equality problem doesn't seem worthwhile, so we're manually excluding [3, 1, 2] from the list
parenthesisOrders = [[0, 1, 2], [0, 2, 1], [1, 0, 2], [1, 2, 0], [2, 0, 1], [2, 1, 0]]
twentyFourSolver = TwentyFour(numbers, signs, parenthesisOrders)
#print(twentyFourSolver.generateSignOrderings())
#print(twentyFourSolver.generateNumberOrderings())

twentyFourSolver.generateAllEquations()
# expected equation = ((4 * 2) ^ 3) / 2
#print(twentyFourSolver.generatePrintableEquationFromSpecificOrdering([2, 4, 3], [4, 2, 3, 2], [2, 1, 0]))
#print(twentyFourSolver.evaluateExpression([2, 4, 3], [4, 2, 3, 2], [2, 1, 0]))