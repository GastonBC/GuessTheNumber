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

        public static void AnswerToNumber(string Guess, string Code, out int GoodAmmount, out int RegularAmmount)
        {
            GoodAmmount = 0;
            RegularAmmount = 0;

            // First check good numbers. Check player index num against same index on npc
            for (int i = 0; i < Guess.Length; i++)
            {
                if (Guess[i] == Code[i])
                {
                    GoodAmmount++;
                }
            }

            // Then check regular numbers. Check each index agains all indexes of NPC_NUM
            for (int i = 0; i < Guess.Length; i++)
            {
                for (int n = 0; n < Guess.Length; n++)
                {
                    // If it's a different index (that'd be a good number) and it's the
                    // same number then increment a regular
                    if (i != n && Guess[i] == Code[n])
                    {
                        RegularAmmount++;
                    }

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
