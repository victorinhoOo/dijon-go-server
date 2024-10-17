namespace GoLogic.Score
{
    /// <summary>
    /// Classe abstraite pour gérer les différents décompte de point
    /// </summary>
    public abstract class ScoreRule
    {
        private readonly GameBoard gameBoard;

        /// <summary>
        /// Le plateau du jeu et ses pions
        /// </summary>
        public GameBoard GameBoard { get => gameBoard; }

        /// <summary>
        /// Le calculateur de score selon les différentes règles
        /// </summary>
        /// <param name="gameBoard">Le plateau du jeu</param>
        public ScoreRule(GameBoard gameBoard)
        {
            this.gameBoard = gameBoard;
        }

        /// <summary>
        /// Règles de décompte des points pour chaque joueur
        /// </summary>
        /// <returns>Tuple d'entier correspondant aux scores noirs et blanc</returns>
        public abstract (int blackStones, int whiteStones) CalculateScore();

        /// <summary>
        /// Compte le nombres de pierres de chaque couleur sur le plateau
        /// </summary>
        /// <returns>Une tuple d'entier correspondants aux pierres noires et blanches</returns>
        public (int blackStones, int whiteStones) CountStones()
        {
            int blackStones = 0;
            int whiteStones = 0;

            for (int i = 0; i < GameBoard.Size; i++)
            {
                for (int j = 0; j < GameBoard.Size; j++)
                {
                    if (GameBoard.Board[i, j].Color == StoneColor.Black) blackStones++;
                    if (GameBoard.Board[i, j].Color == StoneColor.White) whiteStones++;
                }
            }

            return (blackStones, whiteStones);
        }

        /// <summary>
        /// Trouve le territoire des joueurs noirs et blancs
        /// Le territoire est le nombre d'espaces vides entièrement entourés par les pierres d'un joueur
        /// </summary>
        /// <returns>Tuple d'entier des territoires noir, et blanc</returns>
        public (int blackTerritory, int whiteTerritory) FindTerritory()
        {
            int blackTerritory = 0;
            int whiteTerritory = 0;
            HashSet<Stone> visited = new HashSet<Stone>();

            // Explorez le tableau pour trouver des espaces vides entourés de pierres noires ou blanches
            for (int x = 0; x < gameBoard.Size; x++) // Parcourt toutes les lignes du plateau
            {
                for (int y = 0; y < gameBoard.Size; y++) // Parcourt toutes les colonnes du plateau
                {
                    Stone stone = gameBoard.GetStone(x, y);

                    // Si la pierre est vide et n'a pas encore été visité
                    if (stone.Color == StoneColor.Empty && !visited.Contains(stone)) 
                    {
                        // Explore la zone vide à partir de cette pierre
                        var (owner, emptyArea) = ExploreTerritory(stone, visited);

                        if (owner == StoneColor.Black)
                        {
                            // Ajoute la taille de la zone vide au territoire noir
                            blackTerritory += emptyArea.Count;
                        }
                        else if (owner == StoneColor.White)
                        {
                            // Ajoute la taille de la zone vide au territoire blanc
                            whiteTerritory += emptyArea.Count;
                        }
                    }
                }
            }

            return (blackTerritory, whiteTerritory);
        }

        /// <summary>
        /// Explore récursivement une zone vide et détermine son propriétaire
        /// Le propriétaire est le joueur dont les pierres entourent complètement la zone vide.
        /// Si la zone vide touche le bord du plateau, elle est considérée comme non revendiquée.
        /// </summary>
        /// <param name="stone">La pierre à partir de laquelle explorer</param>
        /// <param name="visited">Emplacement visité par la recherche</param>
        /// <returns>Tuple de la couleur, et liste des pierres dans le territoire</returns>
        private (StoneColor owner, List<Stone> emptyArea) ExploreTerritory(Stone stone, HashSet<Stone> visited)
        {
            List<Stone> emptyArea = new List<Stone>();
            HashSet<StoneColor> borderingColors = new HashSet<StoneColor>();
            Queue<Stone> queue = new Queue<Stone>();

            queue.Enqueue(stone);
            visited.Add(stone);

            // Explore récursivement pour trouver une zone vide
            while (queue.Count > 0)
            {
                Stone currentStone = queue.Dequeue(); // Retire la première pierre de la file pour l'examiner
                emptyArea.Add(currentStone); // Ajoute cette pierre à la zone vide en cours d'exploration

                foreach (Stone neighbor in GetNeighbors(currentStone))
                {
                    // Si cette pierre n'a pas encore été visitée
                    if (!visited.Contains(neighbor)) 
                    {
                        if (neighbor.Color == StoneColor.Empty) // Si la pierre voisine est vide, c'est un espace contigu
                        {
                            queue.Enqueue(neighbor); // Ajoute cette pierre à la file pour continuer l'exploration
                            visited.Add(neighbor);   // Marque la pierre comme visitée pour éviter de la réexaminer
                        }
                        else if (neighbor.Color != StoneColor.Empty)
                        {
                            // Ajoute sa couleur à l'ensemble des couleurs bordantes (pour déterminer le propriétaire)
                            borderingColors.Add(neighbor.Color);
                        }
                    }
                }
            }

            // Pas de propriétaire si les couleurs sont mixtes ou la zone touche le bord
            StoneColor resColor = StoneColor.Empty;
            List<Stone> resArea = emptyArea;

            // Si la zone vide touche le bord du plateau, c'est un territoire neutre
            if (stone.X == 0 || stone.X == gameBoard.Size - 1 || stone.Y == 0 || stone.Y == gameBoard.Size - 1)
            {
                resColor = StoneColor.Empty; 
                resArea = emptyArea;
            }

            // Détermine le propriétaire du territoire : s'il n'y a que des pierres noires ou blanches à la frontière
            else if (borderingColors.Count == 1)
            {
                resColor = borderingColors.Contains(StoneColor.Black) ? StoneColor.Black : StoneColor.White;
                resArea = emptyArea;
            }

            return (resColor, resArea); 
        }

        /// <summary>
        /// Obtient les pierres voisines d'une pierre donnée
        /// </summary>
        /// <param name="stone">La pierre dont on cherche les voisins</param>
        /// <returns>Liste des pierres voisines</returns>
        private List<Stone> GetNeighbors(Stone stone)
        {
            List<Stone> neighbors = new List<Stone>();

            // Récupère les coordonnées des Pierres voisines à partir des coordonnées de la pierre actuelle
            foreach (var (nx, ny) in stone.GetNeighborsCoordinate())
            {
                // Si les coordonnées sont correctes, ajoutez la pierre correspondante
                if (gameBoard.IsValidCoordinate(nx, ny))
                {
                    neighbors.Add(gameBoard.GetStone(nx, ny));
                }
            }

            return neighbors;
        }
    }
}
