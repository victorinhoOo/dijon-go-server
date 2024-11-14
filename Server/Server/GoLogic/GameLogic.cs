namespace GoLogic
{
    /// <summary>
    /// Gére les actions et règles de base du go
    /// </summary>
    public class GameLogic
    {
        #region attributs
        private GameBoard board;
        private List<Stone> moves = new List<Stone>();
        private StoneColor currentTurn;
        private bool isEndGame;
        private bool skippedTurn;
        private Stone previousStone;

        /// <summary>
        /// Le plateau de la partie
        /// </summary>
        public GameBoard Board { get => this.board; set => this.board = value; }
        
        /// <summary>
        /// pour le replay, moves = [(1, 1, Black), (2, 2, White), ...]
        /// </summary>
        public List<Stone> Moves { get => this.moves; set => this.moves = value;  }
        
        /// <summary>
        /// Tour actuel, Noir ou Blanc
        /// </summary>
        public StoneColor CurrentTurn { get => currentTurn; set => currentTurn = value; }

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
        public GameLogic(GameBoard board)
        {
            this.board = board;
            this.currentTurn = StoneColor.Black;
        }

        /// <summary>
        /// Passe le tour
        /// </summary>
        public void SkipTurn()
        {
            if (this.skippedTurn) this.isEndGame = true;
            currentTurn = currentTurn == StoneColor.Black ? StoneColor.White : StoneColor.Black;
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
            Stone stone = Board.Board[x, y]; // récupère la pierre aux coordonnées données

            if (!IsValidMove(stone))
            {
                throw new InvalidOperationException($"Move at ({x}, {y}) is not valid.");
            }
            else
            {
                stone.Color = CurrentTurn; // place la pierre en changeant sa couleur de Empty à CurrentTurn
                this.previousStone = stone;
                Moves.Add(new Stone(x, y, CurrentTurn)); // enregistre le coup
                CurrentTurn = CurrentTurn == StoneColor.Black ? StoneColor.White : StoneColor.Black; // tour passe au joueur suivant

                res = true;
            }
            

            return res;
        }

        /// <summary>
        /// Vérifie si le coup est valide selon les règles du GO
        /// </summary>
        /// <param name="stone">La pierre placée sur le plateau</param>
        /// <returns>True si le coup est valide, False sinon</returns>
        public bool IsValidMove(Stone stone)
        {
            GameBoard boardCopy = new GameBoard(board.Size);
            boardCopy.Board = board.CopyBoard();
            boardCopy.PreviousBoard = board.PreviousBoard;
            GameLogic logicCopy = new GameLogic(boardCopy);
            bool result = true;

            // Vérifie que le coup est dans les limites du plateau et que l'emplacement est vide
            if (stone.Color != StoneColor.Empty || !Board.IsValidCoordinate(stone.X, stone.Y))
                result = false;

            else
            {
                // Place la pierre temporairement pour vérifier les libertés et les captures
                stone.Color = CurrentTurn;

                // 1. Vérifie si la pierre a des libertés et capture des pierres adverses
                // Vérifie si le coup entraînerait un "suicide" (pas de libertés)
                // On l'autorise seulement s'il capture des pierres adverses
                if (!HasLiberties(stone) && !CheckCapture(stone))
                {
                    stone.Color = StoneColor.Empty; // Annule le coup
                    result = false; // Coup invalidé (car suicide)
                }
                // 2. Vérifie la règle de Ko (empêche de répéter l'état précédent du plateau)
                else
                {
                    CapturesOpponent(stone); // On execute la capture pour vérifier ko
                    if (IsKoViolation(Board))
                    {
                        result = false; // Coup invalide (ne respecte pas la régle de ko)
                        stone.Color = StoneColor.Empty; // Annule le coup
                        Board.Board = boardCopy.CopyBoard(); // Récupère l'état initial du Goban
                        if (this.CurrentTurn == StoneColor.White)
                        {
                            board.CapturedBlackStones -= 1;
                        }
                        else
                        {
                            board.CapturedWhiteStones -= 1;
                        }
                    }
                    else // Coup valide
                    {
                        Board.PreviousBoard = boardCopy.CopyBoard();
                    }
                    
                }

            }

            return result;
        }

        /// <summary>
        /// Vérifie si l'état actuel du plateau correspond à l'état précédent (règle de Ko).
        /// Pour éviter que le jeu tourne en boucle
        /// </summary>
        /// <returns>Vraie si le coup ne respecte pas la règle, faux sinon</returns>
        public bool IsKoViolation(GameBoard board)
        {
            bool res = true; // Violation de Ko : le plateau correspond à l'état précédent
            
            // Compare l'état du plateau actuel au précédent
            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    if (!board.Board[i, j].Equals(Board.PreviousBoard[i, j]))
                    {
                        res = false; // Les plateaux ne sont pas identiques pas de violation de Ko
                    }
                }
            }

            return res; 
        }

        /// <summary>
        /// Après avoir placé une pierre à (x, y), vérifie si des pierres adverses sont capturées.
        /// Les pierres capturées sont retirées du plateau (couleur Empty)
        /// </summary>
        /// <param name="placedStone">La pierre placée sur le plateau</param>
        public void CapturesOpponent(Stone placedStone)
        {
            // Récupère la couleur opposée au joueur courant
            StoneColor opponentColor = placedStone.Color == StoneColor.Black ? StoneColor.White : StoneColor.Black;

            // Vérifie les pierres voisines pour potentielle capture
            foreach (Stone neighbor in GetNeighbors(placedStone))
            {
                if (neighbor.Color == opponentColor && !HasLiberties(neighbor))
                {
                    CaptureGroup(neighbor); // Capture le groupe si pas de libertés
                }
            }
        }
       
        /// <summary>
        /// Vérifie si une pierre à des libertés
        /// </summary>
        /// <param name="stone">La pierre dont l'on veut connaitre les libertés</param>
        /// <returns>Vrai s'il y a des libertés, Faux sinon</returns>
        private bool HasLiberties(Stone stone)
        {
            HashSet<Stone> visited = new HashSet<Stone>();
            return CheckLiberties(stone, visited, stone.Color);
        }
        
        /// <summary>
        /// Vérifie les libertés du groupe connecté à la pierre spécifié récursivement
        /// </summary>
        /// <param name="stone">La pierre dont l'on cherche les libertés</param>
        /// <param name="visited">Groupe de pierres déjà analysé</param>
        /// <param name="initialStoneColor">Couleur de la pierre initiale</param>
        /// <returns>True si le groupe a des libertés, False sinon (capturé)</returns>
        private bool CheckLiberties(Stone stone, HashSet<Stone> visited, StoneColor initialStoneColor)
        {
            bool res = false;

            // Si la pierre a déjà été visité, on s'arrête
            if (visited.Contains(stone))
                res = false;

            else
            {
                visited.Add(stone);

                // Si la pierre est vide, on s'arrête (car espace vide = libertée)
                if (stone.Color == StoneColor.Empty)
                    res = true;
            
                // Si la pierre qu'on visite est de même couleur que celle initiale, on continu
                if(stone.Color == initialStoneColor)
                {
                    bool result = false;

                    // On continu la récursion sur tous les voisins
                    foreach(Stone neighbor in GetNeighbors(stone))
                    {
                        // Si un voisin renvoie True on s'arrête (libertée)
                        if (CheckLiberties(neighbor, visited, stone.Color))
                            result = true;
                    }
                    return result;
                }
            }
            
            return res;
        }
        
        /// <summary>
        /// Vérifie si placé une pierre capture des pierres adverses
        /// </summary>
        /// <param name="stone">La pierre placer</param>
        /// <returns>True si capture, False sinon</returns>
        public bool CheckCapture(Stone stone)
        {
            // Récupère la couleur opposée au joueur courant
            StoneColor opponentColor = currentTurn == StoneColor.Black ? StoneColor.White : StoneColor.Black;
            bool captured = false;
            
            // Pour chacun des voisin (pierre adjacente) on vérifie s'ils ont des libertées
            foreach (Stone neighbor in GetNeighbors(stone))
            {
                // Si la pierre n'a pas de liberté alors il y a capture
                if (neighbor.Color == opponentColor && !HasLiberties(neighbor))
                {
                    captured = true;
                }
            }

            return captured;
        }
        
        /// <summary>
        /// Capture un groupe de pierres (en passant leur couleur à Empty)
        /// </summary>
        /// <param name="stone">La pierre initiale dont l'on veut capturer le groupe</param>
        private void CaptureGroup(Stone stone)
        {
            HashSet<Stone> visited = []; // collection sans doublon 
            List<Stone> group = [];

            // Récupére le groupe de la pierre passé en paramétre
            group = FindGroup(stone, visited, group, stone.Color);

            foreach (Stone stoneInGroup in group)
            {
                stoneInGroup.Color = StoneColor.Empty; // Retire les pierres capturées (couleur Empty)
            }
            if (currentTurn == StoneColor.Black)
            {
                Board.CapturedWhiteStones += group.Count;
            }
            else
            {
                Board.CapturedBlackStones += group.Count;
            }
        }

        /// <summary>
        /// Récupére récursivement un groupe de pierre de même couleur adjacentes entre elles
        /// </summary>
        /// <param name="stone">La pierre initiale dont l'on cherche le groupe</param>
        /// <param name="visited">Tableau de Pierre visité par la recherche</param>
        /// <param name="group">Tableau de Pierre rechercher</param>
        /// <param name="initialStoneColor">Couleur de la pierre initiale</param>
        /// <returns>Liste de Pierre de même couleur toutes adjacentes</returns>
        private List<Stone> FindGroup(Stone stone, HashSet<Stone> visited, List<Stone> group, StoneColor initialStoneColor)
        {
            if (visited.Contains(stone) || !Board.IsValidCoordinate(stone.X, stone.Y) || stone.Color != initialStoneColor)
            {
                return group;
            }
            visited.Add(stone);
            group.Add(stone);
            foreach (Stone neighbor in GetNeighbors(stone))
            {
                group = FindGroup(neighbor, visited, group, initialStoneColor);
            }
            return group;
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
                if (Board.IsValidCoordinate(y, x))
                {
                    neighbors.Add(Board.GetStone(x, y));
                }
            }

            return neighbors;
        }
        
    }
}

