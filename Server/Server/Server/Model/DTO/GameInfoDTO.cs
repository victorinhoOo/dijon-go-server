namespace Server.Model.DTO
{
    /// <summary>
    /// Représente les informations d'une partie pour le transfert au client
    /// </summary>
    public class GameInfoDTO
    {
        private int id;
        private string title;
        private int size;

        /// <summary>
        /// L'id de la partie
        /// </summary>
        public int Id { get => id; set => id = value; }

        /// <summary>
        /// Le titre de la partie
        /// </summary>
        public string Title { get => title; set => title = value; }

        /// <summary>
        /// Taille de la grille de jeu
        /// </summary>
        public int Size { get => size; set => size = value; }
    }
}
