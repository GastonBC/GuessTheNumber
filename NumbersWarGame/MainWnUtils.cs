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
            // Check if all chars are numbers
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
        private void SessionStart(int Digits)
        {
            STEPS = 0;
            string Asterix = "";

            Enemy = new Enemy(Digits);

            foreach (char ch in Enemy.Code)
            {
                Asterix += "* ";
            }


            tb_NPCNumber.Text = Asterix;
            tb_PlayerNumber.Text = PlayerCode;

            tb_NumberTry.IsEnabled = true;
            tb_PlayerNumber.IsEnabled = false;

            b_Guess.IsEnabled = true;
            b_ConfirmPlayer.IsEnabled = false;
            b_GiveUp.IsEnabled = true;
            b_RandomDiff.IsEnabled = false;

#if DEBUG
            b_ConfirmPlayer.IsEnabled = true;
            tb_NPCNumber.Text = Enemy.Code;
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
                Line line = new Line
                {
                    Stroke = new SolidColorBrush(Colors.DarkRed),
                    StrokeThickness = 3,
                    X1 = currentPoint.X,
                    Y1 = currentPoint.Y,
                    X2 = e.GetPosition(paintSurface).X,
                    Y2 = e.GetPosition(paintSurface).Y
                };

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
