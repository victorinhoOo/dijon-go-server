using GoLogic;
using GoLogic.Score;
using System.Text;
using System.Text.Json;
using ZstdSharp.Unsafe;

namespace WebSocket.Model
{
    /// <summary>
    /// Classe qui représente une partie de jeu de go
    /// </summary>
    public class Game
    {
        private Client player1;
        private Client player2;
        private Client currentTurn;
        private GameBoard gameBoard;
        private GameLogic logic;
        private ScoreRule score;
        private int size;
        private int id;

        /// <summary>
        /// Proprité qui indique si la partie est pleine
        /// </summary>
        public bool IsFull
        {
            get
            {
                return player1 != null && player2 != null;
            }
        }

        /// <summary>
        /// Récupérer ou modifier le joueur 1
        /// </summary>
        public Client Player1 { get => player1; set => player1 = value; }


        /// <summary>
        /// Récupérer ou modifier le joueur 2
        /// </summary>
        public Client Player2 { get => player2; set => player2 = value; }


        /// <summary>
        /// Récupérer ou modifier le joueur qui doit jouer
        /// </summary>
        public Client CurrentTurn { get => currentTurn; }


        /// <summary>
        /// Récupérer ou modifier la taille du plateau
        /// </summary>
        public int Size { get => size; set => size = value; }


        /// <summary>
        /// Récupérer ou modifier l'identifiant de la partie
        /// </summary>
        public int Id { get => id; set => id = value; }


        /// <summary>
        /// Constructeur de la classe Game
        /// </summary>
        /// <param name="player1">Joueur qui créé la partie</param>
        public Game(Client player1)
        {
            id = Server.Games.Count + 1;
            size = 19;
            this.player1 = player1;
            currentTurn = player1;
            gameBoard = new GameBoard(size);
            logic = new GameLogic(gameBoard);
            score = new ChineseScoreRule(gameBoard);
        }


        /// <summary>
        /// Ajouter un joueur à la partie
        /// </summary>
        /// <param name="player2">Joueur à ajouter</param>
        public void AddPlayer(Client player2)
        {
            if (this.player2 == null)
            {
                this.player2 = player2;
            }
            else
            {
                // todo : throw exception
            }
        }


        /// <summary>
        /// Placer une pierre sur le plateau
        /// </summary>
        /// <param name="x">Coordonées en x de la pierre</param>
        /// <param name="y">Coordonnées en y de la pierre</param>
        public void PlaceStone(int x, int y)
        {
            logic.PlaceStone(x, y);
        }


        /// <summary>
        /// Changement de tour
        /// </summary>
        public void ChangeTurn()
        {
            currentTurn = currentTurn == player1 ? player2 : player1;
        }


        /// <summary>
        /// Conversiond de l'état de la partie en chaine de caractère
        /// </summary>
        /// <returns>état de la partie en string</returns>
        public string StringifyGameBoard()
        {
            GameBoard copy = new GameBoard(gameBoard.Size);
            copy.Board = gameBoard.CopyBoard();
            CheckGobanForKo(copy);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("x,y,color");
            foreach (Stone stone in copy.Board)
            {
                sb.AppendLine($"{stone.X},{stone.Y},{stone.Color}");
            }
            return sb.ToString();
        }

        private void CheckGobanForKo(GameBoard board)
        {
            logic.ChecksGobanForKo(board, logic.CurrentTurn);
        }

        /// <summary>
        /// Récupérer le score de la partie
        /// </summary>
        /// <returns>Score de la partie sous forme de tuple</returns>
        public (int, int) GetScore()
        {
            return score.CalculateScore();
        }
        
        /// <summary>
        /// Récupère les pierres noires et blanches capturées
        /// </summary>
        /// <returns>Tuple d'entier des pierres noires et blanches</returns>
        public (int, int) GetCapturedStone()
        {
            return (gameBoard.CapturedBlackStones, gameBoard.CapturedWhiteStones);
        }


        /// <summary>
        /// Passer son tour
        /// </summary>
        public void SkipTurn()
        {
            logic.SkipTurn();
            ChangeTurn();
        }


        /// <summary>
        /// Test si la partie est terminée
        /// </summary>
        /// <returns>True si la partie est terminée, False sinon</returns>
        public bool TestWin()
        {
            return logic.IsEndGame;
        }
    }
}
