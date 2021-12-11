pub mod utils
{
    use itertools::Itertools;
    use rand::seq::SliceRandom;

    pub fn all_permutations(digits: &u8) -> Vec<Vec<u8>>
    {
        let mut all_codes = Vec::new();
        let num_usize = digits.clone() as usize;

        for perm in (0..10).permutations(num_usize)
        {
            all_codes.push(perm)
        }

        return all_codes;
    }

    pub fn number_from_list(possible_numbers: &Vec<Vec<u8>>) -> Vec<u8>
    {
        
        let mut rng = rand::thread_rng();
        let choice = possible_numbers.choose(&mut rng).unwrap();
        return choice.clone();
    }

    pub fn answer_to_guess(guess: &Vec<u8>, code: &Vec<u8>) -> (u8, u8)
    {
        let mut good: u8 = 0;
        let mut regular: u8 = 0;

        for (idx, digit) in guess.iter().enumerate()
        {
            if code.contains(digit)
            {
                // A correct digit in the correct position
                if code[idx] == guess[idx]
                {
                    good += 1;
                }
                else
                {
                    regular += 1;
                }
            }
        }
        (good, regular)
    }

    pub fn vec_as_string(vec_code: &Vec<u8>) -> String
    {
        let joined = vec_code.iter().join("");
        return joined;
    }

}