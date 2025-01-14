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
        public StoneColor CurrentTurn { get => currentTurn; }

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
        }

        /// <summary>
        /// Pierre blanche qui ont été capturée
        /// </summary>
        public int CapturedWhiteStones
        {
            get => this.capturedWhiteStones;
        }
        #endregion Attributs
        
        /// <summary>
        /// Le plateau du jeu et ses pierres
        /// </summary>
        /// <param name="size">La taille du plateau size x size</param>
        /// <param name="handicapColor">Couleur des pierres de handicap</param>
        /// <param name="handicapNbr">Nombre de pierres de handicap</param>
        /// <exception cref="ArgumentOutOfRangeException">Lancer si taille du plateau est négative</exception>
        public GameBoard(int size, string handicapColor = "white", int handicapNbr = 0)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size), "Board size must be positive");

            this.size = size;
            this.board = new Stone[size, size];
            this.previousBoard = new Stone[size, size];
            this.capturedBlackStones = 0;
            this.capturedWhiteStones = 0;
            InitializeBoard();

            StoneColor color = handicapColor == "white" ? StoneColor.White : StoneColor.Black;
            PlaceHandicapStones(handicapNbr, color);
        }

        /// <summary>
        /// Constructor privé pour créer une nouvelle instance avec un état spécifique
        /// </summary>
        /// <param name="size">taille du plateau</param>
        /// <param name="board">Tableau de pierres actuel</param>
        /// <param name="previousBoard">Tableau de pierres précdent</param>
        /// <param name="capturedBlack">Pierres noires capturées</param>
        /// <param name="capturedWhite">Pierres blanches capturées</param>
        /// <param name="currentTurn">Tour du joueur</param>
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
                    this.board[i, j] = new Stone(i, j); // Initialise les pierres à Empty 
                    this.previousBoard[i, j] = new Stone(i, j);
                }
            }
        }

        /// <summary>
        /// Clone le GameBoard
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
        /// Place les pierres de handicap sur le plateau
        /// </summary>
        /// <param name="handicapCount">Nombre de pierre de handicap</param>
        /// <param name="handicapColor">Couleur du joueur avec l'aide</param>
        private void PlaceHandicapStones(int handicapCount, StoneColor handicapColor)
        {
            List<(int row, int col)> positions = new List<(int row, int col)>();

            if (Size == 9)
            {
                (int, int)[] hoshiPoints = new[] {
                (2, 6),   // haut droit
                (6, 2),   // bas gauche
                (6, 6),   // bas droit
                (2, 2)    // haut gauche
                };

                positions.AddRange(handicapCount == 1
                    ? new[] { hoshiPoints[0] }  // seulement haut droit
                    : hoshiPoints.Take(handicapCount)); // sequence normal de 2-4
            }
            else // 13x13 et 19x19
            {
                int near = 3;
                int far = Size - 4;
                int middle = Size / 2;

                // sequence de position standart excepter (middle, middle)
                List<(int, int)> hoshiPoints = new() {
                (near, far),      // haut droit (premier point)
                (far, near),      // bas gauche
                (far, far),       // bas droit
                (near, near),     // haut gauche
                (middle, middle), // centre
                (middle, near),   // bord gauche
                (middle, far),    // bord droit
                (near, middle),   // bord supérieur
                (far, middle),    // bord inférieur    
                };

                // Ajout des pierres de 2 à 4 en fonction
                positions.AddRange(handicapCount == 1
                    ? new[] { hoshiPoints[0] }  // seulement haut droit
                    : hoshiPoints.Take(handicapCount)); // sequence normal 2-4

                if (handicapCount > 4)
                {
                    // Ajout pour 5, 7, 9
                    if (handicapCount % 2 == 1)
                    {
                        positions.AddRange(hoshiPoints.Take(handicapCount));
                    }
                    else // Ajout pour handicap de 6 et 8
                    {
                        if (handicapCount == 6)
                        {
                            hoshiPoints.RemoveAt(4);
                            hoshiPoints.RemoveRange(6,2);
                            positions = hoshiPoints;
                        }
                        else
                        {
                            hoshiPoints.RemoveAt(4);
                            positions = hoshiPoints;
                        }
                    }
                }
            }

            // Place les pierres de handicap
            foreach ((int row, int col) in positions)
            {
                board[row, col].ChangeColor(handicapColor);
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
            if (!IsValidCoordinate(x, y))
            {
                throw new ArgumentOutOfRangeException($"Coordinates ({x}, {y}) are out of bounds for the goban size.");
            }
            return board[x, y];
        }

        /// <summary>
        /// Change la couleur de la pierre aux coordonnés spécifié
        /// </summary>
        /// <param name="stone">Pierre à poser</param>
        /// <param name="color">Couleur de la pierre</param>
        /// <exception cref="ArgumentOutOfRangeException">Lancer si les coordonnés sont hors limites</exception>
        public void PlaceStone(Stone stone, StoneColor color)
        {
            if (!IsValidCoordinate(stone.X, stone.Y))
                throw new ArgumentOutOfRangeException($"Coordinates ({stone.X}, {stone.Y}) are out of bounds for the goban size.");

            CopyToPreviousBoard();
            stone.ChangeColor(color);
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
        /// <param name="source">Tableau de pierres à copier</param>
        /// <returns>Tableau de pierres identiques à l'original</returns>
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
        public void CopyToPreviousBoard()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    this.previousBoard[i, j].CopyStoneColor(this.board[i, j]);
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
            foreach ((int x, int y) in stone.GetNeighborsCoordinate())
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

        /// <inheritdoc/>
        public void AddCapturedStone(StoneColor color, int amount)
        {
            if (color == StoneColor.Black)
            {
                this.capturedBlackStones += amount;
            }
            else
            {
                this.capturedWhiteStones += amount;
            }
        }

        /// <inheritdoc/> 
        public void NextTurn()
        {
            this.currentTurn = this.currentTurn == StoneColor.Black ? StoneColor.White : StoneColor.Black;
        }

        /// <summary>
        /// Compte les nombres de pierres de chaque couleur sur le plateau
        /// </summary>
        /// <returns>Un tuple d'entier correspondant aux pierres noires et blanches</returns>
        public (int blackStones, int whiteStones) CountStones()
        {
            int blackStones = 0;
            int whiteStones = 0;

            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size; j++)
                {
                    if (this.GetStone(i, j).Color == StoneColor.Black) blackStones++;
                    if (this.GetStone(i, j).Color == StoneColor.White) whiteStones++;
                }
            }

            return (blackStones, whiteStones);
        }
    }
}

