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
using System.Windows.Navigation;
using System.Windows.Shapes;
using XODemo;
using ConsoleApp1;

namespace XODemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow :Window
    {
        private readonly double rectWidth;
        private readonly double rectHeight;

        private readonly XOField XOGame = new XOField();
        public MainWindow ()
        {
            InitializeComponent();

            rectHeight = gameField.Height / 3;
            rectWidth = gameField.Width / 3;

            printRectangles();

            XOGame.OnTurn += (row, col, elem) =>
            {
                char symbol = elem == XOElement.Cross ? 'X' : 'O';
                printSymbol(row, col, symbol);
            };

            XOGame.OnGameOver += (winner) =>
            {
                printVictoryLine();
                MessageBox.Show("GAME OVER!");
                if (winner == XOElement.Cross)
                {
                    MessageBox.Show("X WIN");
                } else if (winner == XOElement.Circle)
                {
                    MessageBox.Show("O WIN");
                }
            };

        }

        private void printRectangles() {
            double y = 0;
            for (int i = 1; i <= 3; i++) {

                double x = 0;
                for (int j = 1; j <= 3; j++) {
                    var rect = new Rectangle();
                    rect.Stroke = Brushes.Black;
                    rect.StrokeThickness = 1;
                    rect.Height = rectHeight;
                    rect.Width = rectWidth;

                    Canvas.SetLeft(rect, x);
                    Canvas.SetTop(rect, y);

                    gameField.Children.Add(rect);

                    x += rectWidth;
                }
                y += rectHeight;
            }
        }

        private (int,int) calcCell(double x,double y)
        {
            int row = (int) (y / rectHeight);
            int col = (int) (x / rectWidth);

            if (col >= 3)
            {
                col = 2;
            }

            if (row >= 3)
            {
                row = 2;
            }

            return (row, col);
        }

        private void printSymbol(int row,int col, char symbol)
        {
            (double cornerX, double cornerY) = (rectHeight * col, rectWidth * row);

            Label text = new Label();
            text.Content = symbol;
            text.FontSize = rectHeight / 1.5;

            Canvas.SetLeft(text, cornerX + rectWidth / 4);
            Canvas.SetTop(text, cornerY);
            gameField.Children.Add(text);
        }

        private void makeTurn(object sender, MouseButtonEventArgs e)
        {
            (int row, int col) = calcCell(e.GetPosition(gameField).X, e.GetPosition(gameField).Y);
            XOGame.TryTurn(row, col);
            XOGame.turnGPT();
        }

        private void newGame_menuItem_Click (object sender, RoutedEventArgs e)
        {
            XOGame.Reset();

            gameField.Children.Clear();
            printRectangles();
        }

        private void exit_menuItem_Click (object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void printVictoryLine ()
        {
            var coordinats = XOField.getFieldsWinner();
            Line line = new Line();
            line.Fill = Brushes.Red;
            line.Stroke = Brushes.Red;
            line.StrokeThickness = 1;
            line.X1 = rectHeight * (coordinats.Item1.startX)+rectWidth/2;
            line.Y1 = rectWidth * (coordinats.Item1.startY + 1)-rectHeight/2;
            line.X2 = rectHeight * (coordinats.Item2.endX) + rectWidth / 2;
            line.Y2 = rectWidth * (coordinats.Item2.endY + 1) - rectHeight / 2;
            gameField.Children.Add(line);

           
        }

        private void setCursorIcon (object sender, MouseEventArgs e) {
            (int row, int col) = calcCell(e.GetPosition(gameField).X, e.GetPosition(gameField).Y);
            if (XOGame.CanTurn(row,col)) {
                Cursor = Cursors.Hand;
            } else {
                Cursor = Cursors.No;
            }
        }
    }
}
