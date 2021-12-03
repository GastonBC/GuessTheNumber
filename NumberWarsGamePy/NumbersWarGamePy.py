import random
import utils
import time
from enemy import Enemy

GAMES_TO_PLAY = 10000
DIGITS = 4
ALL_CODES = utils.get_all_codes(DIGITS)

total_steps = 0

start = time.time()

for i in range(0, GAMES_TO_PLAY):
    game_seps = 0

    code = utils.get_valid_number(ALL_CODES)

    # foe = Enemy.enemy.enemy_by_digits(DIGITS)
    foe = Enemy.enemy_by_given_possibilities(ALL_CODES)
    
    foe.make_guess()
    game_seps += 1

    good, regular = utils.answer_to_guess(foe.last_guess, code)

    foe.think(good, regular)

    while code != foe.last_guess:
        foe.make_guess()
        game_seps += 1
        good, regular = utils.answer_to_guess(foe.last_guess, code)
        foe.think(good, regular)
    
    total_steps += game_seps
    # print("{} - Steps: {}".format(i, game_seps))

end = time.time() 
print("Time elapsed:", end - start)
print("Average number of count: ", total_steps / GAMES_TO_PLAY) 