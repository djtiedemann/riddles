class MatrixMultiply:
	def matrixMultiply(self, matrixA, matrixB):
		numRowsA = len(matrixA)
		numColsB = len(matrixB[0])
		numColsARowsB = len(matrixB)
		result = []
		for row in range(0, numRowsA):
			newRow = []
			for col in range(0, numColsB):
				newValue = 0
				for colA in range(0, numColsARowsB):
					newValue = newValue + matrixA[row][colA]*matrixB[colA][col]
				newRow.append(newValue)
			result.append(newRow)
		return result
		
matrixMultiply = MatrixMultiply()
matrixA = [[1, 2], [3, 4]]
matrixB = [[3, 2, 4],  [1, 9, 3]]
result = matrixMultiply.matrixMultiply(matrixA, matrixB)
print(result)
