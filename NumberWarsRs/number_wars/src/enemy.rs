pub mod enemy
{
    use itertools::Itertools;
    use rand::seq::SliceRandom;

    pub use crate::utilities::utils;

    pub struct Enemy
    {
        pub code: Vec<i8>,
        pub old_guesses: Vec<Vec<i8>>,
        pub last_guess: Vec<i8>,
        pub possible_permutations: Vec<Vec<i8>>
    }


    impl Enemy
    {
        pub fn new_by_possibilities(possible_guesses: &Vec<Vec<i8>>) -> Self
        {
            let mut rng = rand::thread_rng();
            let choice = possible_guesses.choose(&mut rng).unwrap();

            Self
            {
                code: choice.clone(),
                old_guesses: Vec::new(),
                last_guess: Vec::new(),
                possible_permutations: possible_guesses.clone()
                
            }
        }

        pub fn new_by_digits(digits: &i8) -> Self
        {
            let mut rng = rand::thread_rng();
            let perms = utils::all_permutations(&digits);
            let choice = perms.choose(&mut rng).unwrap();

            Self
            {
                old_guesses: Vec::new(),
                last_guess: Vec::new(),
                code: choice.clone(),
                possible_permutations: perms
            }
        }

        pub fn code_as_string(&self) -> String
        {
            return utils::vec_as_string(&self.last_guess)
        }


        pub fn think(&mut self, good_ammount: i8, regular_ammount: i8)
        {
            
            for possible_code in self.possible_permutations.clone()
            {
                let answer = utils::answer_to_guess(&possible_code, &self.last_guess);

                if answer.0 != good_ammount || answer.1 != regular_ammount
                {
                    match self.possible_permutations.iter().position(|r| r == &possible_code)
                    {
                        Some(idx) => { self.possible_permutations.swap_remove(idx); }
                        None => panic!("Panicked at thinking")
                    }
                }
            }
            
        }

        pub fn make_guess(&mut self) -> Vec<i8>
        {
            let mut guess = utils::number_from_list(&self.possible_permutations);
            while self.old_guesses.contains(&guess)
            {
                guess = utils::number_from_list(&self.possible_permutations);
            }

            self.last_guess = guess.clone();
            self.old_guesses.push(guess.clone());
            
            return guess;
        }
    }
}