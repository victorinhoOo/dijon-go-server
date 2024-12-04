using GoLogic.Goban;
using System.Text;

namespace GoLogic.Serializer
{
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
        /// <returns>Liste des positions Ko sur le plateau</returns>
        private List<Stone> ChecksGobanForKo(GameLogic logic, StoneColor currentTurn)
        {
            List<Stone> potentialKoPositions = new List<Stone>();

            // Only check Ko if there was a previous stone
            if (logic.PreviousStone == null) return potentialKoPositions;

            // Récupère tous les voisins vides de la pierre précédente
            foreach (Stone stone in logic.Goban.GetNeighbors(logic.PreviousStone))
            {
                if (stone.Color == StoneColor.Empty)
                {
                    // Pour chaque voisin vide, fait une nouvelle copie et teste
                    IBoard boardCopy = logic.Goban.Clone();
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
        public string StringifyGoban(GameLogic logic, StoneColor currentTurn)
        {
            // Récupère les position en Ko
            var koPositions = new HashSet<(int x, int y)>();
            foreach (var stone in ChecksGobanForKo(logic, currentTurn))
            {
                koPositions.Add((stone.X, stone.Y));
            }

            int estimatedCapacity = (logic.Goban.Size * logic.Goban.Size * 8) + 8; // x,y,color\n pour chaque stone + header
            var sb = new StringBuilder(estimatedCapacity);
            sb.AppendLine("x,y,color");

            // Construit la chaine
            for (int i = 0; i < logic.Goban.Size; i++)
            {
                for (int j = 0; j < logic.Goban.Size; j++)
                {
                    Stone stone = logic.Goban.GetStone(i, j);
                    string color = koPositions.Contains((stone.X, stone.Y)) ? "Ko" : stone.Color.ToString();
                    sb.AppendLine($"{stone.X},{stone.Y},{color}");
                }
            }

            return sb.ToString();
        }
    }
} 