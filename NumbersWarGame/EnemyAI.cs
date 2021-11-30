using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumbersWarGame
{
    class EnemyAI
    {
        public Level? Difficulty { get; set; }
        public List<char> PossibleNumbers { get; set; }
        public List<string> OldGuesses { get; set; }
        public string ChosenNumber { get; }
        internal string NextGuess { get; set; }

        public EnemyAI(string PlayerNumber)
        {
            NextGuess = null;
            ChosenNumber = GetValidNumber(PlayerNumber.Length);

            PossibleNumbers = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            OldGuesses = new List<string>();
        }

        public string MakeGuess()
        {
            string Guess = "";
            bool IsGuessPossibleWin = false;

            while (!IsGuessPossibleWin || OldGuesses.Contains(Guess))
            {
                Guess = GetValidNumber(ChosenNumber.Length);

                // Check if every char in guess is in PossibleNumbers. Because the list will shrink
                // with each guess
                IsGuessPossibleWin = Guess.Intersect(PossibleNumbers).Count() == Guess.Count();
            }

            OldGuesses.Add(Guess);

            return Guess;
        }

        /// <summary>
        /// Processes the answer to the guess and returns the next guess
        /// </summary>
        public string Think(string LastGuess, int GoodAmmount, int RegularAmmount )
        {
            // answer = 0G, 0R, discard numbers
            if (GoodAmmount == 0 && RegularAmmount == 0)
            {
                foreach (char ch in LastGuess)
                {
                    PossibleNumbers.Remove(ch);
                }
            }

            // answer is correct but in a different order. Remove the rest of the numbers from 
            // the possible list
            else if (GoodAmmount + RegularAmmount == ChosenNumber.Length)
            {
                List<char> TmpLst = new List<char>(PossibleNumbers);
                foreach (char ch in TmpLst)
                {
                    if (!LastGuess.Contains(ch))
                    {
                        PossibleNumbers.Remove(ch);
                    }
                }
            }

            // 4 Regulars means all of the numbers need to change position

            /* should make a variable for each index, check if that index is 
             * definetly not x number
            */
            return "";
        }


        private bool RunNumberChecks(string NUMBER)
        {
            // First check if first digit is 0
            if (NUMBER.First().ToString() == "0")
            {
                return false;
            }

            // And if all chars are numbers
            for (int i = 0; i < NUMBER.Length; i++)
            {
                if (!Char.IsDigit(NUMBER[i]))
                {
                    return false;
                }
            }

            // And if a number is repeated
            foreach (char ch in NUMBER)
            {
                int freq = NUMBER.Count(f => (f == ch));

                if (freq > 1)
                {
                    return false;
                }
            }

            return true;
        }

        private string GetValidNumber(int digits)
        {
            string RANDOM_NUM = null;

            int min = Convert.ToInt32(1 * (Math.Pow(10d, digits - 1)));    // 1* (10^x-1) - 1.  eg: 1*(10^4) = 10.000
            int max = Convert.ToInt32((1 * (Math.Pow(10d, digits))) - 1);  // 1* (10^x) - 1.  eg: 1*(10^5)-1 = 99.999

            Random rd = new Random();

            bool isNumberWrong = true;

            while (isNumberWrong)
            {
                RANDOM_NUM = rd.Next(min, max).ToString();
                if (RunNumberChecks(RANDOM_NUM))
                {
                    isNumberWrong = false;
                }
            }
            return RANDOM_NUM;
        }
    }
}
