import utils

class Enemy(object):
    def __init__(self, digits = None, possible_guesses = None):
        '''Initializes an instance of the Enemy'''


        if digits is None and possible_guesses is None:
            raise AttributeError("No digits or codes list provided")

        elif digits is not None:
            self._possible_permutations = utils.get_all_codes(digits) # 84.5 secs in 1000 games
        
        elif possible_guesses is not None:
            self._possible_permutations = list.copy(possible_guesses) 

        self._code = utils.get_valid_number(self._possible_permutations)
        self._old_guesses = []
        self._last_guess = None

    @property
    def possible_permutations(self):
        return self._possible_permutations

    @property
    def old_guesses(self):
        return self._old_guesses
    
    @property
    def code(self):
        return self._code

    @property
    def last_guess(self):
        return self._last_guess

    @classmethod
    def enemy_by_given_possibilities(cls, possible_guesses): # 64.7 secs in 1000 games
        '''Initializes an instance of the enemy given the possibilities. This is faster than
        letting the initializer get all the permutations at each object creation'''
        return cls(None, possible_guesses)

    @classmethod
    def enemy_by_digits(cls, digits): # 72.8 secs in 1000 games
        '''Initializes an instance of the enemy given the ammount of digits for 
        the game'''
        return cls(digits, None)
        
    def think(self, good_ammount, regular_ammount):
        '''Compares the last guess to all the remaining possibilities
        and removes any of them that don't match the goods and regulars'''

        for possible_code in list.copy(self.possible_permutations):
            good, regular = utils.answer_to_guess(possible_code, self.last_guess)

            if good_ammount != good or regular_ammount != regular:
                self._possible_permutations.remove(possible_code)

    
    def make_guess(self):
        '''Gets a random number from the remaining possibilities'''

        guess = utils.get_valid_number(self._possible_permutations)

        while guess in self._old_guesses:
            guess = utils.get_valid_number(self._possible_permutations)

        self._last_guess = guess
        self._old_guesses.append(guess)

        return guess