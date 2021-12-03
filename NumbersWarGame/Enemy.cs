using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NumbersWarGame
{
    class Enemy
    {
        public Level? Difficulty { get; set; }
        public List<string> PossiblePermutations { get; set; }
        public List<string> OldGuesses { get; }
        public string Code { get; }
        internal string LastGuess { get; set; }

        // Common constructor when speed is not an issue
        public Enemy(int Digits)
        {
            PossiblePermutations = new List<string>(Utils.GetAllCodes(Digits));

            OldGuesses = new List<string>();
            Code = Utils.GetValidNumber(PossiblePermutations);
        }

        // Requires the list of permutations to make the class faster and a random generator
        // This constructor is meant for testing
        public Enemy(int Digits, List<string> permutations)
        {
            PossiblePermutations = new List<string>(permutations);

            OldGuesses = new List<string>();
            Code = Utils.GetValidNumber(PossiblePermutations);
        }

        public string MakeGuess()
        {
            string Guess = Utils.GetValidNumber(PossiblePermutations);

            while (OldGuesses.Contains(Guess))
            {
                Guess = Utils.GetValidNumber(PossiblePermutations);
            }

            LastGuess = Guess;
            OldGuesses.Add(Guess);

            return Guess;
        }


        public void Think(string LastGuess, int GoodAmmount, int RegularAmmount )
        {

            // Create a new possible permutations list to be able to modify the original
            foreach (string Possibilities in new List<string>(PossiblePermutations))
            {
                int Good;
                int Regular;
                Utils.AnswerToGuess(LastGuess, Possibilities, out Good, out Regular);

                if (GoodAmmount != Good || RegularAmmount != Regular)
                {
                    PossiblePermutations.Remove(Possibilities);
                }
            }
            return;
        }

        
    }
}
