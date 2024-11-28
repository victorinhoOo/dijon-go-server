using GoLogic.Goban;

namespace GoLogic
{
    /// <summary>
    /// Gére les actions et règles de base du go
    /// </summary>
    public class GameLogic
    {
        #region attributs
        private IBoard goban;
        private bool isEndGame;
        private bool skippedTurn;
        private Stone? previousStone;
        private IMoveValidator moveValidator;
        private CaptureManager captureManager;

        /// <summary>
        /// Le plateau de la partie
        /// </summary>
        public IBoard Goban { get => this.goban; }
        
        /// <summary>
        /// Tour actuel, Noir ou Blanc
        /// </summary>
        public StoneColor CurrentTurn { get => this.goban.CurrentTurn; set => this.goban.CurrentTurn = value; }

        /// <summary>
        /// True si la partie est finie
        /// </summary>
        public bool IsEndGame { get => this.isEndGame; }

        /// <summary>
        /// Coup précédemment joué
        /// </summary>
        public Stone PreviousStone { get => this.previousStone; }
        #endregion attributs

        /// <summary>
        /// Gére toute la logique et règles du jeu
        /// </summary>
        /// <param name="board">Le tableau contenant les pierres</param>
        public GameLogic(IBoard board)
        {
            this.goban = board;
            this.captureManager = new CaptureManager(board);
            this.moveValidator = new MoveValidator(board, this.captureManager);
        }

        /// <summary>
        /// Passe le tour
        /// </summary>
        public void SkipTurn()
        {
            if (this.skippedTurn) this.isEndGame = true;
            CurrentTurn = CurrentTurn == StoneColor.Black ? StoneColor.White : StoneColor.Black;
            this.skippedTurn = true;
        }
        
        /// <summary>
        /// Vérifie et place une pierre sur le plateau si possible
        /// </summary>
        /// <param name="x">Position ligne x dans le plateau</param>
        /// <param name="y">Position colonne y dans le plateau</param>
        /// <returns>Vraie si la pierre a pu être placé, faux sinon</returns>
        public bool PlaceStone(int x, int y)
        {
            bool res = false;

            this.skippedTurn = false;
            Stone stone = this.goban.GetStone(x, y); // récupère la pierre aux coordonnées données

            if (!this.moveValidator.IsValidMove(stone))
            {
                throw new InvalidOperationException($"Move at ({x}, {y}) is not valid.");
            }
            else
            {
                this.goban.ChangeStoneColor(stone.X, stone.Y, CurrentTurn); // place la pierre en changeant sa couleur de Empty à CurrentTurn
                stone = this.goban.GetStone(x,y);
                this.captureManager.CapturesOpponent(stone);
                this.previousStone = stone;
                CurrentTurn = CurrentTurn == StoneColor.Black ? StoneColor.White : StoneColor.Black; // tour passe au joueur suivant

                res = true;
            }
            
            return res;
        }
        
    }
}

