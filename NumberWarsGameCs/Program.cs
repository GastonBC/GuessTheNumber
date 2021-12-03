using NumbersWarGame;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// BENCHMARK 2021-12-03 - 20.1 secs in 1.000 games, average 5.4

namespace NumberWarsGameCs
{
    class Program
    {
        static void Main(string[] args)
        {
            const int GAMES_TO_PLAY = 1000;
            const int DIGITS = 4;
            double TotalSteps = 0;
            List<string> ALL_CODES = Utils.GetAllCodes(DIGITS);

            Console.WriteLine($"Games to play: {GAMES_TO_PLAY}");
            Console.WriteLine($"Digits: {DIGITS}");
            Console.WriteLine($"Press enter when ready");
            Console.ReadLine();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            for (int i = 0; i <= GAMES_TO_PLAY; i++)
            {
                // Have to use this method to get a new number instead of GetValidNum(list) because
                // of pseudo randomness being too slow to give different numbers
                string NPC1Code = Utils.GetValidNumber(DIGITS);

                Enemy NPC2 = new Enemy(DIGITS, ALL_CODES);

                NPC2.MakeGuess();

                int Steps = 1;

                int Good;
                int Regular;

                Utils.AnswerToGuess(NPC2.LastGuess, NPC1Code, out Good, out Regular);

                NPC2.Think(NPC2.LastGuess, Good, Regular);

                while (NPC1Code != NPC2.LastGuess)
                {
                    NPC2.MakeGuess();

                    Utils.AnswerToGuess(NPC2.LastGuess, NPC1Code, out Good, out Regular);

                    NPC2.Think(NPC2.LastGuess, Good, Regular);

                    Steps++;
                }

                TotalSteps += Steps;

                Console.WriteLine($"{i} - Steps: {Steps}");
            }

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            Console.WriteLine($"Average: {TotalSteps / GAMES_TO_PLAY}");
            Console.WriteLine($"Time elapsed: {ts}");
            Console.ReadLine();
        }
    }
}
