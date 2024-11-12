using GoLogic;

GameBoard board = new GameBoard(9);
GameLogic Go = new GameLogic(board);

void PrintBoard(Stone[,] board)
{
    int rows = board.GetLength(0);
    int cols = board.GetLength(1);

    for (int i = 0; i < rows; i++)
    {
        for (int j = 0; j < cols; j++)
        {
            string st = ".";
            switch (board[i, j].Color)
            {
                case StoneColor.Black:
                    st = "@";
                    break;
                case StoneColor.White:
                    st = "O";
                    break;
                case StoneColor.Empty:
                    st = ".";
                    break;
            }
            Console.Write(st + " ");
        }
        Console.WriteLine();
    }
}
Console.WriteLine(". : vide, @ : noir, O : blanc");
Console.WriteLine("");

Go.PlaceStone(1, 2); // nr
Go.PlaceStone(2, 2); // bc
Go.PlaceStone(2, 1); // nr
Go.PlaceStone(2, 4); // bc
Go.PlaceStone(3, 2); // nr
Go.PlaceStone(3, 3); // bc
Go.PlaceStone(8, 3); // nr
Go.PlaceStone(1, 3); // bc

Go.PlaceStone(2, 3); // nr capture bc en (2, 2)  PreviousBoard
PrintBoard(Go.Board.Board);
Console.WriteLine("");
Go.PlaceStone(2, 2); // bc refusé par la régle de ko (normalement)  Board




