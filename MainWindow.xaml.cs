using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

/*
 TODO: Make a drawing pane?
        simplify main window, it's weird and slow and ugly now
        randomize player number button
 */

/*
    NPC thinking > it has a list of 0 to 9. Takes the ammounts of digits needed to create a
    valid number and tries it against yours.

    special cases (on a 4 digit number)
    0G - 0R removes all digits on those numbers and tries (x+y = 0)
    again.
    0G - 4R keep trying those numbers with different order
    xG - yR if x + y = 4, those are all the numbers you need, keep trying with those


    Then how to process good and regular numbers?

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


            // First check good numbers. Check player index num against same index on npc
            for(int i = 0; i < GUESS_NUM.Length; i++)
            {
               if (NPC_NUM[i] == GUESS_NUM[i])
                {
                    GoodAmmount++;
                }
            }


            // Then check regular numbers. Check each index agains all indexes of NPC_NUM
            for (int i = 0; i < GUESS_NUM.Length; i++)
            {
                for (int n = 0; n < GUESS_NUM.Length; n++)
                {
                    // If it's a different index (that'd be a good number) and it's the
                    // same number then increment a regular
                    if (i != n && NPC_NUM[i] == GUESS_NUM[n])
                    {
                        RegularAmmount++;
                    }

                }
            }
            WriteGuessLn(tb_NumberTry.Text + " - " + $"{GoodAmmount}G - {RegularAmmount}R");
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



        

        private void GiveUp_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow HelpWn = new HelpWindow();
            HelpWn.Show();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            tb_NPCNumber.Text = "";
            tb_PlayerNumber.Text = "";
            tb_NumberTry.Text = "";

            GuessLog.Text = "";
            ConsoleLog.Text = "";

            b_Guess.IsEnabled = false;
        }
    }
}
