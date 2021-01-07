using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

/*
 TODO: Make a drawing pane?
        simplify main window, it's weird and slow and ugly now
       
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

namespace NumbersWarGame
{
    public partial class MainWindow : Window
    {
        string NPC_NUM;
        string PLAYER_NUM;
        int STEPS;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void onPlayerTextChange(object sender, TextChangedEventArgs e)
        {
            if (tb_PlayerNumber.Text.Length >= 2 && tb_PlayerNumber.Text.Length <= 10)
            {
                b_ConfirmPlayer.IsEnabled = true;
            }
            else
            {
                b_ConfirmPlayer.IsEnabled = false;
            }
        }

        private void onGuessTextChange(object sender, TextChangedEventArgs e)
        {
            if (tb_NumberTry.Text.Length == NPC_NUM.Length)
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

            if (GUESS_NUM == "" || RunNumberChecks(GUESS_NUM, true) == false) return;

            int GoodAmmount = 0;
            int RegularAmmount = 0;


            // First check good numbers. Check player index num against same index on npc
            for (int i = 0; i < GUESS_NUM.Length; i++)
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
            STEPS++;

            // Win condition
            if (GoodAmmount == NPC_NUM.Length)
            {
                WriteGuessLn("");
                WriteGuessLn($"You win! Number was {NPC_NUM}");
                WriteGuessLn($"Steps taken {STEPS}");
                FreezeCommands();
                return;
            }
        }

        private void ConfirmNum_Click(object sender, RoutedEventArgs e)
        {
            PLAYER_NUM = tb_PlayerNumber.Text;

            // if checks fail
            if (!RunNumberChecks(PLAYER_NUM, true))
            {
                return;
            }

            NPC_NUM = GetValidNumber(PLAYER_NUM.Length);
            while (PLAYER_NUM == NPC_NUM)
                NPC_NUM = GetValidNumber(PLAYER_NUM.Length);

            SessionStart();

        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow HelpWn = new HelpWindow();
            HelpWn.Show();
        }


        private void GiveUp_Click(object sender, RoutedEventArgs e)
        {
            tb_NPCNumber.Text = NPC_NUM;

            WriteGuessLn("");
            WriteGuessLn($"Oh no! Number was {NPC_NUM}");
            FreezeCommands();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            // Reset to initial state
            tb_NPCNumber.Text = "";
            tb_PlayerNumber.Text = "";
            tb_NumberTry.Text = "";

            GuessLog.Text = "";
            ConsoleLog.Text = "";

            tb_PlayerNumber.IsEnabled = true;

            b_Guess.IsEnabled = false;
            b_GiveUp.IsEnabled = false;
            b_RandomDiff.IsEnabled = true;
        }

        private void RandomDiff_Click(object sender, RoutedEventArgs e)
        {
            Difficulty DiffWn = new Difficulty();
            DiffWn.ShowDialog();

            // Nothing chosen, nothing happens
            if (DiffWn.Diff == null)
            {
                return;
            }

            int digits;
            switch (DiffWn.Diff)
            {
                case Level.Easy:
                    digits = 3;
                    break;
                default:
                case Level.Normal:
                    digits = 4;
                    break;
                case Level.Hard:
                    digits = 8;
                    break;
            }

            PLAYER_NUM = GetValidNumber(digits);
            NPC_NUM = GetValidNumber(digits);

            while (PLAYER_NUM == NPC_NUM)
                NPC_NUM = GetValidNumber(digits);


            SessionStart();
        }
    }
}
