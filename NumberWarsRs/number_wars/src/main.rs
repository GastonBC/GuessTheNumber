mod utilities;
pub use utilities::utils;

mod enemy;
pub use enemy::enemy::Enemy;

use std::time::Instant;

// TODO program works as intended but it's slow and consumes a lot of cpu, wonder why

fn main() 
{
    const GAMES_TO_PLAY: i32 = 100;
    const DIGITS: i8 = 4;
    let all_codes = utils::all_permutations(&DIGITS);

    let mut total_steps = 0;
    


    let start_timer = Instant::now();
    for i in 0..GAMES_TO_PLAY
    {
        let mut game_steps = 0;

        let code = utils::number_from_list(&all_codes);

        let mut foe = Enemy::new_by_digits(&DIGITS);

        foe.make_guess();
        game_steps += 1;
        

        let answer = utils::answer_to_guess(&foe.last_guess, &code);

        foe.think(answer.0, answer.1);

        while code != foe.last_guess
        {
            foe.make_guess();
            game_steps += 1;
            let answer = utils::answer_to_guess(&foe.last_guess, &code);
            foe.think(answer.0, answer.1)
        }

        total_steps += game_steps;

        println!("{} - Steps: {}", i, game_steps)
    }

    let average: f32 = total_steps as f32 / GAMES_TO_PLAY as f32;
    println!("Time for 100 games {}", start_timer.elapsed().as_secs());
    println!("Average number of count: {}", average) 
}
