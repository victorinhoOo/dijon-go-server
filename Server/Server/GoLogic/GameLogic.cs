using GoLogic.Goban;

namespace GoLogic
{
    /// <summary>
    /// Gére les actions et règles de base du go
    /// </summary>
    public class GameLogic
    {
        #region attributs
        private bool isEndGame;
        private bool skippedTurn;
        private Stone? previousStone;
        private IBoard goban;
        private IMoveValidator moveValidator;
        private CaptureManager captureManager;

        /// <summary>
        /// Le plateau de la partie
        /// </summary>
        public IBoard Goban { get => this.goban; }
        
        /// <summary>
        /// Tour actuel, Noir ou Blanc
        /// </summary>
        public StoneColor CurrentTurn { get => this.goban.CurrentTurn; }

        /// <summary>
        /// True si la partie est finie
        /// </summary>
        public bool IsEndGame { get => this.isEndGame; }

        /// <summary>
        /// Coup précédemment joué
        /// </summary>
        public Stone? PreviousStone { get => this.previousStone; }
        #endregion attributs

        /// <summary>
        /// Gére toute la logique et règles du jeu
        /// </summary>
        /// <param name="board">Le tableau contenant les pierres</param>
        public GameLogic(IBoard board)
        {
            this.goban = board;
            this.captureManager = new CaptureManager(board);
            this.moveValidator = new MoveValidator(board);
        }

        /// <summary>
        /// Passe le tour
        /// </summary>
        public void SkipTurn()
        {
            // si le tour à été précédemment passé la partie est finie
            if (this.skippedTurn) this.isEndGame = true;

            this.goban.NextTurn();
            this.skippedTurn = true;
        }
        
        /// <summary>
        /// Vérifie et place une pierre sur le plateau si possible
        /// </summary>
        /// <param name="x">Position ligne x dans le plateau</param>
        /// <param name="y">Position colonne y dans le plateau</param>
        /// <exception cref="InvalidOperationException">L'emplacement de la pierre n'est pas valide</exception>
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
                if (isEndGame) 
                {
                    throw new InvalidOperationException("Impossible move because game was ended");
                }
                // place la pierre en changeant sa couleur de Empty à CurrentTurn
                this.goban.PlaceStone(stone, this.goban.CurrentTurn); 
                
                // capture les pierres capturables suite au placement de la pierre
                this.captureManager.CapturesOpponent(stone);

                // la pierre précédente deviens la pierre actuel 
                this.previousStone = stone;
                this.goban.NextTurn(); // tour passe au joueur suivant

                res = true;
            }
            
            return res;
        }

        /// <summary>
        /// Vérifie et retourne une liste des positions Ko sur le plateau
        /// Une case est Ko si le coup remet le plateau dans son état précédent
        /// </summary>
        /// <param name="currentTurn">Tour du joueur actuel, noir ou blanc</param>
        /// <returns>List des pierres en situation de ko</returns>
        public List<Stone> ChecksGobanForKo(StoneColor currentTurn)
        {
            List<Stone> potentialKoPositions = new List<Stone>();

            // Ne vérifie que si la pierre précédante existe
            if (this.previousStone != null)
            {
                // Récupère tous les voisins vides de la pierre précédente
                foreach (Stone stone in this.goban.GetNeighbors(this.previousStone))
                {
                    if (stone.Color == StoneColor.Empty)
                    {
                        // Pour chaque voisin vide, fait une nouvelle copie et teste
                        IBoard boardCopy = this.goban.Clone();
                        CaptureManager captureManagerCopy = new CaptureManager(boardCopy);
                        Stone stoneCopy = boardCopy.GetStone(stone.X, stone.Y);

                        // Essaie de placer une pierre de la couleur du joueur actuel
                        stoneCopy.ChangeColor(currentTurn);

                        // Capture toutes les pierres adverses
                        captureManagerCopy.CapturesOpponent(stoneCopy);

                        // Vérifie si cela crée une situation de Ko
                        if (boardCopy.IsKoViolation())
                        {
                            potentialKoPositions.Add(stone);
                        }
                    }
                }
            }

            return potentialKoPositions;
        }
    }
}

