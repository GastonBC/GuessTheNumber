using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

/*
 TODO: Make two consoles, one for goods and regulars, other for help messages
        Make regulars check, with str.Count(f => (f == ch)), this can work for goods as well
        Make a drawing pane?
 */

namespace GuessTheNumber
{
    public partial class MainWindow : Window
    {

        string NPC_NUM;
        string PLAYER_NUM;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void onPlayerTextChange(object sender, TextChangedEventArgs e)
        {
            if (tb_PlayerNumber.Text.Length >= 3)
            {
                tb_NumberTry.IsEnabled = true;
                b_ConfirmPlayer.IsEnabled = true;
            }
            else
            {
                tb_NumberTry.IsEnabled = false;
                b_ConfirmPlayer.IsEnabled = false;
            }
        }

        private void onGuessTextChange(object sender, TextChangedEventArgs e)
        {
            if (tb_NumberTry.Text.Length == tb_NPCNumber.Text.Length)
            {
                b_Guess.IsEnabled = true;
            }
            else
            {
                b_Guess.IsEnabled = false;
            }
        }

        private void Guess_Click(object sender, RoutedEventArgs e)
        {
            string GUESS_NUM = tb_NumberTry.Text;

            if(!RunNumberChecks(GUESS_NUM, true)) return;

            int GoodAmmount = 0;
            int RegularAmmount = 0;


            // First check good numbers
            for(int i = 0; i < GUESS_NUM.Length; i++)
            {
               if (NPC_NUM[i] == GUESS_NUM[i])
                {
                    GoodAmmount++;
                }
            }

            WriteLn(tb_NumberTry.Text + " - " + $"{GoodAmmount}G - {RegularAmmount}R");
        }

        private void ConfirmNum_Click(object sender, RoutedEventArgs e)
        {
            PLAYER_NUM = tb_PlayerNumber.Text;

            // if checks fail
            if (!RunNumberChecks(PLAYER_NUM, true))
            {
                return;
            }


            int min = Convert.ToInt32(1 * (Math.Pow(10d, PLAYER_NUM.Length - 1)));    // 1* (10^x-1) - 1.  eg: 1*(10^4) = 10.000
            int max = Convert.ToInt32( (1 * (Math.Pow(10d, PLAYER_NUM.Length))) - 1); // 1* (10^x) - 1.  eg: 1*(10^5)-1 = 99.999

            Random rd = new Random();

            bool isNumberWrong = true;

            while (isNumberWrong)
            {
                NPC_NUM = rd.Next(min, max).ToString();
                if (RunNumberChecks(NPC_NUM, false))
                {
                    isNumberWrong = false;
                }
            }

            tb_NPCNumber.Text = NPC_NUM;

            // b_ConfirmPlayer.IsEnabled = false;
        }



        private void WriteLn(string text) // Invokes the console.text from the new thread and appends the line
        {

            ConsoleLog.Dispatcher.BeginInvoke(new Action(() =>
            { ConsoleLog.Text += text + Environment.NewLine; }));
        }

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
    }
}
