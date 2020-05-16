bestP = -1
bestOddsRelease = 0
for i in range (1, 10001):
	p = i/10000
	oddsRelease = 1/16*(p**4) + 4*1/8*(p**3)*(1-p) + 6*1/4*(p**2)*(1-p)**2 + 4*1/2*p*(1-p)**3
	if oddsRelease > bestOddsRelease:
		bestP = p
		bestOddsRelease = oddsRelease
print(str(bestP) + ' ' + str(bestOddsRelease))
