namespace Go_logic
{
    public class ScoreCalculator
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
        public ScoreCalculator(GameBoard gameBoard)
        {
            this.gameBoard = gameBoard;
        }
        
        /// <summary>
        /// Trouve le territoire des joueurs noirs et blancs
        /// Le territoire est le nombre d'espaces vides entièrement entourés par les pierres d'un joueur
        /// </summary>
        /// <returns>Tuple d'entier des territoires noir, et blanc</returns>
        private (int blackTerritory, int whiteTerritory) FindTerritory()
        {
            int blackTerritory = 0;
            int whiteTerritory = 0;
            HashSet<(int, int)> visited = new HashSet<(int, int)>();

            // Explorez le tableau pour trouver des espaces vides entourés de pierres noires ou blanches
            for (int x = 0; x < gameBoard.Size; x++)
            {
                for (int y = 0; y < gameBoard.Size; y++)
                {
                    Stone stone = gameBoard.GetStone(x, y);

                    if (stone.Color == StoneColor.Empty && !visited.Contains((x, y)))
                    {
                        var (owner, emptyArea) = ExploreTerritory(x, y, visited);

                        if (owner == StoneColor.Black)
                        {
                            blackTerritory += emptyArea.Count;
                        }
                        else if (owner == StoneColor.White)
                        {
                            whiteTerritory += emptyArea.Count;
                        }
                    }
                }
            }

            return (blackTerritory, whiteTerritory);
        }
        
        /// <summary>
        /// Explore récursivement une zone vide et déterminer son propriétaire
        /// Le propriétaire est le joueur dont les pierres entourent complètement la zone vide
        /// </summary>
        /// <param name="x">Coordonnées sur le plateau</param>
        /// <param name="y">Coordonnées sur le plateau</param>
        /// <param name="visited">Emplacement visité par la recherche</param>
        /// <returns>Tuple de la couleur, et liste des coordonnées du territoire</returns>
        private (StoneColor owner, List<(int, int)> emptyArea) ExploreTerritory(int x, int y, HashSet<(int, int)> visited)
        {
            List<(int, int)> emptyArea = new List<(int, int)>();
            HashSet<StoneColor> borderingColors = new HashSet<StoneColor>();

            Queue<(int, int)> queue = new Queue<(int, int)>();
            queue.Enqueue((x, y));
            visited.Add((x, y));

            // Explore récursivement pour trouver une zone vide
            while (queue.Count > 0)
            {
                var (cx, cy) = queue.Dequeue();
                emptyArea.Add((cx, cy));

                foreach (var (nx, ny) in GetNeighbors(cx, cy))
                {
                    if (!visited.Contains((nx, ny)))
                    {
                        Stone neighborStone = gameBoard.GetStone(nx, ny);

                        if (neighborStone.Color == StoneColor.Empty)
                        {
                            queue.Enqueue((nx, ny));
                            visited.Add((nx, ny));
                        }
                        else if (neighborStone.Color == StoneColor.Black || neighborStone.Color == StoneColor.White)
                        {
                            borderingColors.Add(neighborStone.Color);
                        }
                    }
                }
            }

            // Détermine le propriétaire du territoire : s'il n'y a que des pierres noires ou blanches à la frontière
            if (borderingColors.Count == 1)
            {
                return (borderingColors.Contains(StoneColor.Black) ? StoneColor.Black : StoneColor.White, emptyArea);
            }

            return (StoneColor.Empty, emptyArea);  // Pas de propriétaire si les couleurs sont mixtes
        }

        // Fonction d'aide pour obtenir les voisins d'une position (x, y)
        private List<(int, int)> GetNeighbors(int x, int y)
        {
            List<(int, int)> neighbors = new List<(int, int)>();

            if (x > 0) neighbors.Add((x - 1, y));     // Up
            if (x < gameBoard.Size - 1) neighbors.Add((x + 1, y)); // Down
            if (y > 0) neighbors.Add((x, y - 1));     // Left
            if (y < gameBoard.Size - 1) neighbors.Add((x, y + 1)); // Right

            return neighbors;
        }

        /// <summary>
        /// Règles de décompte chinoises : compte à la fois les pierres et le territoire entouré
        /// </summary>
        /// <returns>Tuple d'entier</returns>
        public (int blackScore, int whiteScore) CalculateScoreChinese()
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

            var (territoryBlack, territoryWhite) = FindTerritory();
            return (blackStones + territoryBlack, whiteStones + territoryWhite);
        }

        /// <summary>
        /// Règles de décompte japonaises : compte le territoire encerclé et les pierres capturées
        /// </summary>
        /// <returns>Tuple d'entier</returns>
        public (int blackScore, int whiteScore) CalculateScoreJapanese()
        {
            var (territoryBlack, territoryWhite) = FindTerritory();
            return (territoryBlack + GameBoard.CapturedWhiteStones, territoryWhite + GameBoard.CapturedBlackStones);
        }
    }
}

