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

namespace NumberWars
{
    public partial class Difficulty : Window
    {
        public Level? Diff { get; set; }

        public Difficulty()
        {
            InitializeComponent();
            this.Diff = null;
        }

        private void Easy_Click(object sender, RoutedEventArgs e)
        {
            Close();
            this.Diff = Level.Easy;
        }

        private void Normal_Click(object sender, RoutedEventArgs e)
        {
            Close();
            this.Diff = Level.Normal;
        }

        private void Hard_Click(object sender, RoutedEventArgs e)
        {
            Close();
            this.Diff = Level.Hard;
        }
    }
}
