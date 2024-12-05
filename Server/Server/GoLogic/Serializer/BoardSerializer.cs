using GoLogic.Goban;
using System.Text;

namespace GoLogic.Serializer
{
    /// <summary>
    /// Serialise en String le Goban
    /// </summary>
    public class BoardSerializer
    {
        private GameLogic logic;

        public BoardSerializer(GameLogic logic)
        {
            this.logic = logic;
        }

        /// <summary>
        /// Vérifie et retourne une liste des positions Ko sur le plateau
        /// Une case est Ko si le coup remet le plateau dans son état précédent
        /// </summary>
        /// <param name="logic">instance de Gamelogic</param>
        /// <param name="currentTurn">Tour du joueur actuel, noir ou blanc</param>
        /// <returns>List des pierres en situation de ko</returns>
        private List<Stone> ChecksGobanForKo(StoneColor currentTurn)
        {
            List<Stone> potentialKoPositions = new List<Stone>();

            // Only check Ko if there was a previous stone
            if (this.logic.PreviousStone == null) return potentialKoPositions;

            // Récupère tous les voisins vides de la pierre précédente
            foreach (Stone stone in logic.Goban.GetNeighbors(this.logic.PreviousStone))
            {
                if (stone.Color == StoneColor.Empty)
                {
                    // Pour chaque voisin vide, fait une nouvelle copie et teste
                    IBoard boardCopy = this.logic.Goban.Clone();
                    CaptureManager captureManagerCopy = new CaptureManager(boardCopy);
                    Stone stoneCopy = boardCopy.GetStone(stone.X, stone.Y);

                    // Essaie de placer une pierre de la couleur du joueur actuel
                    stoneCopy.ChangeColor(currentTurn);

                    // Capture toutes les pierres adverses
                    captureManagerCopy.CapturesOpponent(stoneCopy);

                    // Vérifie si cela crée une situation de Ko
                    if (boardCopy.IsKoViolation())
                    {
                        potentialKoPositions.Add(stone);
                    }
                }
            }

            return potentialKoPositions;
        }

        /// <summary>
        /// Convertit le plateau en chaîne de caractères avec les positions Ko marquées
        /// </summary>
        /// <returns>Représentation du plateau sous forme de chaîne</returns>
        public string StringifyGoban(StoneColor currentTurn)
        {
            // Récupère les positions en Ko
            var koPositions = new HashSet<(int x, int y)>();
            foreach (var stone in ChecksGobanForKo(currentTurn))
            {
                koPositions.Add((stone.X, stone.Y));
            }

            int estimatedCapacity = (logic.Goban.Size * logic.Goban.Size * 8) + 8; // x,y,color! pour chaque stone + header
            var sb = new StringBuilder(estimatedCapacity);
            sb.Append("x,y,color!");

            // Construit la chaine
            for (int i = 0; i < logic.Goban.Size; i++)
            {
                for (int j = 0; j < logic.Goban.Size; j++)
                {
                    Stone stone = logic.Goban.GetStone(i, j);
                    string color = koPositions.Contains((stone.X, stone.Y)) ? "Ko" : stone.Color.ToString();
                    sb.Append($"{stone.X},{stone.Y},{color}!");
                }
            }

            return sb.ToString();
        }
    }
}