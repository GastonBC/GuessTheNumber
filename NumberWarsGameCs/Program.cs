using NumbersWarGame;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberWarsGameCs
{
    // TODO: Still not as quick as expected
    class Program
    {
        static void Main(string[] args)
        {
            // Now using this button to make the average win rate

            int GamesToPlay = 1000;
            int Digits = 4;
            double TotalSteps = 0;

            Console.WriteLine($"Games to play: {GamesToPlay}");
            Console.WriteLine($"Digits: {Digits}");
            Console.WriteLine($"Press enter when ready");
            Console.ReadLine();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<string> ALL_CODES = Utils.GetAllCodes(Digits);

            for (int i = 0; i <= GamesToPlay; i++)
            {
                // Seed the randomness to prevent 1 turn guesses due to quick succession
                string NPC1Code = Utils.GetValidNumber(ALL_CODES, i);

                Enemy NPC2 = new Enemy(Digits, ALL_CODES);

                NPC2.MakeGuess();
                int Steps = 1;

                int Good;
                int Regular;

                Utils.AnswerToGuess(NPC2.LastGuess, NPC1Code, out Good, out Regular);

                NPC2.Think(NPC2.LastGuess, Good, Regular);

                while (NPC1Code != NPC2.LastGuess)
                {
                    NPC2.LastGuess = NPC2.MakeGuess();

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

            Console.WriteLine($"Average: {TotalSteps / GamesToPlay}");
            Console.WriteLine($"Time elapsed: {ts}");
            Console.ReadLine();
        }
    }
}
