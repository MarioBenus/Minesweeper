using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Minesweeper
{
    internal class Field
    {
        public bool IsMine { get; set; } = false;
        public bool IsRevealed { get; set; } = false;
        public bool IsFlag { get; private set; } = false;
        public Button Button { get; set; }
        public int X {  get; set; }
        public int Y {  get; set; }
        private Field[,] MineField { get; set; }

        public Field(Button button, int x, int y, Field[,] fieldGrid)
        {
            Button = button;
            X = x;
            Y = y;
            MineField = fieldGrid;
        }

        public bool RevealField()
        {
            if (IsFlag)
            {
                return false;
            }


            if (IsMine)
            {
                Image image = new Image();
                image.Source = new BitmapImage(new Uri("https://cdn1.iconfinder.com/data/icons/office-boss-3/85/boss-star-badge-achievement-512.png"));
                StackPanel stackPanel = new StackPanel();
                stackPanel.Children.Add(image);
                Button.Content = stackPanel;
                
                Button.Background = Brushes.Red;
                return true;
            }

            IsRevealed = true;
            Button.IsEnabled = false;
            int neighborMines = CountNeighborMines();
            if (neighborMines == 0)
            {
                // neighbor fields
                // ternary operators make sure it stays within bounds
                for (int i = X - 1 > -1 ? X - 1 : 0; i <= (X + 1 < MineField.GetLength(0) - 1 ? X + 1 : MineField.GetLength(0) - 1); i++) // columns
                {
                    for (int j = Y - 1 > -1 ? Y - 1 : 0; j <= (Y + 1 < MineField.GetLength(1) - 1 ? Y + 1 : MineField.GetLength(1) - 1); j++) // rows
                    {
                        if (i == X && j == Y)
                        {
                            continue;
                        }
                        if (!MineField[i, j].IsRevealed)
                            MineField[i, j].RevealField();
                    }
                }
            }
            else
            {
                Button.Content = neighborMines;
            }
            Button.IsEnabled = false;
            return false;
        }

        private int CountNeighborMines()
        {
            int count = 0;
            // neighbor fields
            // ternary operators make sure it stays within bounds
            for (int i = X - 1 > -1 ? X - 1 : 0; i <= (X + 1 < MineField.GetLength(0) - 1 ? X + 1 : MineField.GetLength(0) - 1); i++) // columns
            {
                for (int j = Y - 1 > -1 ? Y - 1 : 0; j <= (Y + 1 < MineField.GetLength(1) - 1 ? Y + 1 : MineField.GetLength(1) - 1); j++) // rows
                {
                    if (MineField[i, j].IsMine)
                        count++;
                }
            }
            return count;
        }

        public void ToggleFlag()
        {
            if (!IsFlag)
            {
                Image image = new Image();
                image.Source = new BitmapImage(new Uri("https://cdn0.iconfinder.com/data/icons/basic-ui-elements-flat/512/flat_basic_home_flag_-512.png"));
                StackPanel stackPanel = new StackPanel();
                stackPanel.Children.Add(image);
                Button.Content = stackPanel;
                IsFlag = true;
            }
            else
            {
                IsFlag = false;
                Button.Content = "";
            }
            
        }

        public void Restart()
        {
            IsFlag = false;
            Button.Content = "";
            IsMine = false;
            IsRevealed = false;
            Button.IsEnabled = true;
            Button.Background = (SolidColorBrush) new BrushConverter().ConvertFrom("#ffdddddd")!;
        }
    }
}
