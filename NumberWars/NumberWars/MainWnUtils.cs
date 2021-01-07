using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace NumberWars
{
    public partial class MainWindow : Window
    {

        /// <summary>
        /// Invokes the console.text from the new thread and appends the line
        /// </summary>
        private void WriteLn(string text)
        {

            ConsoleLog.Dispatcher.BeginInvoke(new Action(() =>
            { ConsoleLog.Text += text + Environment.NewLine; }));
        }

        /// <summary>
        /// Invokes the console.text from the new thread and appends the line
        /// </summary>
        private void WriteGuessLn(string text)
        {

            GuessLog.Dispatcher.BeginInvoke(new Action(() =>
            { GuessLog.Text += text + Environment.NewLine; }));
        }

        /// <summary>
        /// Check if number is valid for the game
        /// </summary>
        private bool RunNumberChecks(string NUMBER, bool messages)
        {
            // First check if first digit is 0
            if (NUMBER.First().ToString() == "0")
            {
                if (messages) WriteLn("Your number cannot begin with a 0");
                return false;
            }

            // And if all chars are numbers
            for (int i = 0; i < NUMBER.Length; i++)
            {
                if (!Char.IsDigit(NUMBER[i]))
                {
                    if (messages) WriteLn("All characters must be digits");
                    return false;
                }
            }

            // And if a number is repeated
            foreach (char ch in NUMBER)
            {
                int freq = NUMBER.Count(f => (f == ch));

                if (freq > 1)
                {
                    if (messages) WriteLn("You can only use a digit once");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Give up condition, freeze all until you hit reset
        /// </summary>
        private void FreezeCommands()
        {
            // Disable all commands
            tb_NPCNumber.IsEnabled = false;
            tb_PlayerNumber.IsEnabled = false;
            tb_NumberTry.IsEnabled = false;

            b_ConfirmPlayer.IsEnabled = false;
            b_Guess.IsEnabled = false;
            b_GiveUp.IsEnabled = false;
            b_RandomDiff.IsEnabled = false;
        }

        /// <summary>
        /// Gets a valid, random number for the game
        /// </summary>
        private string GetValidNumber(int digits)
        {
            string RANDOM_NUM = null;

            int min = Convert.ToInt32(1 * (Math.Pow(10d, digits - 1)));    // 1* (10^x-1) - 1.  eg: 1*(10^4) = 10.000
            int max = Convert.ToInt32((1 * (Math.Pow(10d, digits))) - 1); // 1* (10^x) - 1.  eg: 1*(10^5)-1 = 99.999

            Random rd = new Random();

            bool isNumberWrong = true;

            while (isNumberWrong)
            {
                RANDOM_NUM = rd.Next(min, max).ToString();
                if (RunNumberChecks(RANDOM_NUM, false))
                {
                    isNumberWrong = false;
                }
            }
            return RANDOM_NUM;
        }


        /// <summary>
        /// Freeze not needed buttons, unless you are debugging
        /// </summary>
        private void SessionStart()
        {
            STEPS = 0;
            string Asterix = "";

            foreach (char ch in NPC_NUM)
            {
                Asterix = Asterix + "* ";
            }


            tb_NPCNumber.Text = Asterix;
            tb_PlayerNumber.Text = PLAYER_NUM;

            tb_NumberTry.IsEnabled = true;
            tb_PlayerNumber.IsEnabled = false;

            b_Guess.IsEnabled = true;
            b_ConfirmPlayer.IsEnabled = false;
            b_GiveUp.IsEnabled = true;
            b_RandomDiff.IsEnabled = false;

#if DEBUG
            b_ConfirmPlayer.IsEnabled = true;
            tb_NPCNumber.Text = NPC_NUM;
#endif
        }
    }
}
