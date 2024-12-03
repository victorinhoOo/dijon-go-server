namespace WebSocket.Model.DAO.Redis
{
    /// <summary>
    /// Représente l'état d'une partie, incluant les informations sur le plateau et les pierres capturées.
    /// </summary>
    public class GameState
    {
        private int id;
        private int gameId;
        private string boardState;
        private int capturedBlack;
        private int capturedWhite;

        /// <summary>
        /// Obtient l'identifiant de l'état du jeu.
        /// </summary>
        public int Id
        {
            get { return id; }
        }

        /// <summary>
        /// Obtient l'identifiant du jeu auquel cet état appartient.
        /// </summary>
        public int GameId
        {
            get { return gameId; }
        }

        /// <summary>
        /// Obtient l'état du plateau sous forme de chaîne.
        /// </summary>
        public string BoardState
        {
            get { return boardState; }
        }

        /// <summary>
        /// Obtient le nombre de pierres noires capturées.
        /// </summary>
        public int CapturedBlack
        {
            get { return capturedBlack; }
        }

        /// <summary>
        /// Obtient le nombre de pierres blanches capturées.
        /// </summary>
        public int CapturedWhite
        {
            get { return capturedWhite; }
        }

        public GameState(int gameId, string boardState, int capturedBlack, int capturedWhite)
        {
            this.gameId = gameId;
            this.boardState = boardState;
            this.capturedBlack = capturedBlack;
            this.capturedWhite = capturedWhite;
        }
    }
}
