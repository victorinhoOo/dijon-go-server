namespace GoLogic
{
    /// <summary>
    /// Classe représentation du Goban
    /// </summary>
    public class GameBoard
    {
        #region Attributs
        private Stone[,] board;
        private Stone[,] previousBoard; // Pour la règle de ko
        private int size;
        private int capturedBlackStones = 0;
        private int capturedWhiteStones = 0;
        
        /// <summary>
        /// Tableau qui contient les pierres du plateau
        /// </summary>
        public Stone[,] Board { get => this.board; }
        
        /// <summary>
        /// Le plateau dans l'état précédent
        /// </summary>
        public Stone[,] PreviousBoard { get => this.previousBoard; }
        
        /// <summary>
        /// La taille du plateau (size x size)
        /// </summary>
        public int Size { get => this.size; }

        /// <summary>
        /// Pierre noire qui ont été capturée
        /// </summary>
        public int CapturedBlackStones
        {
            get => this.capturedBlackStones;
            set => this.capturedBlackStones = value;
        }

        /// <summary>
        /// Pierre blanche qui ont été capturée
        /// </summary>
        public int CapturedWhiteStones
        {
            get => this.capturedWhiteStones;
            set => this.capturedWhiteStones = value;
        }
        #endregion Attributs
        
        /// <summary>
        /// Le plateau du jeu et ses pierres
        /// </summary>
        /// <param name="size">La taille du plateau size x size</param>
        public GameBoard(int size)
        {
            this.size = size;
            this.board = new Stone[size, size];
            this.previousBoard = new Stone[size, size];
            this.capturedBlackStones = 0;
            this.capturedWhiteStones = 0;
            InitializeBoard();
        }
    
        /// <summary>
        /// Initialise le plateau et ses pierres
        /// </summary>
        private void InitializeBoard()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    this.board[i, j] = new Stone(i, j); // Initialise les pierres Empty 
                    this.previousBoard[i, j] = new Stone(i, j);
                }
            }
        }
        
        /// <summary>
        /// Récupère l'instance de pierre aux coordonnées spécifiée
        /// </summary>
        /// <param name="x">Position ligne x dans le plateau</param>
        /// <param name="y">Position colonne y dans le plateau</param>
        /// <returns>Une instance de Stone</returns>
        /// <exception cref="ArgumentOutOfRangeException">Coordonnées hors plateau</exception>
        public Stone GetStone(int x, int y)
        {
            if (IsValidCoordinate(x, y))
            {
                return board[x, y];
            }
            throw new ArgumentOutOfRangeException($"Coordinates ({x}, {y}) are out of bounds for the goban size.");
        }

        /// <summary>
        /// Vérifie si une coordonnée (x, y) est dans les limites du tableau
        /// </summary>
        /// <param name="x">Position ligne x dans le plateau</param>
        /// <param name="y">Position colonne y dans le plateau</param>
        /// <returns>True si les coordonnées sont bonnes, False sinon</returns>
        public bool IsValidCoordinate(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Size && y < Size;
        }

        /// <summary>
        /// Vérifie si une pierre est dans les limites du tableau
        /// </summary>
        /// <param name="stone">La pierre à vérifié</param>
        /// <returns>True si les coordonnées sont bonnes, False sinon</returns>
        public bool IsValidCoordinate(Stone stone)
        {
            return IsValidCoordinate(stone.X, stone.Y);
        }
        
        /// <summary>
        /// Renvoie une copie de l'état de Goban
        /// </summary>
        public Stone[,] CopyThisBoard()
        {
            Stone[,] boardCopy = new Stone[this.size, this.size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    StoneColor color = this.board[i, j].Color;
                    boardCopy[i, j] = new Stone(i ,j, color);
                }
            }

            return boardCopy;
        }

        /// <summary>
        /// Copie le plateau en paramétre dans board du GameBoard
        /// </summary>
        /// <param name="boardToCopy">le plateau à copier</param>
        public void CopyToBoard(Stone[,] boardToCopy)
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    this.board[i, j].CopyStoneColor(boardToCopy[i, j]);
                }
            }
        }

        /// <summary>
        /// Copie le plateau en paramétre dans previousBoard du GameBoard
        /// </summary>
        /// <param name="boardToCopy"></param>
        public void CopyToPreviousBoard(Stone[,] boardToCopy)
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    this.previousBoard[i, j].CopyStoneColor(boardToCopy[i, j]);
                }
            }
        }
    }
}

