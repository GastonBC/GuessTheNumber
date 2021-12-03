using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumbersWarGame
{
    class Utils
    {
        public static List<string> GetAllCodes(int Digits)
        {
            IEnumerable<IEnumerable<char>> CharPerms = GetPermutations("0123456789", Digits);
            List<string> PossiblePerms = new List<string>();

            // Populate all possible guesses
            foreach (IEnumerable<char> CodesInChar in CharPerms)
            {
                string CodeAsString = new string(CodesInChar.ToArray());
                PossiblePerms.Add(CodeAsString);
            }

            return PossiblePerms;
        }


        public static string GetValidNumber(List<string> PossibleGuesses)
        {
            Random rd = new Random();
            int idx = rd.Next(PossibleGuesses.Count());

            return PossibleGuesses[idx];
        }

        /// <summary>
        /// Alternate code getting
        /// </summary>
        public static string GetValidNumber(int Digits)
        {
            Random rn = new Random();
            string Code = "";

            while (Code.Length < Digits)
            {
                string dig = rn.Next(10).ToString();
                while (Code.Contains(dig))
                {
                    dig = rn.Next(10).ToString();

                }
                Code += dig;
            }
            return Code;
        }


        public static void AnswerToGuess(string Guess, string Code, out int GoodAmmount, out int RegularAmmount)
        {
            GoodAmmount = 0;
            RegularAmmount = 0;
            
            for (int i = 0; i < Guess.Length; i++)
            {
                // Code contains the guess digit
                if (Code.Contains(Guess[i]))
                {
                    // Guess digit positioning is also correct
                    if (Guess[i] == Code[i])
                    {
                        GoodAmmount++;
                        continue;
                    }

                    RegularAmmount++;
                }
            }
        }

        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }
    }
}
