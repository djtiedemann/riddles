# start RGBY
# 1 turn to RRBG
# at R1 R2 B G
# choices are
# draws that don't help(R1, R2), (R2, R1), (B, R1), (B, R2), (G, R1), (G, R2)
# draws that get you to 3 reds (R1, B), (R1, G), (R2, B), (R2, G)
# draws that get you to 2 reds, 2 blues (G, B), (B, G)

# your odds of advancing is then 50% each round
# that gives you sum i=1^inf i/((1/2)^2)
# numIterations = 100
# sum = 0
# for i in range(1, numIterations+1):
	# sum = sum + i / (2**i)
	# print('iteration: ' + str(i) + ', ' + str(sum))

# the sum converges to 2
# so at 3 moves, you are in one of 2 states
# RRRB 2 out of 3 times, RRBB 1 out of 3 times

# let's say you're at R1 R2 B1 B2
# draws that move to 3 of a kind (R1, B1), (R1, B2), (R2, B1), (R2, B2), (B1, R1), (B1, R2), (B2, R1), (B2, R2)
# draws that do not help (R1, R2), (R2, R1), (B1, B2), (B2, B1)
# 2/3s of the time you move to a helpful state. so the odds of you moving to a successful state are
# numIterations = 100
# sum = 0
# for i in range(1, numIterations+1):
	# sum = sum + i * 2 * ((1/3)**i)
	# print('iteration: ' + str(i) + ', ' + str(sum))

# converges to 1.5. you go to RRBB 1/3 of the time, so we have 3 + (1/3)*(3/2)
# so you first make it to RRRB, on average after 3.5 moves
# from R1 R2 R3 B
# draws that win the game (R1, B), (R2, B), (R3, B)
# draws that make no progress (R1, R2), (R1, R3), (R2, R1), (R2, R3), (R3, R1), (R3, R2) - costing 1 additional move
# draws that take you back to the previous state (B, R1), (B, R2), (B, R3) - costing 2.5 additional moves
#
# each move costs you
# draws that win the game = 1
# draws that make no progress = 1
# draws that take you backwards = 1 + 1.5 expected to return
# therefore 1/4*1 + 1/2*1 + 2.5*1/4
# total cost of each turn = 11/8
# you have a 3/4 chance of having to do it again
numIterations = 100
sum = 11/8
for i in range(1, numIterations+1):
	sum = sum + ((3/4)**i)*11/8
	print('iteration: ' + str(i) + ', ' + str(sum))

# converges to 5.5
# 9 moves in total
# 1 + 2 + 1/3*3/2 + 11/2