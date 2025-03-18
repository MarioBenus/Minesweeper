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

namespace Minesweeper
{
    /// <summary>
    /// Interaction logic for CustomSettings.xaml
    /// </summary>
    public partial class CustomSettings : Window
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int Mines { get; set; }

        public CustomSettings()
        {
            InitializeComponent();

        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            int rows;
            int columns;
            int mines;
            if (Int32.TryParse(RowsTextBox.Text, out rows) &&
                Int32.TryParse(ColumnsTextBox.Text, out columns) &&
                Int32.TryParse(MinesTextBox.Text, out mines))
            {
                
                if (rows < 5 || columns < 5) 
                {
                    ErrorLine.Text = "Grid must be at least size 5x5!";
                    return;
                }
                if (mines < 1)
                {
                    ErrorLine.Text = "There must be at least 1 mine!";
                    return;
                }
                if (mines > rows * columns - 9) 
                {
                    ErrorLine.Text = "There must be at least 9 fields without a mine!";
                    return;
                }
                Rows = rows;
                Columns = columns;
                Mines = mines;
                this.DialogResult = true;
                this.Close();
            }

            ErrorLine.Text = "Error parsing text into numbers!";
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
