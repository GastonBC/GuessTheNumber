using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NumbersWarGame
{
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();

            tb_Block.Text =
                "How to play:\n" +
                "· You need to guess the other player's number. On your turn write a number and the opponent will tell you how many Good digits and how many Regular digits you have.\n" +
                "· A Good digit means one of your number's digits is correct and is in the correct location.\n" +
                "· A Regular digit means one of your number's digits is correct, but is in the wrong location.\n\n" +
                "eg: If your opponent number is 1234 and your guess is 5261 you will get 1G - 1R because your 2 is in the correct location but 1 is not.\n" +
                "Those are your clues to guess your opponent's number.\n\n" +

                "Rules\n" +
                "· Minimum number length is 2, maximum is 10\n" +
                "· Numbers can not start with a 0\n" +
                "· Digits in the number can not repeat (eg 1231 is wrong)\n" +
                "· Number can not contain letters\n";
        }
    }
}
