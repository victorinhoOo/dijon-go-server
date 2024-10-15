using GoLogic;
using System.Text;
using System.Text.Json;

namespace WebSocket
{
    public class Game
    {
        private Client player1;
        private Client player2;
        private Client currentTurn;
        private GameBoard gameBoard;
        private GameLogic logic;    

        public bool IsFull
        {
            get
            {
                return this.player1 != null && this.player2 != null;
            }
        }

        public Client Player1 { get => player1; set => player1 = value; }
        public Client Player2 { get => player2; set => player2 = value; }
        public Client CurrentTurn { get => currentTurn; }

        public Game(Client player1)
        {
            this.player1 = player1;
            this.currentTurn = player1;
            this.gameBoard = new GameBoard(19);
            this.logic = new GameLogic(this.gameBoard);
        }

        public void AddPlayer(Client player2)
        {
            if(this.player2 == null)
            {
                this.player2 = player2;
            }
            else
            {
                // todo : throw exception
            }
        }

        public void PlaceStone(int x, int y)
        {
            this.logic.PlaceStone(x, y);
        }

        public void ChangeTurn()
        {
            this.currentTurn = this.currentTurn == this.player1 ? this.player2 : this.player1;
        }

        public string StringifyGameBoard()
        { 
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("x,y,color");
            foreach (Stone stone in this.gameBoard.Board)
            {
                sb.AppendLine($"{stone.X},{stone.Y},{stone.Color}");
            }
            return sb.ToString();
        }
    }
}
