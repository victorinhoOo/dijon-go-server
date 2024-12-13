using GoLogic.Goban;

namespace GoLogic
{
    /// <summary>
    /// Gére la capture des pierres du plateau
    /// </summary>
    public class CaptureManager
    {
        private IBoard goban;

        public CaptureManager(IBoard board) 
        {
            this.goban = board;
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
            foreach (Stone neighbor in this.goban.GetNeighbors(placedStone))
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
        public bool HasLiberties(Stone stone)
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
        public bool CheckLiberties(Stone stone, HashSet<Stone> visited, StoneColor initialStoneColor)
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
                if (stone.Color == initialStoneColor)
                {
                    // On continu la récursion sur tous les voisins
                    foreach (Stone neighbor in this.goban.GetNeighbors(stone))
                    {
                        // Si un voisin renvoie True on s'arrête (libertée)
                        if (CheckLiberties(neighbor, visited, stone.Color))
                            res = true;
                    }
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
            StoneColor opponentColor = stone.Color == StoneColor.Black ? StoneColor.White : StoneColor.Black;
            bool captured = false;

            // Pour chacun des voisin (pierre adjacente) on vérifie s'ils ont des libertées
            foreach (Stone neighbor in this.goban.GetNeighbors(stone))
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
            HashSet<Stone> visited = new HashSet<Stone>(); // collection sans doublon 
            List<Stone> group = new List<Stone>();

            // Récupére le groupe de la pierre passé en paramétre
            group = FindGroup(stone, visited, group, stone.Color);

            foreach (Stone stoneInGroup in group)
            {
                stoneInGroup.ChangeColor(StoneColor.Empty); // Retire les pierres capturées (couleur Empty)
            }

            if (this.goban.CurrentTurn == StoneColor.Black) goban.AddCapturedStone(StoneColor.White, group.Count);
            
            else goban.AddCapturedStone(StoneColor.Black, group.Count);
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
            if (!visited.Contains(stone) && this.goban.IsValidCoordinate(stone.X, stone.Y) && stone.Color == initialStoneColor)
            {
                visited.Add(stone);
                group.Add(stone);
                foreach (Stone neighbor in this.goban.GetNeighbors(stone))
                {
                    FindGroup(neighbor, visited, group, initialStoneColor);
                }
            }
            return group;
        }
    }
}
