pub mod enemy
{
    use itertools::Itertools;
    use rand::seq::SliceRandom;

    pub use crate::utilities::utils;

    pub struct Enemy
    {
        pub code: Vec<u8>,
        pub old_guesses: Vec<Vec<u8>>,
        pub last_guess: Vec<u8>,
        pub possible_permutations: Vec<Vec<u8>>
    }


    impl Enemy
    {
        pub fn new_by_possibilities(possible_guesses: &Vec<Vec<u8>>) -> Self
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

        pub fn new_by_digits(digits: &u8) -> Self
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


        // pub fn think(&mut self, good_ammount: u8, regular_ammount: u8) {
        //     self.possible_permutations = self.possible_permutations
        //         .into_iter()
        //         .filter(|possible_code| {
        //             utils::answer_to_guess(possible_code, &self.last_guess) == (good_ammount, regular_ammount)
        //         })
        //         .collect();
        //     }
        
        pub fn think(&mut self, good_ammount: u8, regular_ammount: u8) 
        {
            self.possible_permutations.retain(|possible_code| 
            {
                utils::answer_to_guess(possible_code, &self.last_guess) == (good_ammount, regular_ammount)
            });
        }

        pub fn make_guess(&mut self) -> Vec<u8>
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