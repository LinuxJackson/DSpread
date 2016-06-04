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

namespace DSpread
{
    /// <summary>
    /// Starting.xaml 的交互逻辑
    /// </summary>
    public partial class Starting : Window
    {
        public Starting()
        {
            InitializeComponent();
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.Foreground = new SolidColorBrush(Colors.Gray);
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            MainWindow mw = new MainWindow();
            mw.Show();
        }
    }
}
