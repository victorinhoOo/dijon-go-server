namespace GoLogic.Goban
{
    /// <summary>
    /// Classe représentation du Goban
    /// </summary>
    public class GameBoard : IBoard
    {
        #region Attributs
        private StoneColor currentTurn;
        private Stone[,] board;
        private Stone[,] previousBoard; // Pour la règle de ko
        private int size;
        private int capturedBlackStones = 0;
        private int capturedWhiteStones = 0;

        /// <summary>
        /// Tour actuel, Noir ou Blanc
        /// </summary>
        public StoneColor CurrentTurn { get => currentTurn; set => this.currentTurn = value; }

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
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size), "Board size must be positive");

            this.size = size;
            this.board = new Stone[size, size];
            this.previousBoard = new Stone[size, size];
            this.capturedBlackStones = 0;
            this.capturedWhiteStones = 0;
            InitializeBoard();
        }

        /// <summary>
        /// Constructor privé pour créer une nouvelle instance avec un état spécifique
        /// </summary>
        private GameBoard(int size, Stone[,] board, Stone[,] previousBoard, int capturedBlack, int capturedWhite, StoneColor currentTurn)
        {
            this.size = size;
            this.board = board;
            this.previousBoard = previousBoard;
            this.capturedBlackStones = capturedBlack;
            this.capturedWhiteStones = capturedWhite;
            this.currentTurn = currentTurn;
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
        /// Change la couleur de la pierre aux coordonnés spécifié
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="color">Couleur de la pierre</param>
        public void ChangeStoneColor(int x, int y, StoneColor color)
        {
            if (!IsValidCoordinate(x, y))
                throw new ArgumentOutOfRangeException($"Coordinates ({x}, {y}) are out of bounds for the goban size.");

            CopyToPreviousBoard(this.board);
            this.board[x, y] = new Stone(x, y, color);
        }

        /// <summary>
        /// Clone le Goban
        /// </summary>
        /// <returns>Renvoie une nouvelle instance de GameBoard</returns>
        public IBoard Clone()
        {
            return new GameBoard(
                this.size,
                CloneBoardArray(this.board),
                CloneBoardArray(this.previousBoard),
                this.capturedBlackStones,
                this.capturedWhiteStones,
                this.currentTurn
            );
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
        /// Crée une copie compléte du board liste de pierre
        /// </summary>
        private Stone[,] CloneBoardArray(Stone[,] source)
        {
            Stone[,] clone = new Stone[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    clone[i, j] = new Stone(i, j, source[i, j].Color);
                }
            }
            return clone;
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

        /// <summary>
        /// Récupères les pierres voisines de celle spécifiée
        /// </summary>
        /// <param name="stone">La pierre dont on cherche les voisines</param>
        /// <returns>Liste des pierres voisines</returns>
        public List<Stone> GetNeighbors(Stone stone)
        {
            List<Stone> neighbors = [];

            // Récupère les coordonnées des Pierres voisines
            foreach (var (x, y) in stone.GetNeighborsCoordinate())
            {
                // Si les coordonnées sont correctes, on ajoute la pierre correspondante
                if (IsValidCoordinate(y, x))
                {
                    neighbors.Add(GetStone(x, y));
                }
            }

            return neighbors;
        }

        /// <summary>
        /// Vérifie si l'état actuel du plateau correspond à l'état précédent (règle de Ko).
        /// Pour éviter que le jeu tourne en boucle
        /// </summary>
        /// <returns>Vraie si le coup ne respecte pas la règle, faux sinon</returns>
        public bool IsKoViolation()
        {
            bool res = true; // Violation de Ko : le plateau correspond à l'état précédent

            // Compare l'état du plateau actuel au précédent
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    if (!this.board[i, j].Equals(this.previousBoard[i, j]))
                    {
                        res = false; // Les plateaux ne sont pas identiques pas de violation de Ko
                    }
                }
            }

            return res;
        }
    }
}

