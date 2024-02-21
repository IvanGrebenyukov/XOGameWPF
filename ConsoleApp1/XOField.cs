using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{

    //rows  ->
    //cols |
    public enum XOElement { None = 0, Cross = 1, Circle = -1 }

    public class XOField
    {
        public event Action<XOElement> OnGameOver;

        public event Action<int, int, XOElement> OnTurn;

        private const int rowCount = 3, colCount = 3;

        private XOElement[,] rawValues = new XOElement[rowCount, colCount];

        private XOElement lastTurn = XOElement.Circle;

        private XOElement winner = XOElement.None;

        private static ((int startRow, int startCol),(int endRow,int endCol)) coordinatsWinner = ((0,0),(0,0));

        public XOElement Winner
        {
            get
            {
                return winner;
            }

            set
            {
                winner = value;

                if (winner != XOElement.None)
                    OnGameOver?.Invoke(winner);
            }

        }

        public char[,] Field
        {
            get
            {
                char[,] result = new char[rowCount, colCount];
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        switch (rawValues[i, j])
                        {

                            case XOElement.Cross:
                                result[i, j] = 'X';
                                break;
                            case XOElement.Circle:
                                result[i, j] = 'O';
                                break;
                            default:
                                result[i, j] = '-';
                                break;
                        }
                    }
                }
                return result;
            }
        }

        private XOElement reverseElement (XOElement element)
        {
            return (XOElement) (-(int) element);
        }

        public bool GameOver
        {
            get
            {
                return winner != XOElement.None;
            }
        }

        public void turnGPT () {
            if (GameOver)
                return;
            Random rnd = new Random();
            
            int row = rnd.Next(0, rowCount);
            int col = rnd.Next(0, colCount);
            while (!CanTurn(row, col)) {
                row = rnd.Next(0, rowCount);
                col = rnd.Next(0, colCount);
            }
            TryTurn(row, col);
        }
        public bool CanTurn (int row, int col)
        {
            return rawValues[row, col] == XOElement.None && !GameOver;
        }

        public bool TryTurn (int row, int col)
        {
            if (!CanTurn(row, col))
                return false;

            rawValues[row, col] = reverseElement(lastTurn);
            lastTurn = rawValues[row, col];
            OnTurn?.Invoke(row, col, lastTurn);

            Winner = checkWinner();

            return true;
        }

        private XOElement checkRows ()
        {
            for (int i = 0; i < rowCount; i++)
            {
                int sum = 0;
                int j = 0;
                for (j = 0; j < colCount; j++)
                {
                    sum += (int) rawValues[i, j];
                }

                if (sum == 3)
                {
                    coordinatsWinner = ((j - 3, i), (j - 1, i));
                    return XOElement.Cross;
                }
                if (sum == -3)
                {
                    coordinatsWinner = ((j - 3, i), (j - 1, i));
                    return XOElement.Circle;
                }
                    
            }
            return XOElement.None;
        }

        private XOElement checkCols ()
        {
            for (int j = 0; j < colCount; j++)
            {

                int sum = 0;
                int i = 0;
                for (i = 0; i < rowCount; i++)
                {
                    sum += (int) rawValues[i, j];
                }
                if (sum == 3) {
                    coordinatsWinner = ((j , i-3), (j, i-1));
                    return XOElement.Cross;
                }
                if (sum == -3) {
                    coordinatsWinner = ((j, i -3), (j, i -1));
                    return XOElement.Circle;
                }
            }
            return XOElement.None;
        }

        private XOElement checkDiagonal ()
        {
            int sum = 0;
            for (int i = 0; i < rowCount; i++)
            {
                sum += (int) rawValues[i, i];

                if (sum == 3) {
                    coordinatsWinner = ((0, 0), (i, i));
                    return XOElement.Cross;
                }
                if (sum == -3) {
                    coordinatsWinner = ((0, 0), (i, i));
                    return XOElement.Circle;
                }
            }

            sum = 0;
            int j = 0;
            for (int i = rowCount - 1; i >= 0; i--)
            {
                sum += (int) rawValues[i, j];
                j++;
                if (sum == 3) {
                    coordinatsWinner = ((rowCount-1, 0), (i, j-1));
                    return XOElement.Cross;
                }
                if (sum == -3) {
                    coordinatsWinner = ((rowCount - 1, 0), (i, j-1));
                    return XOElement.Circle;
                }
            }
            return XOElement.None;
        }

        private XOElement checkWinner ()
        {
            XOElement winner = checkRows();
            if (winner != XOElement.None)
                return winner;

            winner = checkCols();
            if (winner != XOElement.None)
                return winner;

            winner = checkDiagonal();
            if (winner != XOElement.None)
                return winner;

            return winner;
        }

        public static ((int startX, int startY), (int endX, int endY)) getFieldsWinner ()
        {
            return coordinatsWinner;
        }

        public void Reset ()
        {
            rawValues = new XOElement[rowCount, colCount];
            winner = XOElement.None;
            lastTurn = XOElement.Circle;
        }
    }
}
