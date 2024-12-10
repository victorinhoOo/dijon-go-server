using GoLogic.Goban;
using System.Collections.Generic;

namespace GoLogic.Score
{
    /// <summary>
    /// Classe abstraite pour gérer les différents décomptes de point
    /// </summary>
    public abstract class ScoreRule
    {
        protected readonly IBoard gameBoard;
        protected readonly float komi = 6.5f;
        protected readonly DeadStoneAnalyzer deadStoneAnalyzer;

        /// <summary>
        /// Gère les pierres mortes
        /// </summary>
        public DeadStoneAnalyzer DeadStoneAnalyzer { get => deadStoneAnalyzer; }

        /// <summary>
        /// Le calculateur de score selon les différentes règles
        /// </summary>
        /// <param name="gameBoard">Le plateau du jeu</param>
        /// <param name="komi"></param>
        public ScoreRule(IBoard gameBoard, float komi = 6.5f)
        {
            this.gameBoard = gameBoard;
            this.komi = komi;
            this.deadStoneAnalyzer = new DeadStoneAnalyzer(gameBoard);
        }

        /// <summary>
        /// Règles de décompte des points pour chaque joueur
        /// </summary>
        /// <returns>Tuple d'entier correspondant aux scores noir et blanc</returns>
        public abstract (float blackStones, float whiteStones) CalculateScore();

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
        /// Retire les pierres mortes du plateau
        /// </summary>
        public void RemoveDeadStones()
        {
            this.DeadStoneAnalyzer.RemoveDeadStones();
        }
    }
}
