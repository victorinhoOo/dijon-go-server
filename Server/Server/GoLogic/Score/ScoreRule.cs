namespace GoLogic.Score
{
    /// <summary>
    /// Classe abstraite pour gérer les différents décomptes de point
    /// </summary>
    public abstract class ScoreRule
    {
        private readonly GameBoard gameBoard;

        /// <summary>
        /// Le plateau du jeu et ses pions
        /// </summary>
        public GameBoard GameBoard { get => this.gameBoard; }

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
        /// <returns>Tuple d'entier correspondant aux scores noir et blanc</returns>
        public abstract (int blackStones, int whiteStones) CalculateScore();

        /// <summary>
        /// Compte les nombres de pierres de chaque couleur sur le plateau
        /// </summary>
        /// <returns>Un tuple d'entier correspondant aux pierres noires et blanches</returns>
        public (int blackStones, int whiteStones) CountStones()
        {
            int blackStones = 0;
            int whiteStones = 0;

            for (int i = 0; i < this.gameBoard.Size; i++)
            {
                for (int j = 0; j < this.gameBoard.Size; j++)
                {
                    if (this.gameBoard.Board[i, j].Color == StoneColor.Black) blackStones++;
                    if (this.gameBoard.Board[i, j].Color == StoneColor.White) whiteStones++;
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
                        (StoneColor owner, List<Stone> emptyArea) = ExploreTerritory(stone, visited);

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

            // Explore pour trouver une zone vide
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
                        else
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

            if (borderingColors.Count == 1)
            {
                resColor = borderingColors.Contains(StoneColor.Black) ? StoneColor.Black : StoneColor.White;
                resArea = emptyArea;
            }

            return (resColor, resArea); 
        }

        /// <summary>
        /// Récupère récursivement les pierres voisines d'une pierre donnée et leurs libertées
        /// </summary>
        /// <param name="stone">La pierre dont on veut récupérer le groupe</param>
        /// <param name="visited">Pierre visité lors de la recherche</param>
        /// <param name="group">Le groupe de pierre</param>
        /// <param name="liberties">les libertées du groupe</param>
        private void CollectGroupAndLiberties(Stone stone, HashSet<Stone> visited, List<Stone> group, HashSet<Stone> liberties)
        {
            // si la pierre a déjà été visité on s'arrête
            if (visited.Contains(stone)) return;
            visited.Add(stone);

            // si la pierre est Empty c'est une liberté
            if (stone.Color == StoneColor.Empty)
            {
                liberties.Add(stone);
                return;
            }

            StoneColor initialColor = group.FirstOrDefault()?.Color ?? stone.Color;
            if (stone.Color == initialColor)
            {
                group.Add(stone);
                foreach (Stone neighbor in GetNeighbors(stone))
                {
                    CollectGroupAndLiberties(neighbor, visited, group, liberties);
                }
            }
        }

        /// <summary>
        /// Vérifie si une liberté constitue l'oeil d'un groupe
        /// </summary>
        /// <param name="liberty">la liberté à tester</param>
        /// <param name="color">La couleur des pierres du groupe</param>
        /// <returns>True si la liberté est un oeil, False sinon</returns>
        private bool IsRealEye(Stone liberty, StoneColor color)
        {
            bool res = true;

            // Tous les voisins directs doivent être de la même couleur
            List<Stone> neighbors = GetNeighbors(liberty);
            if (!neighbors.All(n => n.Color == color)) res = false;
            else
            {
                // Obtine les positions diagonales
                int x = liberty.X;
                int y = liberty.Y;
                List<Stone> diagonals = new List<Stone>();

                int[][] diagonalOffsets = new int[][]
                {
                new int[] {-1, -1}, new int[] {-1, 1},
                new int[] {1, -1}, new int[] {1, 1}
                };

                foreach (var offset in diagonalOffsets)
                {
                    int newX = x + offset[0];
                    int newY = y + offset[1];
                    
                    // si les coordonnes sont valides (dans le Goban) on récupère la pierre 
                    if (gameBoard.IsValidCoordinate(newX, newY))
                    {
                        diagonals.Add(gameBoard.GetStone(newX, newY));
                    }
                }

                // Les pierres aux bords et coins nécessitent moins de diagonales contrôlées
                bool isEdge = x == 0 || x == gameBoard.Size - 1 || y == 0 || y == gameBoard.Size - 1;
                bool isCorner = (x == 0 || x == gameBoard.Size - 1) && (y == 0 || y == gameBoard.Size - 1);

                int requiredDiagonals = isCorner ? 1 : (isEdge ? 2 : 3);
                int controlledDiagonals = diagonals.Count(d => d.Color == color || d.Color == StoneColor.Empty);

                res = controlledDiagonals >= requiredDiagonals;
            }

            return res;
        }

        /// <summary>
        /// Vérifie si un groupe est dans une situation de seki
        /// </summary>
        /// <param name="group">Le groupe à tester</param>
        /// <param name="liberties">les libertées de ce groupe</param>
        /// <returns></returns>
        private bool IsSeki(List<Stone> group, HashSet<Stone> liberties)
        {
            bool res = false;

            StoneColor color = group[0].Color;
            StoneColor oppositeColor = color == StoneColor.Black ? StoneColor.White : StoneColor.Black;

            // Pour chaque liberté, vérifiez si elle est partagée avec un groupe ennemi
            foreach (Stone liberty in liberties)
            {
                List<Stone> neighbors = GetNeighbors(liberty);
                List<Stone> enemyNeighbors = neighbors.Where(n => n.Color == oppositeColor).ToList();

                foreach (Stone enemyStone in enemyNeighbors)
                {
                    List<Stone> enemyGroup = new List<Stone>();
                    HashSet<Stone> enemyLiberties = new HashSet<Stone>();
                    HashSet<Stone> enemyVisited = new HashSet<Stone>();

                    CollectGroupAndLiberties(enemyStone, enemyVisited, enemyGroup, enemyLiberties);

                    // Si les deux groupes partagent les mêmes libertés vitales et ont peu de libertés
                    List<Stone> sharedLiberties = liberties.Intersect(enemyLiberties).ToList();
                    if (sharedLiberties.Count >= 1 && liberties.Count <= 2 && enemyLiberties.Count <= 2)
                    {
                        res = true;
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Détermine si un groupe de pierre est mort
        /// </summary>
        /// <param name="stone">La pierre initial du groupe à analyser</param>
        /// <returns>Renvoie True si les pierres sont mortes False sinon</returns>
        public bool IsGroupDead(Stone stone)
        {
            bool res = true;
            if (stone.Color == StoneColor.Empty) res = false;

            HashSet<Stone> visited = new HashSet<Stone>();
            List<Stone> group = new List<Stone>();
            HashSet<Stone> liberties = new HashSet<Stone>();

            // Obtenez le groupe complet et ses libertés
            CollectGroupAndLiberties(stone, visited, group, liberties);

            // Vérifie pour les vraies yeux
            List<Stone> eyes = liberties.Where(l => IsRealEye(l, stone.Color)).ToList();
            if (eyes.Count >= 2)
            {
                res = false; // Deux yeux = en vie
            }

            // vérifie si c'est une situation de seki
            if (IsSeki(group, liberties))
            {
                res = false;
            }

            // Si le groupe n'a qu'une seule liberté mais qu'il n'est pas complètement entouré de pierres ennemies
            if (liberties.Count == 1)
            {
                Stone liberty = liberties.First();
                List<Stone> surroundingStones = GetNeighbors(liberty);
                StoneColor oppositeColor = stone.Color == StoneColor.Black ? StoneColor.White : StoneColor.Black;

                // Si toutes les pierres environnantes ne sont pas de la couleur opposée, le groupe n'est peut-être pas mort
                if (!surroundingStones.All(s => s.Color == oppositeColor || group.Contains(s)))
                {
                    res = false;
                }
            }

            // Vérifiez si le groupe est en territoire ennemi
            (int blackTerritory, int whiteTerritory) = FindTerritory();
            foreach (Stone liberty in liberties)
            {
                List<Stone> libertyNeighbors = GetNeighbors(liberty);
                StoneColor oppositeColor = stone.Color == StoneColor.Black ? StoneColor.White : StoneColor.Black;

                // Si la liberté a des voisins vides non contrôlés par l'adversaire
                if (libertyNeighbors.Any(n => n.Color == StoneColor.Empty &&
                    !liberties.Contains(n)))
                {
                    res = false;
                }
            }

            // Si nous avons un oeil et le potentiel pour un autre
            if (eyes.Count == 1)
            {
                foreach (Stone liberty in liberties.Where(l => !eyes.Contains(l)))
                {
                    List<Stone> potentialEyeNeighbors = GetNeighbors(liberty);
                    if (potentialEyeNeighbors.Count(n => n.Color == stone.Color || liberties.Contains(n)) >= 3)
                    {
                        res = false;
                    }
                }
            }

            return res;
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
            foreach ((int nx, int ny) in stone.GetNeighborsCoordinate())
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
