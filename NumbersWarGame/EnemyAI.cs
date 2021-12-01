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
        public List<string> PossiblePermutations { get; set; }
        public List<string> OldGuesses { get; set; }
        public string Code { get; }
        internal string NextGuess { get; set; }

        public EnemyAI(int Digits)
        {
            IEnumerable<IEnumerable<char>> CharPerms = GetPermutations("0123456789", Digits);
            PossiblePermutations = new List<string>();

            // Populate all possible guesses
            foreach (IEnumerable<char> CodesInChar in CharPerms)
            {
                string CodeAsString = new string(CodesInChar.ToArray());

                PossiblePermutations.Add(CodeAsString);
            }

            OldGuesses = new List<string>();
            Code = GetValidNumber(PossiblePermutations);
        }

        public string MakeGuess()
        {
            string Guess = GetValidNumber(PossiblePermutations);

            while (OldGuesses.Contains(Guess))
            {
                Guess = GetValidNumber(PossiblePermutations);
            }

            OldGuesses.Add(Guess);

            return Guess;
        }

        /// <summary>
        /// Processes the answer to the guess and returns the next guess
        /// </summary>
        public string Think(string LastGuess, int GoodAmmount, int RegularAmmount )
        {
            foreach(string Possibilities in new List<string>(PossiblePermutations))
            {
                int Good;
                int Regular;
                AnswerToNumber(LastGuess, Possibilities, out Good, out Regular);

                if (GoodAmmount != Good || RegularAmmount != Regular)
                {
                    PossiblePermutations.Remove(Possibilities);
                }
            }
            


            return "";
        }

        private string GetValidNumber(List<string> PossibleGuesses)
        {
            Random rd = new Random();
            int idx = rd.Next(PossiblePermutations.Count());

            return PossibleGuesses[idx];
        }

        private void AnswerToNumber(string Guess, string Code, out int GoodAmmount, out int RegularAmmount)
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


        static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }
    }
}
