using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

/*
 TODO: 
        simplify main window, it's weird and slow and ugly now
        clean canvas button
        organize window with stacked panels
        add stats: won games, failed games, steps record, steps average
        save game state on closure
 */



namespace NumbersWarGame
{
    public partial class MainWindow : Window
    {
        Point currentPoint = new Point();

        string PlayerCode;
        int STEPS;
        Enemy Enemy;
        Difficulty DiffWn;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void Guess_Click(object sender, RoutedEventArgs e)
        {
            string PlayerGuess = tb_NumberTry.Text;

            // Check if its a valid number
            if (PlayerGuess == "" || RunNumberChecks(PlayerGuess, true) == false) return;

            int GoodAmmount;
            int RegularAmmount;
            Utils.AnswerToGuess(PlayerGuess, Enemy.Code, out GoodAmmount, out RegularAmmount);

            WriteGuessLn(tb_NumberTry.Text + " - " + $"{GoodAmmount}G - {RegularAmmount}R");
            STEPS++;

            // Win condition
            if (PlayerGuess == Enemy.Code)
            {
                tb_NPCNumber.Text = Enemy.Code;

                GameFinished(true, STEPS);

                WriteGuessLn("");
                WriteGuessLn($"You win! Number was {Enemy.Code}");
                WriteGuessLn($"Steps taken {STEPS}");
                FreezeCommands();
                return;
            }

            EnemyTurn(Enemy);
        }

        

        private void EnemyTurn(Enemy NPC)
        {
            WriteLn("Enemy turn!");

            NPC.MakeGuess();

            int GoodAmmount;
            int RegularAmmount;
            Utils.AnswerToGuess(NPC.LastGuess, PlayerCode, out GoodAmmount, out RegularAmmount);

            WriteLn(NPC.LastGuess + " - " + $"{GoodAmmount}G - {RegularAmmount}R");

            // Win condition
            if (PlayerCode == NPC.LastGuess)
            {
                tb_NPCNumber.Text = NPC.Code;

                GameFinished(true, STEPS);

                WriteLn("");
                WriteLn($"Foe wins! Number was {NPC.LastGuess}");
                WriteLn($"Steps taken {STEPS}");
                FreezeCommands();
                return;
            }

            NPC.Think(NPC.LastGuess, GoodAmmount, RegularAmmount);
        }

        private void ConfirmNum_Click(object sender, RoutedEventArgs e)
        {
            PlayerCode = tb_PlayerNumber.Text;

            // if checks fail
            if (!RunNumberChecks(PlayerCode, true))
            {
                return;
            }

            SessionStart(PlayerCode.Length);

        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow HelpWn = new HelpWindow();
            HelpWn.Show();
        }

        private void GiveUp_Click(object sender, RoutedEventArgs e)
        {
            tb_NPCNumber.Text = Enemy.Code;

            GameFinished(false, STEPS);

            WriteGuessLn("");
            WriteGuessLn($"Oh no! Number was {Enemy.Code}");
            FreezeCommands();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {

            GameFinished(false, STEPS);

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

            paintSurface.Children.Clear();
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
            if (tb_NumberTry.Text.Length == Enemy.Code.Length)
            {
                b_Guess.IsEnabled = true;
            }
            else
            {
                b_Guess.IsEnabled = false;
            }
        }

        private void RandomDiff_Click(object sender, RoutedEventArgs e)
        {
            DiffWn = new Difficulty();
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

            PlayerCode = Utils.GetValidNumber(digits);

            SessionStart(digits);
        }
    }
}
