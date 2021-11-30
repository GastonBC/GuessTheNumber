using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NumbersWarGame
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
        private bool RunNumberChecks(string NUMBER, bool ShowExceptions)
        {
            // First check if first digit is 0
            if (NUMBER.First().ToString() == "0")
            {
                if (ShowExceptions) WriteLn("Your number cannot begin with a 0");
                return false;
            }

            // And if all chars are numbers
            for (int i = 0; i < NUMBER.Length; i++)
            {
                if (!Char.IsDigit(NUMBER[i]))
                {
                    if (ShowExceptions) WriteLn("All characters must be digits");
                    return false;
                }
            }

            // And if a number is repeated
            foreach (char ch in NUMBER)
            {
                int freq = NUMBER.Count(f => (f == ch));

                if (freq > 1)
                {
                    if (ShowExceptions) WriteLn("You can only use a digit once");
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
            int max = Convert.ToInt32((1 * (Math.Pow(10d, digits))) - 1);  // 1* (10^x) - 1.  eg: 1*(10^5)-1 = 99.999

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
        /// Provides the answer to the number guessed, needs two out variables to store the answers
        /// </summary>
        private void AnswerToNumber(string Number, out int GoodAmmount, out int RegularAmmount)
        {
            GoodAmmount = 0;
            RegularAmmount = 0;

            // First check good numbers. Check player index num against same index on npc
            for (int i = 0; i < Number.Length; i++)
            {
                if (Enemy.ChosenNumber[i] == Number[i])
                {
                    GoodAmmount++;
                }
            }

            // Then check regular numbers. Check each index agains all indexes of NPC_NUM
            for (int i = 0; i < Number.Length; i++)
            {
                for (int n = 0; n < Number.Length; n++)
                {
                    // If it's a different index (that'd be a good number) and it's the
                    // same number then increment a regular
                    if (i != n && Enemy.ChosenNumber[i] == Number[n])
                    {
                        RegularAmmount++;
                    }

                }
            }
        }

        private void GameFinished(bool Won, int Steps)
        {//STAT_MostStepsTakenToWin
            if (Won)
            {
                string _GamesWon = ConfigurationManager.AppSettings["STAT_GamesWon"];
                int GamesWon = int.Parse(_GamesWon);

                AddOrUpdateAppSettings("STAT_GamesWon", GamesWon++.ToString());
            }

            else
            {
                string _GamesLost = ConfigurationManager.AppSettings["STAT_GamesLost"];
                int GamesLost = int.Parse(_GamesLost);

                AddOrUpdateAppSettings("STAT_GamesWon", GamesLost++.ToString());
            }

            string _MostSteps = ConfigurationManager.AppSettings["STAT_MostStepsTakenToWin"];
            int MostSteps = int.Parse(_MostSteps);

            string _LessSteps = ConfigurationManager.AppSettings["STAT_LessStepsTakenToWin"];
            int LessSteps = int.Parse(_LessSteps);

            if (Steps > MostSteps)
            {
                AddOrUpdateAppSettings("STAT_MostStepsTakenToWin", Steps.ToString());
            }

            else if (Steps < LessSteps)
            {
                AddOrUpdateAppSettings("STAT_LessStepsTakenToWin", Steps.ToString());
            }
        }

        /// <summary>
        /// Freeze not needed buttons, unless you are debugging
        /// </summary>
        private void SessionStart()
        {
            STEPS = 0;
            string Asterix = "";

            Enemy = new EnemyAI(PLAYER_NUM);

            foreach (char ch in Enemy.ChosenNumber)
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
            tb_NPCNumber.Text = Enemy.ChosenNumber;
#endif
        }

        private void Canvas_MouseDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                currentPoint = e.GetPosition(paintSurface);
        }

        private void Canvas_MouseMove_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Line line = new Line();

                line.Stroke = new SolidColorBrush(Colors.DarkRed);
                line.StrokeThickness = 3;
                line.X1 = currentPoint.X;
                line.Y1 = currentPoint.Y;
                line.X2 = e.GetPosition(paintSurface).X;
                line.Y2 = e.GetPosition(paintSurface).Y;

                currentPoint = e.GetPosition(paintSurface);

                paintSurface.Children.Add(line);
            }
        }


        internal static void AddOrUpdateAppSettings(string key, string value)
        {
            try
            {
#if DEBUG
                string applicationName =
                    Environment.GetCommandLineArgs()[0];
#else
           string applicationName =
          Environment.GetCommandLineArgs()[0]+ ".exe";
#endif

                string exePath = System.IO.Path.Combine(Environment.CurrentDirectory, applicationName);

                var configFile = ConfigurationManager.OpenExeConfiguration(exePath);
                var settings = configFile.AppSettings.Settings;

                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            { }
        }
    }
}
