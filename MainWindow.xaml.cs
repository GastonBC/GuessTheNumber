using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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



        private void Start_Click(object sender, RoutedEventArgs e)
        {
            // A new background thread is needed to process the files. The window thread remains available for the updates
            Thread t = new Thread(() =>
            {

                try
                {
                  
                }



                catch (Exception ex)
                {
                    WriteLn("--------------------------");
                    WriteLn(ex.GetType().ToString());
                    WriteLn(ex.Message);
                    WriteLn("");
                    WriteLn(ex.StackTrace);
                    WriteLn("--------------------------");


                    b_Guess.Dispatcher.BeginInvoke(new Action(() =>
                    { b_Guess.IsEnabled = true; }));
                }

            });

            t.Start();
        }



        private void WriteLn(string text) // Invokes the console.text from the new thread and appends the line
        {

            ConsoleLog.Dispatcher.BeginInvoke(new Action(() =>
            { ConsoleLog.Text += text + Environment.NewLine; }));
        }

        private void onTextChange(object sender, TextChangedEventArgs e)
        {

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
            WriteLn(tb_NumberTry.Text);
        }

        private void ConfirmNum_Click(object sender, RoutedEventArgs e)
        {
            // First check if first digit is 0
            if (tb_PlayerNumber.Text.First().ToString() == "0")
            {
                WriteLn("Your number cannot begin with a 0");
                return;
            }

            PLAYER_NUM = tb_PlayerNumber.Text;


            int min = Convert.ToInt32(1 * (Math.Pow(10d, PLAYER_NUM.Length - 1)));    // 1* (10^x-1) - 1.  eg: 1*(10^4) = 10.000
            int max = Convert.ToInt32( (1 * (Math.Pow(10d, PLAYER_NUM.Length))) - 1); // 1* (10^x) - 1.  eg: 1*(10^5)-1 = 99.999

            Random rd = new Random();
            NPC_NUM = rd.Next(min, max).ToString();

            tb_NPCNumber.Text = NPC_NUM;
        }
    }
}
