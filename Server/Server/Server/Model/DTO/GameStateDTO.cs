namespace Server.Model.DTO
{
    /// <summary>
    /// Représente l'état d'une partie à un moment donnée pour le transfert au client
    /// </summary>
    public class GameStateDTO
    {
        private string board;

        private int capturedBlack;

        private int capturedWhite;

        private int moveNumber;

        /// <summary>
        /// Renvoi l'état du board au moment de cet état de la partie
        /// </summary>
        public string Board { get => board; }

        /// <summary>
        /// Renvoi le nombre de pierres capturées par les noirs à ce moment de la partie
        /// </summary>
        public int CapturedBlack { get => capturedBlack; }

        /// <summary>
        /// Renvoie le nombre de pierres capturées par les blancs à ce moment de la partie
        /// </summary>
        public int CapturedWhite { get => capturedWhite; }

        /// <summary>
        /// Renvoi le numéro du coup de cet état de la partie
        /// </summary>
        public int MoveNumber { get => moveNumber; }

        public GameStateDTO(string board, int capturedBlack, int capturedWhite, int moveNumber)
        {
            this.board = board;
            this.capturedBlack = capturedBlack;
            this.capturedWhite = capturedWhite;
            this.moveNumber = moveNumber;
        }
    }
}
