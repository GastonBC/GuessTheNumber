using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace GuessTheNumber
{
    public partial class MainWindow : Window
    {
        private void WriteLn(string text) // Invokes the console.text from the new thread and appends the line
        {

            ConsoleLog.Dispatcher.BeginInvoke(new Action(() =>
            { ConsoleLog.Text += text + Environment.NewLine; }));
        }

        private void WriteGuessLn(string text) // Invokes the console.text from the new thread and appends the line
        {

            GuessLog.Dispatcher.BeginInvoke(new Action(() =>
            { GuessLog.Text += text + Environment.NewLine; }));
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
