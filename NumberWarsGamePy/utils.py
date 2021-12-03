from itertools import permutations
import random

def get_all_codes(digits):
    '''Gets all the codes that match the ammount of digits'''
    population = permutations(["0","1","2","3","4","5","6","7","8","9"], digits)
    return list(population)

def get_valid_number(possible_numbers):
    '''Gets a valid number from the list of possibilities'''
    num = random.choice(possible_numbers)
    return num

def answer_to_guess(guess, code):
    '''Provides the answer to the guess in a tuple of (good, regular)'''
    good = 0
    regular = 0

    for idx, val in enumerate(code):
        if guess[idx] in code:
            if guess[idx] == code[idx]:
                good += 1
                continue
            regular += 1

    return good, regular