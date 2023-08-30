using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameBoard game;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            game = new GameBoard(15, 15, 25);
            this.RedrawBoard();
        }

        private void RedrawBoard()
        {
            gameGrid.Children.Clear();

            var rows = new StackPanel();
            rows.Orientation = Orientation.Vertical;

            for (var Y = 0; Y < game.boardHeight; Y++)
            {
                var columns = new StackPanel();
                columns.Orientation = Orientation.Horizontal;

                for (var X = 0; X < game.boardWidth; X++)
                {
                    var cell = game.Board[Y, X];

                    if (cell.revealed == false)
                    {
                        this.AddButton(columns, cell);
                    }
                    else
                    {
                        this.AddLabel(columns, cell);
                    }
                }

                rows.Children.Add(columns);
            }

            gameGrid.Children.Add(rows);
        }

        private void AddButton(StackPanel panel, Cell cell)
        {
            var button = new Button();

            button.Width = 40;
            button.Height = 40;
            button.Tag = game.Board[cell.YCoordinate, cell.XCoordinate];
            button.Click += Button_Click;

            if (cell.State == State.Flagged || cell.State == State.BombFlag)
            {
                button.Content = "F";
            }

            button.MouseRightButtonUp += Button_MouseRightButtonUp;
            panel.Children.Add(button);
        }

        private void AddLabel(StackPanel panel, Cell cell)
        {
            var label = new Label();

            var neighbours = cell.GetTotalSurroundingBombs();

            if(neighbours > 0)
            {
                label.Content = neighbours;

                if(neighbours == 1)
                {
                    label.Foreground = new SolidColorBrush(Colors.Blue);
                }
                else if (neighbours == 2)
                {
                    label.Foreground = new SolidColorBrush(Colors.Green);
                }
                else if (neighbours == 3)
                {
                    label.Foreground = new SolidColorBrush(Colors.Red);
                }
                else if (neighbours == 4)
                {
                    label.Foreground = new SolidColorBrush(Colors.DarkBlue);
                }
            }

            if(cell.State == State.Bomb || cell.State == State.BombFlag)
            {
                label.Foreground = new SolidColorBrush(Colors.Black);
                label.Content = "B";
            }
            
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            label.VerticalContentAlignment = VerticalAlignment.Center;
            label.FontSize = 15;
            label.BorderThickness = new Thickness(1, 1, 1, 1);
            label.BorderBrush = new SolidColorBrush(Colors.Black);
            label.Width = 40;
            label.Height = 40;
            panel.Children.Add(label);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var currentButton = (Button)sender;
            var cell = currentButton.Tag as Cell;

            cell.FlipCell();

            RedrawBoard();
        }

        private void Button_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var currentButton = (Button)sender;
            var cell = currentButton.Tag as Cell;

            if (cell.State == State.Flagged || cell.State == State.BombFlag)
                cell.UnFlag();
            else
                cell.Flag();

            RedrawBoard();
        }
    }
}
