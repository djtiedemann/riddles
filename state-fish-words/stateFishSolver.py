# https://fivethirtyeight.com/features/somethings-fishy-in-the-state-of-the-riddler/
class stateFishSolver:
	def __init__(self, statesLetterDict, statesMackerelCountDictionary):
		self.statesLetterDict = statesLetterDict
		self.states = statesLetterDict.keys()
		self.longestMackerel = ''
		self.stateWithLongestMackerel = ''
		self.statesMackerelCountDictionary = statesMackerelCountDictionary
		
	def getLongestMackerel(self):
		return self.longestMackerel
		
	def getStateWithLongestMackerel(self):
		return self.stateWithLongestMackerel
		
	def getStateWithMostMackerelsInfo(self):
		mostMackerels = 0
		stateWithMostMackerels = ''
		for state in self.statesMackerelCountDictionary.keys():
			if(self.statesMackerelCountDictionary[state] > mostMackerels):
				mostMackerels = self.statesMackerelCountDictionary[state]
				stateWithMostMackerels = state
		return (stateWithMostMackerels, mostMackerels)
			
		
	def processWord(self, word):
		matchingStates = self.getMatchingStates(word)
		if(len(matchingStates) == 1):
			self.statesMackerelCountDictionary[matchingStates[0]] = self.statesMackerelCountDictionary[matchingStates[0]] + 1
			if(len(word) > len(self.longestMackerel)):
				self.longestMackerel = word
				self.stateWithLongestMackerel = matchingStates[0]
		
	def getMatchingStates(self, word):
		distinctLetters = set(word)
		matchingStates = []
		for state in self.states:
			hasOverlappingLetter = False
			for letter in distinctLetters:
				if letter in self.statesLetterDict[state]:
					hasOverlappingLetter = True
					break
			if(not hasOverlappingLetter):
				matchingStates.append(state)
		return matchingStates

statesFile = open('..\states.txt', 'r')
statesLetterDict = {}
statesMackerelCountDictionary = {}
for line in statesFile:
	state = line.rstrip().lower().replace(" ", "")
	distinctLetters = set(state)
	statesLetterDict[state] = distinctLetters
	statesMackerelCountDictionary[state] = 0
statesFile.close()
stateFishSolver = stateFishSolver(statesLetterDict, statesMackerelCountDictionary)
# stateFishSolver.processWord('mackerel')
# print(stateFishSolver.getLongestMackerel())
# print(stateFishSolver.getStateWithLongestMackerel())
# stateFishSolver.processWord('jellyfish')
# print(stateFishSolver.getLongestMackerel())
# print(stateFishSolver.getStateWithLongestMackerel())
# stateFishSolver.processWord('goldfish')
# print(stateFishSolver.getLongestMackerel())
# print(stateFishSolver.getStateWithLongestMackerel())
f = open('..\words.txt', 'r')
i = 1
for line in f:
	stateFishSolver.processWord(line)
f.close()
print(stateFishSolver.getLongestMackerel())
print(stateFishSolver.getStateWithLongestMackerel())
stateWithMostMackerelsInfo = stateFishSolver.getStateWithMostMackerelsInfo()
print(stateWithMostMackerelsInfo[0] + ': ' + str(stateWithMostMackerelsInfo[1]))