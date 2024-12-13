using GoLogic.Goban;
using System.Text;

namespace GoLogic.Serializer
{
    /// <summary>
    /// Serialise en String l'état du Goban
    /// </summary>
    public class BoardSerializer
    {
        private GameLogic logic;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="logic">l'instance de gameLogic</param>
        public BoardSerializer(GameLogic logic)
        {
            this.logic = logic;
        }

        /// <summary>
        /// Convertit le plateau en chaîne de caractères avec les positions Ko marquées
        /// </summary>
        /// <param name="currentTurn">Couleur du tour du joueur</param>
        /// <returns>Représentation du plateau sous forme de chaîne</returns>
        public string StringifyGoban(StoneColor currentTurn)
        {
            // Récupère les positions en Ko
            HashSet<(int,int)> koPositions = new HashSet<(int x, int y)>();
            foreach (Stone stone in this.logic.ChecksGobanForKo(currentTurn))
            {
                koPositions.Add((stone.X, stone.Y));
            }

            int estimatedCapacity = (logic.Goban.Size * logic.Goban.Size * 8) + 8; // x,y,color! pour chaque stone + header
            StringBuilder sb = new StringBuilder(estimatedCapacity);
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