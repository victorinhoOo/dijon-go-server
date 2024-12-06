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
        public int Id { get => id; }

        /// <summary>
        /// Taille de la grille de jeu
        /// </summary>
        public int Size { get => size; }

        /// <summary>
        /// Règle du jeu de la partie 
        /// </summary>
        public string Rule { get => rule; }

        /// <summary>
        /// Nom de l'utilisateur qui a crée la partie 
        /// </summary>
        public string CreatorName { get => creatorName; }

        /// <summary>
        /// Valeur du Komi
        /// </summary>
        public float Komi { get => komi; }

        /// <summary>
        /// Retourne et modifie le nom de la partie
        /// </summary>
        public string Name { get => name; }

        /// <summary>
        /// Retourne et modifie le handicap de la partie
        /// </summary>
        public int Handicap { get => handicap; }

        /// <summary>
        /// Retourne et modifie le choix de la couleur du handicap
        /// </summary>
        public string HandicapColor { get => handicapColor; }
        /// <summary>
        /// constructeur de available game info
        /// </summary>
        /// <param name="id">l'id de la partie</param>
        /// <param name="size">la taille de la grille</param>
        /// <param name="rule">les règles</param>
        /// <param name="creatorName">le nom du createur</param>
        /// <param name="komi"></param>
        /// <param name="name">titre de la partie</param>
        /// <param name="handicap"></param>
        /// <param name="handicapColor">le joueur qui a le handicap</param>
        public AvailableGameInfoDTO(int id, int size, string rule, string creatorName, float komi, string name, int handicap, string handicapColor)
        {
            this.id = id;
            this.size = size;
            this.rule = rule;
            this.creatorName = creatorName;
            this.komi = komi;
            this.name = name;
            this.handicap = handicap;
            this.handicapColor = handicapColor;
        }
    }
}
