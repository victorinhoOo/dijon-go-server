namespace Server.Model.DTO
{
    /// <summary>
    /// Représente les informations d'une partie qu'il est possible de rejoindre pour le transfert au client
    /// </summary>
    public class AvailableGameInfoDTO
    {
        private int id;
        private int size;
        private string rule;
        private string creatorName;
        private float komi;
        private string name;
        private int handicap;
        private string handicapColor;

        /// <summary>
        /// L'id de la partie
        /// </summary>
        public int Id { get => id; set => id = value; }

        /// <summary>
        /// Taille de la grille de jeu
        /// </summary>
        public int Size { get => size; set => size = value; }

        /// <summary>
        /// Règle du jeu de la partie 
        /// </summary>
        public string Rule { get => rule; set => rule = value; }

        /// <summary>
        /// Nom de l'utilisateur qui a crée la partie 
        /// </summary>
        public string CreatorName { get => creatorName; set => creatorName = value; }

        /// <summary>
        /// Valeur du Komi
        /// </summary>
        public float Komi { get => komi; set => komi = value; }

        /// <summary>
        /// Retourne et modifie le nom de la partie
        /// </summary>
        public string Name { get => name; set => name = value; }

        /// <summary>
        /// Retourne et modifie le handicap de la partie
        /// </summary>
        public int Handicap { get => handicap; set => handicap = value; }

        /// <summary>
        /// Retourne et modifie le choix de la couleur du handicap
        /// </summary>
        public string HandicapColor { get => handicapColor; set => handicapColor = value; }
    }
}
