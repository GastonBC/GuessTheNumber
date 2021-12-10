pub mod utils
{
    use itertools::Itertools;
    use rand::seq::SliceRandom;

    pub fn all_permutations(digits: &i8) -> Vec<Vec<i8>>
    {
        let mut all_codes = Vec::new();
        let num_usize = digits.clone() as usize;

        for perm in (0..10).permutations(num_usize)
        {
            all_codes.push(perm)
        }

        return all_codes;
    }

    pub fn number_from_list(possible_numbers: &Vec<Vec<i8>>) -> Vec<i8>
    {
        
        let mut rng = rand::thread_rng();
        let choice = possible_numbers.choose(&mut rng).unwrap();
        return choice.clone();
    }

    pub fn answer_to_guess(guess: &Vec<i8>, code: &Vec<i8>) -> (i8, i8)
    {
        let mut good: i8 = 0;
        let mut regular: i8 = 0;

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

    pub fn vec_as_string(vec_code: &Vec<i8>) -> String
    {
        let joined = vec_code.iter().join("");
        return joined;
    }

}