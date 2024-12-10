using GoLogic.Goban;

namespace GoLogic.Score
{
    /// <summary>
    /// Gére l'analyse des pierres mortes d'un goban
    /// </summary>
    public class DeadStoneAnalyzer
    {
        private readonly IBoard gameBoard;

        /// <summary>
        /// Gére l'analyse des pierres mortes d'un goban
        /// </summary>
        /// <param name="gameBoard">IBoard à analyser</param>
        public DeadStoneAnalyzer(IBoard gameBoard)
        {
            this.gameBoard = gameBoard;
        }

        /// <summary>
        /// Récupère récursivement les pierres voisines d'une pierre donnée et leurs libertées
        /// </summary>
        /// <param name="stone">La pierre dont on veut récupérer le groupe</param>
        /// <param name="visited">Pierre visité lors de la recherche</param>
        /// <param name="group">Le groupe de pierre</param>
        /// <param name="liberties">les libertées du groupe</param>
        private void CollectGroupAndLiberties(Stone stone, ref HashSet<Stone> visited, ref List<Stone> group, ref HashSet<Stone> liberties)
        {
            // si la pierre n'a pas déjà été visité on continue
            if (!visited.Contains(stone))
            {
                visited.Add(stone);

                if (stone.Color == StoneColor.Empty) // si la pierre est Empty c'est une liberté
                {
                    liberties.Add(stone);
                }
                else
                {
                    StoneColor initialColor = group.FirstOrDefault()?.Color ?? stone.Color;
                    if (stone.Color == initialColor)
                    {
                        group.Add(stone); // ajoute la pierre au groupe
                        foreach (Stone neighbor in this.gameBoard.GetNeighbors(stone))
                        {
                            CollectGroupAndLiberties(neighbor, ref visited, ref group, ref liberties);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Vérifie si une liberté constitue l'oeil d'un groupe
        /// </summary>
        /// <param name="liberty">La liberté à tester</param>
        /// <param name="color">La couleur des pierres du groupe</param>
        /// <returns>True si la liberté est un oeil, False sinon</returns>
        private bool IsRealEye(Stone liberty, StoneColor color)
        {
            bool res = true;

            // Tous les voisins directs doivent être de la même couleur
            List<Stone> neighbors = this.gameBoard.GetNeighbors(liberty);
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

                foreach (int[] offset in diagonalOffsets)
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
        /// <returns>True si le groupe est seki, False sinon</returns>
        private bool IsSeki(List<Stone> group, HashSet<Stone> liberties)
        {
            bool res = false;

            StoneColor color = group[0].Color;
            StoneColor oppositeColor = color == StoneColor.Black ? StoneColor.White : StoneColor.Black;

            // Pour chaque liberté, vérifiez si elle est partagée avec un groupe ennemi
            foreach (Stone liberty in liberties)
            {
                List<Stone> neighbors = this.gameBoard.GetNeighbors(liberty);
                List<Stone> enemyNeighbors = neighbors.Where(n => n.Color == oppositeColor).ToList();

                foreach (Stone enemyStone in enemyNeighbors)
                {
                    List<Stone> enemyGroup = new List<Stone>();
                    HashSet<Stone> enemyLiberties = new HashSet<Stone>();
                    HashSet<Stone> enemyVisited = new HashSet<Stone>();

                    CollectGroupAndLiberties(enemyStone, ref enemyVisited, ref enemyGroup, ref enemyLiberties);

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
        /// <param name="stone">La pierre initiale du groupe à analyser</param>
        /// <returns>Renvoie True si les pierres sont mortes, False sinon</returns>
        public bool IsGroupDead(Stone stone)
        {
            bool res = true;

            if (stone.Color == StoneColor.Empty)
            {
                res = false;
            }
            else
            {
                (List<Stone> group, HashSet<Stone> liberties) = GetGroupAndLiberties(stone);
                List<Stone> eyes = GetRealEyes(liberties, stone.Color);

                // vérifie différente condition de vie
                if (HasTwoEyes(eyes) || 
                    IsSeki(group, liberties) ||
                    HasPotentialLife(liberties, group, stone.Color) ||
                    HasPotentialSecondEye(eyes, liberties, stone.Color))
                {
                    res = false;
                }
            }

            return res;
        }

        /// <summary>
        /// Récupère un groupe de pierres et ses libertés à partir d'une pierre donnée
        /// </summary>
        /// <param name="stone">La pierre initiale</param>
        /// <returns>Un tuple contenant le groupe de pierres et ses libertés</returns>
        private (List<Stone> group, HashSet<Stone> liberties) GetGroupAndLiberties(Stone stone)
        {
            HashSet<Stone> visited = new HashSet<Stone>();
            List<Stone> group = new List<Stone>();
            HashSet<Stone> liberties = new HashSet<Stone>();

            CollectGroupAndLiberties(stone, ref visited, ref group, ref liberties);
            return (group, liberties);
        }

        /// <summary>
        /// Identifie les vrais yeux parmi les libertés d'un groupe
        /// </summary>
        /// <param name="liberties">Les libertés à analyser</param>
        /// <param name="color">La couleur du groupe</param>
        /// <returns>Liste des libertés qui sont de vrais yeux</returns>
        private List<Stone> GetRealEyes(HashSet<Stone> liberties, StoneColor color)
        {
            return liberties.Where(l => IsRealEye(l, color)).ToList();
        }

        /// <summary>
        /// Vérifie si un groupe possède deux yeux ou plus
        /// </summary>
        /// <param name="eyes">Liste des yeux du groupe</param>
        /// <returns>Vrai si le groupe a au moins deux yeux</returns>
        private bool HasTwoEyes(List<Stone> eyes)
        {
            return eyes.Count >= 2;
        }

        /// <summary>
        /// Évalue si un groupe a une possibilité de vie en analysant ses libertés
        /// </summary>
        /// <param name="liberties">Les libertés du groupe</param>
        /// <param name="group">Le groupe de pierres</param>
        /// <param name="color">La couleur du groupe</param>
        /// <returns>Vrai si le groupe a un potentiel de vie</returns>
        private bool HasPotentialLife(HashSet<Stone> liberties, List<Stone> group, StoneColor color)
        {
            bool res = false;
            // Vérifie la condition d'une seule liberté
            if (liberties.Count == 1)
            {
                Stone liberty = liberties.First();
                List<Stone> surroundingStones = this.gameBoard.GetNeighbors(liberty);
                StoneColor oppositeColor = color == StoneColor.Black ? StoneColor.White : StoneColor.Black;

                if (!surroundingStones.All(s => s.Color == oppositeColor || group.Contains(s)))
                {
                    res = true;
                }
            }
            else
            {
                // Vérifie la condition de territoire
                foreach (Stone liberty in liberties)
                {
                    List<Stone> libertyNeighbors = this.gameBoard.GetNeighbors(liberty);
                    if (libertyNeighbors.Any(n => n.Color == StoneColor.Empty && !liberties.Contains(n)))
                    {
                        res = true;
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Évalue si un groupe avec un œil peut potentiellement former un second œil
        /// </summary>
        /// <param name="eyes">Liste des yeux existants</param>
        /// <param name="liberties">Les libertés du groupe</param>
        /// <param name="color">La couleur du groupe</param>
        /// <returns>Vrai si un second œil est possible</returns>
        private bool HasPotentialSecondEye(List<Stone> eyes, HashSet<Stone> liberties, StoneColor color)
        {
            bool res = false;
            if (eyes.Count == 1)
            {
                foreach (Stone liberty in liberties.Where(l => !eyes.Contains(l)))
                {
                    List<Stone> potentialEyeNeighbors = this.gameBoard.GetNeighbors(liberty);
                    if (potentialEyeNeighbors.Count(n => n.Color == color || liberties.Contains(n)) >= 3)
                    {
                        res = true;
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Identifie et retire les pierres mortes du plateau
        /// </summary>
        public void RemoveDeadStones()
        {
            HashSet<Stone> processedStones = new HashSet<Stone>();

            // Parcours toutes les pierres du plateau
            for (int x = 0; x < this.gameBoard.Size; x++)
            {
                for (int y = 0; y < this.gameBoard.Size; y++)
                {
                    Stone stone = this.gameBoard.GetStone(x, y);

                    // Saute si l'intersection est vide ou déjà visité
                    if (stone.Color == StoneColor.Empty || processedStones.Contains(stone))
                        continue;

                    // Si le groupe est mort, on récupère toutes ses pierres et on les retire
                    if (IsGroupDead(stone))
                    {
                        (List<Stone> deadGroup, HashSet<Stone> _) = GetGroupAndLiberties(stone);
                        foreach (Stone deadStone in deadGroup)
                        {
                            deadStone.ChangeColor(StoneColor.Empty);
                            processedStones.Add(deadStone);
                        }
                    }
                    else
                    {
                        // Marque les pierres du groupe vivant comme traitées
                        (List<Stone> livingGroup, HashSet<Stone> _) = GetGroupAndLiberties(stone);
                        foreach (var livingStone in livingGroup)
                        {
                            processedStones.Add(livingStone);
                        }
                    }
                }
            }
        }
    }
}
