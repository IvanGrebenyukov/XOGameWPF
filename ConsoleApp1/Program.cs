using ConsoleApp1;

XOField XOGame = new XOField();
XOGame.OnTurn += (row, col, elem) =>
{
    for (int i = 0; i < XOGame.Field.GetLength(0); i++)
    {
        for (int j = 0; j < XOGame.Field.GetLength(1); j++)
        {
            Console.Write(XOGame.Field[i, j] + " ");
        }
        Console.WriteLine();
    }
    Console.WriteLine("========");
};

XOGame.OnGameOver += (winner) =>
{
    Console.WriteLine("GAME OVER!");
    if (winner == XOElement.Cross)
    {
        Console.WriteLine("X WIN");
    } else if (winner == XOElement.Circle)
    {
        Console.WriteLine("O WIN");
    }
};


XOGame.TryTurn(0, 0);
XOGame.TryTurn(1, 0);
XOGame.TryTurn(0, 1);
XOGame.TryTurn(1, 1);
XOGame.TryTurn(0, 2);
XOGame.TryTurn(1, 2);

