using GoLogic.Goban;
using System.Collections.Generic;

namespace GoLogic.Score
{
    /// <summary>
    /// Classe abstraite pour gérer les différents décomptes de point
    /// </summary>
    public abstract class ScoreRule
    {
        private readonly IBoard gameBoard;
        protected readonly float komi = 6.5f;

        /// <summary>
        /// Le plateau du jeu et ses pions
        /// </summary>
        public IBoard GameBoard { get => this.gameBoard; }

        /// <summary>
        /// Le calculateur de score selon les différentes règles
        /// </summary>
        /// <param name="gameBoard">Le plateau du jeu</param>
        public ScoreRule(IBoard gameBoard, float komi = 6.5f)
        {
            this.gameBoard = gameBoard;
            this.komi = komi;
        }

        /// <summary>
        /// Règles de décompte des points pour chaque joueur
        /// </summary>
        /// <returns>Tuple d'entier correspondant aux scores noir et blanc</returns>
        public abstract (float blackStones, float whiteStones) CalculateScore();

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
                    if (this.gameBoard.GetStone(i, j).Color == StoneColor.Black) blackStones++;
                    if (this.gameBoard.GetStone(i, j).Color == StoneColor.White) whiteStones++;
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

                foreach (Stone neighbor in this.gameBoard.GetNeighbors(currentStone))
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
                foreach (Stone neighbor in this.gameBoard.GetNeighbors(stone))
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
                List<Stone> neighbors = this.gameBoard.GetNeighbors(liberty);
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
            if (stone.Color == StoneColor.Empty) return false;

            var (group, liberties) = GetGroupAndLiberties(stone);
            List<Stone> eyes = GetRealEyes(liberties, stone.Color);

            // Check various life conditions
            if (HasTwoEyes(eyes) ||
                IsSeki(group, liberties) ||
                HasPotentialLife(liberties, group, stone.Color) ||
                HasPotentialSecondEye(eyes, liberties, stone.Color))
            {
                return false;
            }

            return true;
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

            CollectGroupAndLiberties(stone, visited, group, liberties);
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
            // Vérifie la condition d'une seule liberté
            if (liberties.Count == 1)
            {
                Stone liberty = liberties.First();
                List<Stone> surroundingStones = this.gameBoard.GetNeighbors(liberty);
                StoneColor oppositeColor = color == StoneColor.Black ? StoneColor.White : StoneColor.Black;

                if (!surroundingStones.All(s => s.Color == oppositeColor || group.Contains(s)))
                {
                    return true;
                }
            }

            // Vérifie la condition de territoire
            foreach (Stone liberty in liberties)
            {
                List<Stone> libertyNeighbors = this.gameBoard.GetNeighbors(liberty);
                if (libertyNeighbors.Any(n => n.Color == StoneColor.Empty && !liberties.Contains(n)))
                {
                    return true;
                }
            }

            return false;
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
            if (eyes.Count != 1) return false;

            foreach (Stone liberty in liberties.Where(l => !eyes.Contains(l)))
            {
                List<Stone> potentialEyeNeighbors = this.gameBoard.GetNeighbors(liberty);
                if (potentialEyeNeighbors.Count(n => n.Color == color || liberties.Contains(n)) >= 3)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Identifie et retire les pierres mortes du plateau
        /// </summary>
        public void RemoveDeadStones()
        {
            HashSet<Stone> processedStones = new HashSet<Stone>();

            // Parcours toutes les pierres du plateau
            for (int x = 0; x < GameBoard.Size; x++)
            {
                for (int y = 0; y < GameBoard.Size; y++)
                {
                    Stone stone = GameBoard.GetStone(x, y);

                    // Saute si l'intersection est vide ou déjà visité
                    if (stone.Color == StoneColor.Empty || processedStones.Contains(stone))
                        continue;

                    // Si le groupe est mort, on récupère toutes ses pierres et on les retire
                    if (IsGroupDead(stone))
                    {
                        (List <Stone> deadGroup, HashSet <Stone> _) = GetGroupAndLiberties(stone);
                        foreach (Stone deadStone in deadGroup)
                        {
                            deadStone.ChangeColor(StoneColor.Empty);
                            processedStones.Add(deadStone);
                        }
                    }
                    else
                    {
                        // Marque les pierres du groupe vivant comme traitées
                        var (livingGroup, _) = GetGroupAndLiberties(stone);
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
