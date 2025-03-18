using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool firstClick = true;
        private bool end = false;
        private int rows = 15;
        private int columns = 20;
        private int mines = 30;
        private Field[,] mineField;
        private System.Timers.Timer timer;

        public MainWindow()
        {
            InitializeComponent();

            timer = new System.Timers.Timer();
            timer.Elapsed += Timer_Tick;
            timer.Interval = 1000;

            Board.Rows = rows;
            Board.Columns = columns;
            FlagCounter.Text = mines.ToString();

            mineField = new Field[columns, rows];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Button button = new Button();
                    button.Width = 30;
                    button.Height = 30;
                    button.Margin = new Thickness(1);
                    button.Click += Field_Click;
                    button.MouseRightButtonUp += Field_Right_Click;
                    Board.Children.Add(button);
                    mineField[j, i] = new Field(button, j, i, mineField);
                }
            }

            

        }

        // click on a field
        private void Field_Click(object sender, RoutedEventArgs e)
        {
            if (end)
                return;


            Button button = (Button)sender;
            (int x, int y) buttonCoordinates = (Board.Children.IndexOf(button) % columns, Board.Children.IndexOf(button) / columns);


            if (firstClick)
            {
                GenerateMines(buttonCoordinates.x, buttonCoordinates.y);
                timer.Enabled = true;
            }
            firstClick = false;

            // clicked on a mine
            if (mineField[buttonCoordinates.x, buttonCoordinates.y].RevealField())
            {
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        if (mineField[i, j].IsMine)
                            mineField[i, j].RevealField();
                    }
                }
                timer.Enabled = false;
                end = true;
            }
            


            int revealedFieldsCount = 0;
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (mineField[i, j].IsRevealed)
                        revealedFieldsCount++;
                }
            }

            // all fields revealed
            if (revealedFieldsCount == rows * columns - mines)
            {
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        if (!mineField[i, j].IsRevealed && !mineField[i, j].IsFlag)
                            mineField[i, j].ToggleFlag();
                    }
                }
                FlagCounter.Text = "0";
                timer.Enabled = false;
            }

        }

        // right click on a field
        private void Field_Right_Click(object sender, RoutedEventArgs e)
        {
            if (end)
                return;

            Button button = (Button)sender;
            (int x, int y) buttonCoordinates = (Board.Children.IndexOf(button) % columns, Board.Children.IndexOf(button) / columns);

            if (mineField[buttonCoordinates.x, buttonCoordinates.y].IsFlag)
            {
                FlagCounter.Text = (Int32.Parse(FlagCounter.Text) + 1).ToString();
            }
            else
            {
                FlagCounter.Text = (Int32.Parse(FlagCounter.Text) - 1).ToString();
            }

            mineField[buttonCoordinates.x, buttonCoordinates.y].ToggleFlag();
        }


        private void GenerateMines(int x, int y)
        {
            int fields = columns * rows - 9; // starting coordinate + neighbor fields
            if ((x == 0 && y == 0) || (x == 0 && y == rows - 1) || (x == columns - 1 && y == rows - 1) || (x == columns - 1 && y == 0))  // corners
                fields += 5;
            else if (x == 0 || x == columns - 1 || y == 0 || y == rows - 1) // edges
                fields += 3;
            int minesToPlace = mines;
            Random random = new Random();
            for (int i = 0; i < rows; i++) // row
            {
                for (int j = 0; j < columns; j++) // column
                {
                    if (i >= y - 1 && i <= y + 1 &&
                        j >= x - 1 && j <= x + 1)
                    {
                        continue;
                    }
                    if (random.Next(fields) < minesToPlace)
                    {
                        mineField[j, i].IsMine = true;

                        minesToPlace--;
                    }

                    fields--;
                }
            }
        }

        // click on the "restart" button
        private void Restart_Click(object? sender, RoutedEventArgs? e)
        {
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    mineField[i, j].Restart();
                }
            }
            FlagCounter.Text = mines.ToString();
            TimerCounter.Text = "0";
            firstClick = true;
            end = false;
        }

        private void Timer_Tick(object? sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                TimerCounter.Text = (Int32.Parse(TimerCounter.Text) + 1).ToString();
            }));
        }

        private void CustomSettings_Click(object sender, RoutedEventArgs e)
        {
            CustomSettings customSettings = new CustomSettings();
            customSettings.ShowDialog();

            if (customSettings.DialogResult == true)
            {
                rows = customSettings.Rows;
                columns = customSettings.Columns;
                mines = customSettings.Mines;

                Board.Children.Clear();
                Board.Rows = rows;
                Board.Columns = columns;
                FlagCounter.Text = mines.ToString();

                mineField = new Field[columns, rows];

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        Button button = new Button();
                        button.Width = 30;
                        button.Height = 30;
                        button.Margin = new Thickness(1);
                        button.Click += Field_Click;
                        button.MouseRightButtonUp += Field_Right_Click;
                        Board.Children.Add(button);
                        mineField[j, i] = new Field(button, j, i, mineField);
                    }
                }

                FlagCounter.Text = mines.ToString();
                TimerCounter.Text = "0";
                firstClick = true;
                end = false;
            }
        }
    }
}