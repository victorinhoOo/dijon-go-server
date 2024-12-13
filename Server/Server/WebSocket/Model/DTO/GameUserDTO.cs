namespace WebSocket.Model.DTO
{

    /// <summary>
    /// Représente les informations d'un utilisateur jouant une partie de Go
    /// </summary>
    public class GameUserDTO
    {
        private int id;

        private string name;

        private int elo;

        private string token;

        /// <summary>
        /// Renvoi ou modifie l'id d'un utilisateur jouant une partie
        /// </summary>
        public int Id { get => id; set => id = value; }

        /// <summary>
        /// Renvoi ou modifie le Token d'un utilisateur jouant une partie
        /// </summary>
        public string Token { get => token; set => token = value; }

        /// <summary>
        /// Renvoi ou modifie le nom d'utilisateur d'un utilisateur jouant une partie
        /// </summary>
        public string Name { get => name; set => name = value; }

        /// <summary>
        /// Renvoi ou modifie l'elo d'un joueur jouant une partie
        /// </summary>
        public int Elo { get => elo; set => elo = value; }

    }
}
