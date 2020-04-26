choices = list(range(100))
bestI = -1
bestNumSearches = 1000
for i in choices:
	# i means that we will only stop if we are within i of the song
	# i = 0 means we must hit our random number choice exactly
	# i = 99 means we will always sequentially search
	p_stopRandomSearch = (i+1)/100.0
	expectedSearchesAfterStop = i/2.0
	expectedNumberRandomSearches = 1/p_stopRandomSearch
	#print('i: ' + str(i) + ', p_stopRandomSearch: ' + str(p_stopRandomSearch))
	#print('i: ' + str(i) + ', expectedSearchesAfterStop: ' + str(expectedSearchesAfterStop) + ', expectedNumberRandomSearches ' + str(expectedNumberRandomSearches))
	expectedNumSearches = expectedNumberRandomSearches + expectedSearchesAfterStop
	print('i: ' + str(i) + ', expected number of searches ' + str(expectedNumSearches))
	if(expectedNumSearches < bestNumSearches):
		bestNumSearches = expectedNumSearches
		bestI = i
print('best I: ' + str(bestI) + ', best num searches: ' + str(bestNumSearches))
