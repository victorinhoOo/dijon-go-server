namespace GoLogic
{
    public class Stone
    {
        #region attributs
        private readonly int x; 
        private readonly int y;
        private StoneColor color;

        /// <summary>
        /// Position de la ligne dans le plateau, de 0 à n-1
        /// </summary>
        public int X { get => this.x; }

        /// <summary>
        /// Position de la ligne dans le plateau, de 0 à n-1
        /// </summary>
        public int Y { get => this.y; }

        /// <summary>
        /// Couleur de la pierre Black, White ou Empty
        /// </summary>
        public StoneColor Color { get => this.color; set => this.color = value; }
        #endregion attributs

        /// <summary>
        /// Instancie une pierre aux coordonnées spécifiée de couleur vide
        /// </summary>
        /// <param name="x">Position ligne x dans le plateau</param>
        /// <param name="y">Position colonne y dans le plateau</param>
        public Stone(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.color = StoneColor.Empty;
        }

        /// <summary>
        /// Instancie une pierre aux coordonnées et de la couleur spécifié
        /// </summary>
        /// <param name="x">Position ligne x dans le plateau</param>
        /// <param name="y">Position colonne y dans le plateau</param>
        /// <param name="color">la couleur Black, White ou Empty</param>
        public Stone(int x, int y, StoneColor color)
        {
            this.x = x;
            this.y = y;
            this.color = color;
        }

        /// <summary>
        /// Compare les attributs de 2 objets Stone
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Renvoie vrai si les 2 ont les mêmes attributs</returns>
        public bool Equals(Stone other)
        {
            return this.X == other.X && this.Y == other.Y && this.Color == other.Color;
        }
        
        /// <summary>
        /// Renvoie les coordonnées des pierres adjacentes
        /// </summary>
        /// <returns>Un tableau de tuple d'entier</returns>
        public List<(int x, int y)> GetNeighborsCoordinate()
        {
            List<(int x, int y)> neighbors = new List<(int x, int y)>();
            
            // Tuple des positions adjacentes à la pierre (bas, haut, droite, gauche)
            foreach (var (dx, dy) in new List<(int, int)> { (1, 0), (-1, 0), (0, 1), (0, -1) })
            {
                int nx = x + dx;
                int ny = y + dy;
                neighbors.Add((nx, ny));
            }

            return neighbors;
        }
    }
}
