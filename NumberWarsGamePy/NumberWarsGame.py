import random
from itertools import permutations
import time

play_number = 10000  # how many time we want to play
total_num = 0  # store the value of the total number of guessing through all the play
NUM_OF_DIGITS = 4

def givenum():
    num = random.sample(range(0,9), NUM_OF_DIGITS)
    return tuple(num)

def playresult(notknow, guess):
    A = 0
    B = 0
    for idx, val in enumerate(notknow):
        for idx2, val2 in enumerate(guess):
            if (idx == idx2 and val == val2):  # position & value are correct
                A = A + 1
            elif (val == val2):
                B = B + 1
    return A, B

def ini_population():
    population = permutations([0,1,2,3,4,5,6,7,8,9], NUM_OF_DIGITS)
    return list(population)

start = time.time() # start timer
total_num = 0
for i in range(play_number):  # start playing each game
    code = givenum()  # Create a code
    code_set = ini_population()  # Initialize a set of code set containing possible answer

    # Create a first guess randomly
    guess = tuple(random.sample(range(0, 9), NUM_OF_DIGITS))
    
    # Get the A, B value with guess and code
    A, B = playresult(code, guess)
    play_count = 1  # store the value of the number of guessing in this play

    while (A < NUM_OF_DIGITS):  # Still cleaning the code_set until we find the real answer
        play_count = play_count + 1

        # takes every combination that yields the same A, B
        # the correct one will eventually be one of them
        code_set = [t for t in code_set if playresult(t, guess) == (A, B)]

        guess = min(code_set)
        
        A, B = playresult(code, guess)

    print(i, "- STEPS:", play_count)
    total_num = total_num + play_count  # add the total number of playing to total_num

end = time.time() # finish timer
print("Time elapsed:", end - start)
print("Average number of count: ", total_num / play_number)  # show the average number of guessing