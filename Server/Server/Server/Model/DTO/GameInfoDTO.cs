using System.Threading.Tasks.Dataflow;

namespace Server.Model.DTO
{
    /// <summary>
    /// Représente les informations d'une partie terminée pour le transfert au client
    /// </summary>
    public class GameInfoDTO
    {
        private int id;
        private string usernamePlayer1;
        private string usernamePlayer2;
        private int size;
        private string rule;
        private float scorePlayer1;
        private float scorePlayer2;
        private bool won;
        private DateTime date;

        /// <summary>
        /// Renvoi l'id de la partie
        /// </summary>
        public int Id { get => id; }

        /// <summary>
        /// Renvoi le nom d'utilisateur du joueur 1 de la partie
        /// </summary>
        public string UsernamePlayer1 { get => usernamePlayer1; }

        /// <summary>
        /// Renvoi le nom d'utilisateur du joueur 2 de la partie
        /// </summary>
        public string UsernamePlayer2 { get => usernamePlayer2; }

        /// <summary>
        /// Renvoi la taille de la grille de la partie
        /// </summary>
        public int Size { get => size; }

        /// <summary>
        /// Renvoi les règles de la partie
        /// </summary>
        public string Rule { get => rule; }

        /// <summary>
        /// Renvoi le score du joueur 1 de la partie 
        /// </summary>
        public float ScorePlayer1 { get => scorePlayer1; }

        /// <summary>
        /// Renvoi le score du joueur 2 de la partie
        /// </summary>
        public float ScorePlayer2 { get => scorePlayer2; }

        /// <summary>
        /// Renvoi si l'utilisateur ayant fait la requête a gagné la partie ou non
        /// </summary>
        public bool Won { get => won; }

        /// <summary>
        /// Renvoi la date à laquelle la partie a été jouée
        /// </summary>
        public DateTime Date { get => date; }

        public GameInfoDTO(int id, string player1, string player2, int size, string rule, float scorePlayer1, float scorePlayer2, bool won, DateTime date)
        {
            this.id = id;
            this.usernamePlayer1 = player1;
            this.usernamePlayer2 = player2;
            this.size = size;
            this.rule = rule;
            this.scorePlayer1 = scorePlayer1;
            this.scorePlayer2 = scorePlayer2;
            this.won = won;
            this.date = date;
        }
    }
}
