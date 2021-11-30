using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NumbersWarGame
{
    class EnemyAI
    {
        public Level? Difficulty { get; set; }
        public List<char> PossibleNumbers { get; set; }
        public List<string> OldGuesses { get; set; }
        public List<List<char>> MustNotBe { get; set; }
        public string ChosenNumber { get; }
        internal string NextGuess { get; set; }

        public EnemyAI(string PlayerNumber)
        {
            PossibleNumbers = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            OldGuesses = new List<string>();
            MustNotBe = new List<List<char>>();

            // Initialize empty nested lists or it gives an error on MakeGuess()
            foreach (char ch in PlayerNumber.ToString())
            {
                List<char> CharLst = new List<char>();
                MustNotBe.Add(CharLst);
            }

            ChosenNumber = GetValidNumber(PlayerNumber.Length);
            NextGuess = null;
        }

        public string MakeGuess()
        {
            string Guess = "";
            bool IsGuessPossibleWin = false;

            // While it's NOT a possible win
            while (!IsGuessPossibleWin)
            {
                // This number will have to pass certain conditions
                // If all of them are passed, then IsGuessPossibleWin is true
                Guess = GetValidNumber(ChosenNumber.Length);

                // Check if every char in guess is in PossibleNumbers. Because the list will shrink
                // with each guess
                if (Guess.Intersect(PossibleNumbers).Count() != Guess.Count())
                { continue; }

                // OldGuesses must NOT contain the new guess
                if (OldGuesses.Contains(Guess))
                { continue; }

                // Check Guess indexes to must not bees
                bool SkipFlag = false;
                foreach (char ch in Guess)
                {
                    int idx = Guess.IndexOf(ch);

                    if (!IsCharacterValidAtIdx(ch, idx))
                    {
                        SkipFlag = true;
                        break;
                    }
                }
                if (SkipFlag)
                { continue; }


                IsGuessPossibleWin = true;

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

                if (RegularAmmount == ChosenNumber.Length)
                {
                    foreach (char ch in LastGuess)
                    {
                        int idx = LastGuess.IndexOf(ch);

                        MustNotBe[idx].Add(ch);
                    }
                }
            }

            /* List different strategies
             * 
             * Try completely different sets of numbers each time
             * 
             * If a guess had 2 R or G and another completely different another 2 then keep
             * trying those 8 numbers only
             * 
             * Try common numbers? Is that a thing?
             * 
             * After that it's basically reading through the history and learning but howww
             */

            return "";
        }

        private bool IsCharacterValidAtIdx(char ch, int idx)
        { 
            // No zeroes at the beginning of string
            if (ch == '0' && idx == 0)
            {
                return false;
            }

            // Must not be list changes with guesses
            if (MustNotBe[idx].Contains(ch))
            {
                return false;
            }

            return true;
        }

        private string GetValidNumber(int digits)
        {
            string RANDOM_NUM = "";

            Random rd = new Random();

            for (int i = 0; i < digits; i++)
            {
                // Choose random number from POSSIBLE nums
                int idx = rd.Next(PossibleNumbers.Count);

                bool IsCharValid = false;
                // If all conditions are met then is valid, exit while
                while (!IsCharValid)
                {
                    idx = rd.Next(PossibleNumbers.Count);

                    // Check MustNotBees
                    if (!IsCharacterValidAtIdx(PossibleNumbers[idx], i))
                    { continue; }

                    // Check if the character repeats
                    bool SkipFlag = false;
                    foreach (char NUM_ch in RANDOM_NUM)
                    {
                        if (NUM_ch == PossibleNumbers[idx])
                        {
                            SkipFlag = true;
                            break;
                        }
                    }
                    if (SkipFlag == true)
                    { continue; }

                    IsCharValid = true;


                }

                RANDOM_NUM = RANDOM_NUM + PossibleNumbers[idx];
            }

            return RANDOM_NUM;
        }
    }
}
